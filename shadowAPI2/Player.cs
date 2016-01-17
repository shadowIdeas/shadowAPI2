using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace shadowAPI2
{
    public class Player
    {
        private static IntPtr playerName;
        private static IntPtr structSAMP;
        private static IntPtr structSAMPPools;
        private static IntPtr poolPlayers;
        private static IntPtr playerId;

        internal static void GenerateAddresses(IntPtr sampBase)
        {
            playerName = IntPtr.Add(sampBase, 0x219A6F);
            Memory.ReadMemory<IntPtr>(IntPtr.Add(sampBase, 0x21A0F8), out structSAMP);
            Memory.ReadMemory<IntPtr>(IntPtr.Add(structSAMP, 0x3CD), out structSAMPPools);
            Memory.ReadMemory<IntPtr>(IntPtr.Add(structSAMPPools, 0x18), out poolPlayers);
            playerId = IntPtr.Add(poolPlayers, 0x04);
        }

        private static IntPtr GetPlayerPointer()
        {
            Memory.Init();

            IntPtr pointer = IntPtr.Zero;
            Memory.ReadMemory<IntPtr>(0xB6F5F0, out pointer);
            return pointer;
        }

        /// <summary>
        /// Get the current health of the player
        /// </summary>
        /// <returns>Health</returns>
        public static int GetHealth()
        {
            Memory.Init();

            float health = -1;

            IntPtr pointer = GetPlayerPointer();
            if (pointer != IntPtr.Zero)
                Memory.ReadMemory<float>(pointer + 0x540, out health);

            return (int)health;
        }

        /// <summary>
        /// Get the current armor of the player
        /// </summary>
        /// <returns>Armor</returns>
        public static int GetArmor()
        {
            Memory.Init();

            float armor = -1;

            IntPtr pointer = GetPlayerPointer();
            if (pointer != IntPtr.Zero)
                Memory.ReadMemory<float>(pointer + 0x548, out armor);

            return (int)armor;
        }

        public static bool GetPosition(ref float x, ref float y, ref float z)
        {
            Memory.Init();

            x = 0.0f;
            y = 0.0f;
            z = 0.0f;

            IntPtr pointer = GetPlayerPointer();
            if (pointer != IntPtr.Zero)
            {
                IntPtr matrix = IntPtr.Zero;
                if (Memory.ReadMemory<IntPtr>(pointer + 0x14, out matrix))
                {
                    if (matrix != IntPtr.Zero)
                    {
                        if (Memory.ReadMemory<float>(matrix + 0x30, out x) && Memory.ReadMemory<float>(matrix + 0x34, out y) && Memory.ReadMemory<float>(matrix + 0x38, out z))
                            return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Get the current X-Coordinate of the player
        /// </summary>
        /// <returns>X-Coordinate</returns>
        public static bool GetX(ref float x)
        {
            Memory.Init();

            x = 0.0f;

            IntPtr pointer = GetPlayerPointer();
            if (pointer != IntPtr.Zero)
            {
                IntPtr matrix = IntPtr.Zero;
                if (Memory.ReadMemory<IntPtr>(pointer + 0x14, out matrix))
                {
                    if (matrix != IntPtr.Zero)
                    {
                        if (Memory.ReadMemory<float>(matrix + 0x30, out x))
                            return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Get the current X-Coordinate of the player
        /// </summary>
        /// <returns>X-Coordinate</returns>
        public static bool GetY(ref float y)
        {
            Memory.Init();

            y = 0.0f;

            IntPtr pointer = GetPlayerPointer();
            if (pointer != IntPtr.Zero)
            {
                IntPtr matrix = IntPtr.Zero;
                if (Memory.ReadMemory<IntPtr>(pointer + 0x14, out matrix))
                {
                    if (matrix != IntPtr.Zero)
                    {
                        if (Memory.ReadMemory<float>(matrix + 0x34, out y))
                            return true;
                    }
                }
            }

            return false;
        }   

        /// <summary>
        /// Get the current X-Coordinate of the player
        /// </summary>
        /// <returns>X-Coordinate</returns>
        public static bool GetZ(ref float z)
        {
            Memory.Init();

            z = 0.0f;

            IntPtr pointer = GetPlayerPointer();
            if (pointer != IntPtr.Zero)
            {
                IntPtr matrix = IntPtr.Zero;
                if (Memory.ReadMemory<IntPtr>(pointer + 0x14, out matrix))
                {
                    if (matrix != IntPtr.Zero)
                    {
                        if (Memory.ReadMemory<float>(matrix + 0x38, out z))
                            return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Get the current city of the player
        /// </summary>
        /// <returns>City</returns>
        public static string GetCity()
        {
            Memory.Init();

            var x = 0.0f;
            var y = 0.0f;
            var z = 0.0f;

            GetPosition(ref x, ref y, ref z);

            return ZoneManager.City(x, y, z);
        }

        /// <summary>
        /// Get the current zone of the player
        /// </summary>
        /// <returns>Zone</returns>
        public static string GetZone()
        {
            Memory.Init();

            var x = 0.0f;
            var y = 0.0f;
            var z = 0.0f;

            GetPosition(ref x, ref y, ref z);

            return ZoneManager.Zone(x, y, z);
        }

        /// <summary>
        /// Check if's the player is in a interior
        /// </summary>
        /// <returns>True if it in a interior, false if not</returns>
        public static bool InInterior()
        {
            Memory.Init();

            byte interior = 0;

            IntPtr pointer = GetPlayerPointer();
            if (pointer != IntPtr.Zero)
                Memory.ReadMemory<byte>(pointer + 0x2F, out interior);

            return interior != 0 ? true : false;
        }

        /// <summary>
        /// Check if's the player is in a vehicle
        /// </summary>
        /// <returns>True if it in a vehicle, false if not</returns>
        public static bool InVehicle()
        {
            Memory.Init();

            var returned = false;
            if (Vehicle.GetVehiclePointer() != IntPtr.Zero)
                returned = true;
            return returned;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="x">The X-Coordinate where check the range</param>
        /// <param name="y">The Y-Coordinate where check the range</param>
        /// <param name="radius">The radius of the point</param>
        /// <returns></returns>
        public static bool InRange2D(float x, float y, float z, float radius)
        {
            var currentX = 0.0f;
            var currentY = 0.0f;
            GetX(ref currentX);
            GetY(ref currentY);

            x = currentX - x;
            y = currentY - y;

            if ((x < radius) && (x > -radius) && (y < radius) && (y > -radius) && (z < radius) && (z > -radius))
                return true;
            else
                return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="x">The X-Coordinate where check the range</param>
        /// <param name="y">The Y-Coordinate where check the range</param>
        /// <param name="z">The Z-Coordinate where check the range</param>
        /// <param name="radius">The radius of the point</param>
        /// <returns></returns>
        public static bool InRange3D(float x, float y, float z, float radius)
        {
            var currentX = 0.0f;
            var currentY = 0.0f;
            var currentZ = 0.0f;

            GetPosition(ref currentX, ref currentY, ref currentZ);

            x = currentX - x;
            y = currentY - y;
            z = currentZ - z;

            if ((x < radius) && (x > -radius) && (y < radius) && (y > -radius) && (z < radius) && (z > -radius))
                return true;
            else
                return false;
        }

        /// <summary>
        /// Get the SAMP id of the player (Not functional)
        /// </summary>
        /// <returns>SAMP id</returns>
        public static int GetId()
        {
            Memory.Init();

            Int16 id = -1;
            Memory.ReadMemory<Int16>(playerId, out id);

            return id;
        }

        /// <summary>
        /// Get the current SAMP name of the player
        /// </summary>
        /// <returns>SAMP name</returns>
        public static string GetName()
        {
            Memory.Init();

            var name = Memory.ReadString(playerName, 32);
            return name;
        }

        /// <summary>
        /// Get the current GTA Money what the players has on the hand.
        /// </summary>
        /// <returns>GTA Money</returns>
        public static int GetMoney()
        {
            Memory.Init();

            Int16 returned = 0;
            Memory.ReadMemory<Int16>(0xB7CE50, out returned);

            return (int)returned;
        }

        /// <summary>
        /// Get the weapon ID of the gun the player is holding
        /// </summary>
        /// <returns>GTA Weapon ID</returns>
        public static int GetWeaponId()
        {
            Memory.Init();

            Int16 returned = 0;
            Memory.ReadMemory<Int16>(0x0BAA410, out returned);

            return (int)returned;
        }
    }
}