using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace shadowAPI2
{
    public class Game
    {
        public static int GetFPS()
        {
            Memory.Init();

            var fps = 0;
            Memory.ReadMemory<int>(0xB729A0, out fps);
            return fps;
        }
    }
}
