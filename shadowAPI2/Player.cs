using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace shadowAPI2
{
    public class Player
    {
        private static Player instance;

        private ZoneManager zoneManager;

        private Player()
        {
            zoneManager = ZoneManager.GetInstance();
        }

        public static Player GetInstance()
        {
            if (instance == null)
                instance = new Player();

            return instance;
        }

        public int PlayerHealth()
        {
            if (!Memory.IsInit)
                Memory.Init();

            int result = (int)Memory.ReadFloat(Memory.playerHealth);

            return result;
        }

        public int PlayerArmor()
        {
            if (!Memory.IsInit)
                Memory.Init();

            int result = (int)Memory.ReadFloat(Memory.playerArmor);

            return result;
        }

        public float PlayerX()
        {
            if (!Memory.IsInit)
                Memory.Init();

            float result = Memory.ReadFloat(Memory.playerPositionX);

            return result;
        }

        public float PlayerY()
        {
            if (!Memory.IsInit)
                Memory.Init();

            float result = Memory.ReadFloat(Memory.playerPositionY);

            return result;
        }

        public float PlayerZ()
        {
            if (!Memory.IsInit)
                Memory.Init();

            float result = Memory.ReadFloat(Memory.playerPositionZ);

            return result;
        }

        public string PlayerCity()
        {
            if (!Memory.IsInit)
                Memory.Init();

            float x = Memory.ReadFloat(Memory.playerPositionX);
            float y = Memory.ReadFloat(Memory.playerPositionY);
            float z = Memory.ReadFloat(Memory.playerPositionZ);

            return zoneManager.City(x, y, z);
        }

        public string PlayerZone()
        {
            if (!Memory.IsInit)
                Memory.Init();

            float x = Memory.ReadFloat(Memory.playerPositionX);
            float y = Memory.ReadFloat(Memory.playerPositionY);
            float z = Memory.ReadFloat(Memory.playerPositionZ);

            return zoneManager.Zone(x, y, z);
        }

        public bool IsInInterior()
        {
            if (!Memory.IsInit)
                Memory.Init();

            int result = Memory.ReadInteger(Memory.playerLocation);

            if (result == 0)
                return false;
            else
                return true;
        }

        public bool IsInVehicle()
        {
            if (!Memory.IsInit)
                Memory.Init();

            uint result = BitConverter.ToUInt32(Memory.ReadMemory(Memory.vehicleOffsetBase, 4), 0);

            if (result == 0)
                return false;
            else
                return true;
        }

        public bool IsInRangeOf(float x, float y, float z, float radius)
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
