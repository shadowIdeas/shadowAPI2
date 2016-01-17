using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.IO;
using System.Security.AccessControl;

namespace shadowAPI2
{
    public class Chat
    {
        private static FileSystemWatcher watcher;
        private static StreamReader reader;
        private static FileInfo info;

        private const string CHATLOG_FILE = "chatlog.txt";
        private static string chatlogPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "GTA San Andreas User Files\\SAMP\\");

        private static IntPtr chatMessage;
        private static IntPtr dialog;

        private static IntPtr isChatOpen;
        private static IntPtr isDialogOpen;

        private static IntPtr functionSendCommand;
        private static IntPtr functionSendText;
        private static IntPtr functionAddChatMessage;
        private static IntPtr functionShowDialog;
        private static IntPtr functionCloseDialog;
        private static IntPtr functionSelectDialogListIndex;

        /// <summary>
        /// Chat-Message delegate
        /// </summary>
        /// <param name="time">When the message was sent</param>
        /// <param name="message">The message itself</param>
        public delegate void OnChatMessageReceived(DateTime time, String message);

        /// <summary>
        /// This Event will provide you with the latest chat-messages
        /// </summary>
        public static event OnChatMessageReceived OnChatMessage;

        internal static void GenerateAddresses(IntPtr sampBase)
        {
            IntPtr temp = IntPtr.Zero;
            isChatOpen = IntPtr.Zero;
            isDialogOpen = IntPtr.Zero;
            chatMessage = IntPtr.Zero;

            while (isChatOpen == IntPtr.Zero || isDialogOpen == IntPtr.Zero || chatMessage == IntPtr.Zero)
            {
                System.Threading.Thread.Sleep(250);

                if (Memory.ReadMemory<IntPtr>(sampBase + 0x21A10C, out temp))
                    if (temp != IntPtr.Zero)
                        isChatOpen = temp + 0x55;

                if (Memory.ReadMemory<IntPtr>(sampBase + 0x21A0B8, out dialog))
                    if (dialog != IntPtr.Zero)
                        isDialogOpen = dialog + 0x28;

                Memory.ReadMemory<IntPtr>(sampBase + 0x21A0E4, out chatMessage);
            }

            functionSendCommand = sampBase + 0x65C60;
            functionSendText = sampBase + 0x57F0;
            functionAddChatMessage = sampBase + 0x64520;
            functionShowDialog = sampBase + 0x6B9C0;
            functionCloseDialog = sampBase + 0x6C040;
            functionSelectDialogListIndex = sampBase + 0x863C0;

            SetupLogging();
        }

        internal static void SetupLogging()
        {
            try
            {
                reader = new StreamReader(new FileStream(Path.Combine(chatlogPath, CHATLOG_FILE), FileMode.Open, FileAccess.Read, FileShare.ReadWrite), Encoding.Default, true);
                reader.ReadToEnd();

                info = new FileInfo(Path.Combine(chatlogPath, CHATLOG_FILE));

                watcher = new FileSystemWatcher();
                watcher.Path = chatlogPath;
                watcher.Filter = CHATLOG_FILE;
                watcher.Changed += ChangeReceived;
                watcher.EnableRaisingEvents = true;
            }
            catch (Exception)
            {
                chatlogPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonDocuments), "GTA San Andreas User Files\\SAMP\\");
                reader = new StreamReader(new FileStream(Path.Combine(chatlogPath, CHATLOG_FILE), FileMode.Open, FileAccess.Read, FileShare.ReadWrite), Encoding.Default, true);
                reader.ReadToEnd();

                info = new FileInfo(Path.Combine(chatlogPath, CHATLOG_FILE));

