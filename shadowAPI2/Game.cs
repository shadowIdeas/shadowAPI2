using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace shadowAPI2
{
    public class Game
    {
        private static Game instance;

        private Game()
        {

        }

        public static Game GetInstance()
        {
            if (instance == null)
                instance = new Game();

            return instance;
        }

        public int GetFPS()
        {
            if (!Memory.IsInit)
                Memory.Init(Memory._processName);

            int fps = Memory.ReadInteger(Memory.FRAMES_PER_SECOND);

            return fps;
        }
    }
}
