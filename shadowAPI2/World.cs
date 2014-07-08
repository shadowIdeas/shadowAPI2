using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace shadowAPI2
{
    public class World
    {
        private static World instance;

        private World()
        {

        }

        public static World GetInstance()
        {
            if(instance == null)
                instance = new World();

            return instance;
        }

        public void CreateVojel()
        {
            /*
            float x = Player.PlayerX();
            float y = Player.PlayerY();
            float z = Player.PlayerZ();

            float destX = x + 30.0f;
            float destY = y + 15.0f;
            float destZ = z + 5.0f;

            Memory.Call(0x711EF0, new object[] { x, y, z, destX, destY, destZ, 1, 1, 1 }, true);
             */
        }
    }
}