                watcher = new FileSystemWatcher();
                watcher.Path = chatlogPath;
                watcher.Filter = CHATLOG_FILE;
                watcher.Changed += ChangeReceived;
                watcher.EnableRaisingEvents = true;
            }
        }

        private static void ChangeReceived(object ob, FileSystemEventArgs e)
        {
            info.Refresh();
            if (info.Length < reader.BaseStream.Position)
            {
                reader.BaseStream.Position = 0;
                reader.DiscardBufferedData();
            }
            String line = "";
            while (!reader.EndOfStream)
            {
                line = reader.ReadLine();
                if (line == "")
                    continue;
                else
                {
                    List<String> splitted = line.Split(' ').ToList();
                    DateTime date = DateTime.Parse(splitted[0].Remove(0, 1).Remove(splitted[0].Length - 2));
                    splitted.RemoveAt(0);
                    if (OnChatMessage != null)
                        OnChatMessage(date, string.Join(" ", splitted));
                }
            }
        }


        /// <summary>
        /// Check if the chat is open
        /// </summary>
        /// <returns>True if its open, else false</returns>
        public static bool IsOpen()
        {
            Memory.Init();

            byte byteResult = 0;
            bool result = Memory.ReadMemory<byte>(isChatOpen, out byteResult);

            if (result && byteResult != 0)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Check if a dialog is open
        /// </summary>
        /// <returns>True if its open, else false</returns>
        public static bool IsDialogOpen()
        {
            Memory.Init();

            byte byteResult = 0;
            bool result = Memory.ReadMemory<byte>(isDialogOpen, out byteResult);

            if (result && byteResult != 0)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Send a message/command to the server
        /// </summary>
        /// <param name="message">The message/command</param>
        /// <param name="args">Arguments for a command, e.g an ID</param>
        public static void Send(string message, params object[] args)
        {
            Memory.Init();

            if (message.Length != 0)
            {
                if (args.Length > 0)
                    message += " " + string.Join(" ", args);
                if (message[0] == '/')
                    Memory.Call(functionSendCommand, false, message);
                else
                    Memory.Call(functionSendText, false, message);
            }
        }

        /// <summary>
        /// Add a new message in the SAMP chat (only local)
        /// </summary>
        /// <param name="text">The text to be written</param>
        /// <param name="color">A color in Hex</param>
        public static void AddMessage(string text, string color = "FFFFFF")
        {
            Memory.Init();

            Memory.Call(functionAddChatMessage, true, (int)chatMessage, "{" + color + "}" + text);
        }

        /// <summary>
        /// Add a new message in the SAMP chat (only local)
        /// </summary>
        /// <param name="text">The text to be written</param>
        /// <param name="color">A Color-Type</param>
        public static void AddMessage(string text, Color color)
        {
            Memory.Init();

            Memory.Call(functionAddChatMessage, true, (int)chatMessage, "{" + Util.ColorToHexRGB(color) + "}" + text);
        }

        /// <summary>
        /// Add a new message in the SAMP chat (only local)
        /// </summary>
        /// <param name="prefix">The prefix to be written</param>
        /// <param name="prefixColor">A prefix color in Hex</param>
        /// <param name="text">The text to be written</param>
        /// <param name="color">A color in Hex</param>
        public static void AddMessage(string prefix, string prefixColor, string text, string color = "FFFFFF")
        {
            Memory.Init();

            Memory.Call(functionAddChatMessage, true, (int)chatMessage, "{" + prefixColor + "}" + prefix + " {" + color + "}" + text);
        }

        /// <summary>
        /// Add a new message in the SAMP chat (only local)
        /// </summary>
        /// <param name="prefix">The prefix to be written</param>
        /// <param name="prefixColor">A Color-Type</param>
        /// <param name="text">The text to be written</param>
        /// <param name="color">A Color-Type</param>
        public static void AddMessage(string prefix, Color prefixColor, string text, Color color)
        {
            Memory.Init();

            Memory.Call(functionAddChatMessage, true, (int)chatMessage, "{" + Util.ColorToHexRGB(prefixColor) + "}" + prefix + " {" + Util.ColorToHexRGB(color) + "}" + text);
        }


        public static void ShowDialog(DialogStyle style, string caption, string text, string button = "", string button2 = "")
        {
            Memory.Init();

            List<byte> ptr = new List<byte>();
            ptr.Add(0xB9);
            ptr.AddRange(BitConverter.GetBytes((uint)dialog));
            Memory.Call(functionShowDialog, ptr.ToArray(), false, 1, (int)style, caption, text, button, button2, 0);
        }

        public static void SelectDialogListIndex(uint index)
        {
            Memory.Init();

            // Check if id == 2
            int id = 0;
            if (Memory.ReadMemory<int>(dialog + 0x2C, out id) && id == 2)
            {
                // Set the entry id to entryId
                int something = 0;
                if (Memory.ReadMemory<int>(dialog + 0x20, out something))
                {
                    List<byte> ptr = new List<byte>();
                    ptr.Add(0xB9);
                    ptr.AddRange(BitConverter.GetBytes(something));

                    Memory.Call(functionSelectDialogListIndex, ptr.ToArray(), false, index);
                }
            }
        }

        public static string GetSelectedDialogListString()
        {
            Memory.Init();

            int id = 0;
            var listString = "";
            if (Memory.ReadMemory<int>(dialog + 0x2C, out id) && id == 2)
            {
                IntPtr temp = IntPtr.Zero;
                if (Memory.ReadMemory<IntPtr>(IntPtr.Add(dialog, 0x20), out temp))
                {
                    var index = 0;
                    Memory.ReadMemory<int>(IntPtr.Add(temp, 0x143), out index);

                    if (Memory.ReadMemory<IntPtr>(IntPtr.Add(temp, 0x14C), out temp))
                    {
                        if (Memory.ReadMemory<IntPtr>(IntPtr.Add(temp, (int)(4 * index)), out temp))
                        {
                            listString = Memory.ReadString(temp, 64);
                        }
                    }
                }
            }

            return listString;
        }

        public static string GetSelectedDialogListStringByIndex(uint index)
        {
            Memory.Init();

            int id = 0;
            var listString = "";
            if (Memory.ReadMemory<int>(dialog + 0x2C, out id) && id == 2)
            {
                IntPtr temp = IntPtr.Zero;
                if (Memory.ReadMemory<IntPtr>(IntPtr.Add(dialog, 0x20), out temp))
                {
                    if (Memory.ReadMemory<IntPtr>(IntPtr.Add(temp, 0x14C), out temp))
                    {
                        if (Memory.ReadMemory<IntPtr>(IntPtr.Add(temp, (int)(4 * index)), out temp))
                        {
                            listString = Memory.ReadString(temp, 64);
                        }
                    }
                }
            }

            return listString;
        }

        public static void CloseDialog()
        {
            Memory.Init();

            List<byte> ptr = new List<byte>();
            ptr.Add(0xB9);
            ptr.AddRange(BitConverter.GetBytes((uint)dialog));

            Memory.Call(functionCloseDialog, ptr.ToArray(), false, 1);
        }

        public static uint GetCurrentDialogId()
        {
            Memory.Init();

            int result = 0;
            Memory.ReadMemory<int>(dialog + 0x30, out result);

            return (uint)result;
        }
    }
}
