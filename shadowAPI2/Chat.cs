using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace shadowAPI2
{
    public class Chat
    {
        private static Chat instance;

        private Chat()
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
                Memory.Init();

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
                Memory.Init();

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
                Memory.Init();

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
                Memory.Init();

            Memory.Call(Memory.functionAddChatMessage, new object[] {(int)Memory.chatMessage, "{" + color + "}" + text }, true);
        }

        /// <summary>
        /// Add a new message in the SAMP chat (only local)
        /// </summary>
        /// <param name="text">The text to be written</param>
        /// <param name="color">A Color-Type</param>
        public void AddMessage(string text, Color color)
        {
            if (!Memory.IsInit)
                Memory.Init();

            Memory.Call(Memory.functionAddChatMessage, new object[] { (int)Memory.chatMessage, "{" + Util.ColorToHexRGB(color) + "}" + text }, true);
        }
    }
}
