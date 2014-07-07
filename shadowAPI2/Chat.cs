using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace shadowAPI2
{
    public static class Chat
    {
        public static bool IsChatOpen()
        {
            if (!Memory.isInit)
                Memory.Init();

            bool result = Memory.ReadBoolean(Memory.isChatOpen);

            return result;
        }

        public static bool IsDialogOpen()
        {
            if (!Memory.isInit)
                Memory.Init();

            bool result = Memory.ReadBoolean(Memory.isDialogOpen);

            return result;
        }

        public static void SendChat(string message, int state = 0)
        {
            if (!Memory.isInit)
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

        public static void AddChatMessage(string text, string color = "FFFFFF")
        {
            if (!Memory.isInit)
                Memory.Init();

            Memory.Call(Memory.functionAddChatMessage, new object[] {(int)Memory.chatMessage, "{" + color + "}" + text }, true);
        }

        public static void AddChatMessage(string text, Color color)
        {
            if (!Memory.isInit)
                Memory.Init();

            Memory.Call(Memory.functionAddChatMessage, new object[] { (int)Memory.chatMessage, "{" + Util.ColorToHexRGB(color) + "}" + text }, true);
        }
    }
}
