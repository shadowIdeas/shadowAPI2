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

        /// <summary>
        /// Get the current health of the player
        /// </summary>
        /// <returns>Health</returns>
        public int GetHealth()
        {
            if (!Memory.IsInit)
                Memory.Init(Memory._processName);

            int result = (int)Memory.ReadFloat(Memory.playerHealth);

            return result;
        }

        /// <summary>
        /// Get the current armor of the player
        /// </summary>
        /// <returns>Armor</returns>
        public int GetArmor()
        {
            if (!Memory.IsInit)
                Memory.Init(Memory._processName);

            int result = (int)Memory.ReadFloat(Memory.playerArmor);

            return result;
        }

        /// <summary>
        /// Get the current X-Coordinate of the player
        /// </summary>
        /// <returns>X-Coordinate</returns>
        public float GetX()
        {
            if (!Memory.IsInit)
                Memory.Init(Memory._processName);

            float result = Memory.ReadFloat(Memory.playerPositionX);

            return result;
        }

        /// <summary>
        /// Get the current X-Coordinate of the player
        /// </summary>
        /// <returns>X-Coordinate</returns>
        public float GetY()
        {
            if (!Memory.IsInit)
                Memory.Init(Memory._processName);

            float result = Memory.ReadFloat(Memory.playerPositionY);

            return result;
        }

        /// <summary>
        /// Get the current X-Coordinate of the player
        /// </summary>
        /// <returns>X-Coordinate</returns>
        public float GetZ()
        {
            if (!Memory.IsInit)
                Memory.Init(Memory._processName);

            float result = Memory.ReadFloat(Memory.playerPositionZ);

            return result;
        }

        /// <summary>
        /// Get the current city of the player
        /// </summary>
        /// <returns>City</returns>
        public string GetCity()
        {
            if (!Memory.IsInit)
                Memory.Init(Memory._processName);

            float x = Memory.ReadFloat(Memory.playerPositionX);
            float y = Memory.ReadFloat(Memory.playerPositionY);
            float z = Memory.ReadFloat(Memory.playerPositionZ);

            return zoneManager.City(x, y, z);
        }

        /// <summary>
        /// Get the current zone of the player
        /// </summary>
        /// <returns>Zone</returns>
        public string GetZone()
        {
            if (!Memory.IsInit)
                Memory.Init(Memory._processName);

            float x = Memory.ReadFloat(Memory.playerPositionX);
            float y = Memory.ReadFloat(Memory.playerPositionY);
            float z = Memory.ReadFloat(Memory.playerPositionZ);

            return zoneManager.Zone(x, y, z);
        }

        /// <summary>
        /// Check if's the player is in a interior
        /// </summary>
        /// <returns>True if it in a interior, false if not</returns>
        public bool IsInInterior()
        {
            if (!Memory.IsInit)
                Memory.Init(Memory._processName);

            int result = Memory.ReadInteger(Memory.playerLocation);

            if (result == 0)
                return false;
            else
                return true;
        }

        /// <summary>
        /// Check if's the player is in a vehicle
        /// </summary>
        /// <returns>True if it in a vehicle, false if not</returns>
        public bool IsInVehicle()
        {
            if (!Memory.IsInit)
                Memory.Init(Memory._processName);

            bool inVehicle = false;

            if (Vehicle.GetInstance().IsInVehicle() != 0)
                inVehicle = true;

            return inVehicle;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="x">The X-Coordinate where check the range</param>
        /// <param name="y">The Y-Coordinate where check the range</param>
        /// <param name="z">The Z-Coordinate where check the range</param>
        /// <param name="radius">The radius of the point</param>
        /// <returns></returns>
        public bool IsInRangeOf(float x, float y, float z, float radius)
        {
            x = GetX() - x;
            y = GetY() - y;
            z = GetZ() - z;

            if ((x < radius) && (x > -radius) && (y < radius) && (y > -radius) && (z < radius) && (z > -radius))
                return true;
            else
                return false;
        }

        /// <summary>
        /// Get the SAMP id of the player
        /// </summary>
        /// <returns>SAMP id</returns>
        public int GetId()
        {
            if (!Memory.IsInit)
                Memory.Init(Memory._processName);

            int id = -1;

            id = Memory.ReadInteger16(Memory.playerId);

            return id;
        }

        /// <summary>
        /// Get the current SAMP name of the player
        /// </summary>
        /// <returns>SAMP name</returns>
        public string GetName()
        {
            if (!Memory.IsInit)
                Memory.Init(Memory._processName);

            string name = "";

            name = Memory.ReadString(Memory.playerName, 25).Replace("\0", "");

            return name;
        }

        /// <summary>
        /// Get the current GTA Money what the players has on the hand.
        /// </summary>
        /// <returns>GTA Money</returns>
        public int GetHandCash()
        {
            if (!Memory.IsInit)
                Memory.Init(Memory._processName);
            return Memory.ReadInteger16(Memory.playerMoney);
        }

        /// <summary>
        /// Get the weapon ID of the gun the player is holding
        /// </summary>
        /// <returns>GTA Weapon ID</returns>
        public int GetWeaponId()
        {
            if (!Memory.IsInit)
                Memory.Init(Memory._processName);
            return Memory.ReadInteger16(Memory.playerWeapon);
        }
    }
}