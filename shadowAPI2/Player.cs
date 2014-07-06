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

            float x = Memory.ReadFloat(Memory.playerPositionX);

            return x;
        }

        public static float PlayerY()
        {
            if (!Memory.isInit)
                Memory.Init();

            float y = Memory.ReadFloat(Memory.playerPositionY);

            return y;
        }

        public static float PlayerZ()
        {
            if (!Memory.isInit)
                Memory.Init();

            float z = Memory.ReadFloat(Memory.playerPositionZ);

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

            return false;
        }

        public static bool IsPlayerConnected(string player)
        {
            if (!Memory.isInit)
                Memory.Init();

            for (int i = 0; i < 1003; i++) // 500 wegen Spielerlimit RGN | Normal: 1003 Evtl. 1003?
            {
                uint remotePlayer = BitConverter.ToUInt32(Memory.ReadMemory((uint)(Memory.structPlayers + Memory.structRemotePlayersOffset + i * 4), 4), 0);
                int nameLength = Memory.ReadInteger(remotePlayer + Memory.remotePlayerStringLengthOffset);

                if (nameLength < 16)
                {
                    string name = Memory.ReadString(remotePlayer + Memory.remotePlayerUsernameOffset, (uint)nameLength);
                    if (player.ToLower() == name.ToLower())
                        return true;
                }
                else
                {
                    uint nameExtension = BitConverter.ToUInt32(Memory.ReadMemory(remotePlayer + Memory.remotePlayerUsernameOffset, 4), 0);
                    string name = Memory.ReadString(nameExtension, (uint)nameLength);
                    if (player.ToLower() == name.ToLower())
                        return true;
                }
            }
            return false;
        }

        public static bool[] IsPlayerConnected(string[] player)
        {
            if (!Memory.isInit)
                Memory.Init();

            bool[] connected = new bool[player.Length];
            for (int i = 0; i < 1003; i++) // 300 wegen nicht viel | 500 wegen Spielerlimit RGN | Normal: 1004 Evtl. 1003?
            {
                uint remotePlayer = BitConverter.ToUInt32(Memory.ReadMemory((uint)(Memory.structPlayers + Memory.structRemotePlayersOffset + i * 4), 4), 0);
                int nameLength = Memory.ReadInteger(remotePlayer + Memory.remotePlayerStringLengthOffset);

                if (nameLength < 16)
                {
                    string name = Memory.ReadString(remotePlayer + Memory.remotePlayerUsernameOffset, (uint)nameLength);
                    for (int j = 0; j < player.Length; j++)
                    {
                        if (player[j].ToLower() == name.ToLower())
                        {
                            connected[j] = true;
                            break;
                        }
                    }
                }
                else
                {
                    uint nameExtension = BitConverter.ToUInt32(Memory.ReadMemory(remotePlayer + Memory.remotePlayerUsernameOffset, 4), 0);
                    string name = Memory.ReadString(nameExtension, (uint)nameLength);

                    for (int j = 0; j < player.Length; j++)
                    {
                        if (player[j].ToLower() == name.ToLower())
                        {
                            connected[j] = true;
                            break;
                        }
                    }
                }
            }
            return connected;
        }
    }
}
