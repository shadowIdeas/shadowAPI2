using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace shadowAPI2
{
    public class Player
    {
        public static int PlayerHealth()
        {
            if (!Memory.isInit)
                Memory.Init();

            int result = (int)Memory.ReadFloat(Memory.playerHealth);

            return result;
        }

        public static int PlayerArmor()
        {
            if (!Memory.isInit)
                Memory.Init();

            int result = (int)Memory.ReadFloat(Memory.playerArmor);

            return result;
        }

        public static float PlayerX()
        {
            if (!Memory.isInit)
                Memory.Init();

            float result = Memory.ReadFloat(Memory.playerPositionX);

            return x;
        }

        public static float PlayerY()
        {
            if (!Memory.isInit)
                Memory.Init();

            float result = Memory.ReadFloat(Memory.playerPositionY);

            return y;
        }

        public static float PlayerZ()
        {
            if (!Memory.isInit)
                Memory.Init();

            float result = Memory.ReadFloat(Memory.playerPositionZ);

            return z;
        }

        public static string PlayerCity()
        {
            if (!Memory.isInit)
                Memory.Init();

            float x = Memory.ReadFloat(Memory.playerPositionX);
            float y = Memory.ReadFloat(Memory.playerPositionY);
            float z = Memory.ReadFloat(Memory.playerPositionZ);

            return ZoneManager.City(x, y, z);
        }

        public static string PlayerZone()
        {
            if (!Memory.isInit)
                Memory.Init();

            float x = Memory.ReadFloat(Memory.playerPositionX);
            float y = Memory.ReadFloat(Memory.playerPositionY);
            float z = Memory.ReadFloat(Memory.playerPositionZ);

            return ZoneManager.Zone(x, y, z);
        }

        public static bool IsInInterior()
        {
            if (!Memory.isInit)
                Memory.Init();

            int result = Memory.ReadInteger(Memory.playerLocation);

            if (result == 0)
                return false;
            else
                return true;
        }

        public static bool IsInVehicle()
        {
            if (!Memory.isInit)
                Memory.Init();

            uint result = BitConverter.ToUInt32(Memory.ReadMemory(Memory.vehicleOffsetBase, 4), 0);

            if (result == 0)
                return false;
            else
                return true;
        }

        public static bool IsInRangeOf(float x, float y, float z, float radius)
        {
            x = PlayerX() - x;
            y = PlayerY() - y;
            z = PlayerZ() - z;

            if ((x < radius) && (x > -radius) && (y < radius) && (y > -radius) && (z < radius) && (z > -radius))
                return true;
            else
                return false;
        }
    }
}
