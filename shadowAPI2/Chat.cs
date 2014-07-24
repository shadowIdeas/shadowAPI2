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
        private static Chat instance;

        private FileSystemWatcher watcher;
        private StreamReader reader;
        private FileInfo info;

        private const string CHATLOG_FILE = "chatlog.txt";
        private string chatlogPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "GTA San Andreas User Files\\SAMP\\");
        private long lastSize;

        /// <summary>
        /// Chat-Message delegate
        /// </summary>
        /// <param name="time">When the message was sent</param>
        /// <param name="message">The message itself</param>
        public delegate void OnChatMessageReceived(DateTime time, String message);

        /// <summary>
        /// This Event will provide you with the latest chat-messages
        /// </summary>
        public event OnChatMessageReceived OnChatMessage;

        private Chat()
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

        internal void ChangeReceived(object ob, FileSystemEventArgs e)
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

        public void Close()
        {

        }

        public static Chat GetInstance()
        {
            if (instance == null)
                instance = new Chat();

            return instance;
        }

        /// <summary>
        /// Check if the chat is open
        /// </summary>
        /// <returns>True if its open, else false</returns>
        public bool IsOpen()
        {
            if (!Memory.IsInit)
                Memory.Init(Memory._processName);

            bool result = Memory.ReadBoolean(Memory.isChatOpen);

            return result;
        }

        /// <summary>
        /// Check if a dialog is open
        /// </summary>
        /// <returns>True if its open, else false</returns>
        public bool IsDialogOpen()
        {
            if (!Memory.IsInit)
                Memory.Init(Memory._processName);

            bool result = Memory.ReadBoolean(Memory.isDialogOpen);

            return result;
        }

        /// <summary>
        /// Send a message/command to the server
        /// </summary>
        /// <param name="message">The message/command</param>
        /// <param name="state">Don't used</param>
        public void Send(string message, int state = 0)
        {
            if (!Memory.IsInit)
                Memory.Init(Memory._processName);

            switch (state)
            {
                case 0:
                    {
                        if (message[0] == '/')
                            Memory.Call(Memory.functionSendCommand, new object[] { message }, false);
                        else
                            Memory.Call(Memory.functionSendSay, new object[] { message }, false);
                    }
                    break;
                case 1:
                    {
                        Memory.Call(Memory.functionSendCommand, new object[] { message }, false);
                    }
                    break;
                case 2:
                    {
                        Memory.Call(Memory.functionSendSay, new object[] { message }, false);
                    }
                    break;
            }
        }

        /// <summary>
        /// Add a new message in the SAMP chat (only local)
        /// </summary>
        /// <param name="text">The text to be written</param>
        /// <param name="color">A color in Hex</param>
        public void AddMessage(string text, string color = "FFFFFF")
        {
            if (!Memory.IsInit)
                Memory.Init(Memory._processName);

            Memory.Call(Memory.functionAddChatMessage, new object[] { (int)Memory.chatMessage, "{" + color + "}" + text }, true);
        }

        /// <summary>
        /// Add a new message in the SAMP chat (only local)
        /// </summary>
        /// <param name="text">The text to be written</param>
        /// <param name="color">A Color-Type</param>
        public void AddMessage(string text, Color color)
        {
            if (!Memory.IsInit)
                Memory.Init(Memory._processName);

            Memory.Call(Memory.functionAddChatMessage, new object[] { (int)Memory.chatMessage, "{" + Util.ColorToHexRGB(color) + "}" + text }, true);
        }
    }
}
