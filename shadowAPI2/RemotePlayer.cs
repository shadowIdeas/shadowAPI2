using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace shadowAPI2
{
    public class RemotePlayer
    {
        private static StructureRemotePlayer[] remotePlayers = new StructureRemotePlayer[1003];

        private static IntPtr structSAMP;
        private static IntPtr structSAMPPools;
        private static IntPtr poolPlayers;
        private static IntPtr updateScoreboardData;
        private static IntPtr tick;


        internal static void GenerateAddresses(IntPtr sampBase)
        {
            Memory.ReadMemory<IntPtr>(IntPtr.Add(sampBase, 0x21A0F8), out structSAMP);
            Memory.ReadMemory<IntPtr>(IntPtr.Add(structSAMP, 0x3CD), out structSAMPPools);
            Memory.ReadMemory<IntPtr>(IntPtr.Add(structSAMPPools, 0x18), out poolPlayers);
            updateScoreboardData = IntPtr.Add(sampBase, 0x8A10);
            Memory.ReadMemory<IntPtr>(IntPtr.Add(sampBase, 0x104978), out tick);
        }

        /// <summary>
        /// Get the id of the player by there name
        /// </summary>
        /// <param name="player">The name of the querried player</param>
        /// <param name="reloadData">Get the current data if it's true</param>
        /// <returns></returns>
        public static int GetPlayerIdByName(string player, bool reloadData = true)
        {
            return GetPlayerIdByName(new string[] { player }, reloadData)[0];
        }

        /// <summary>
        /// Get the id's of player by there names
        /// </summary>
        /// <param name="player">Array of string with the player names to be querried</param>
        /// <param name="reloadData">Get the current data if it's true</param>
        /// <returns></returns>
        public static int[] GetPlayerIdByName(string[] player, bool reloadData = true)
        {
            Memory.Init();

            int[] id = Enumerable.Repeat(-1, player.Length).ToArray();

            if (!reloadData)
            {
                for (int i = 0; i < 1003; i++)
                {
                    for (int j = 0; j < player.Length; j++)
                    {
                        if (remotePlayers[i].name.ToLower() == player[j].ToLower())
                        {
                            id[j] = i;
                            break;
                        }
                    }
                }
            }
            else
            {
                for (int i = 0; i < 1003; i++)
                {
                    IntPtr remotePlayer = IntPtr.Zero;
                    Memory.ReadMemory<IntPtr>(IntPtr.Add(IntPtr.Add(poolPlayers, 0x2E), i * 4), out remotePlayer);

                    if (remotePlayer != IntPtr.Zero)
                    {
                        int nameLength = 0;
                        Memory.ReadMemory<int>(IntPtr.Add(remotePlayer, 0x20), out nameLength);

                        if (nameLength < 16)
                        {
                            string name = Memory.ReadString(IntPtr.Add(remotePlayer, 0xC), nameLength);
                            remotePlayers[i].name = name;

                            for (int j = 0; j < player.Length; j++)
                            {
                                if (player[j].ToLower() == name.ToLower())
                                {
                                    //uint remotePlayerData = Memory.ReadMemory(remotePlayer + Memory.OFFSET_REMOTE_PLAYER_DATA);
                                    id[j] = i; //(int)Memory.ReadUInteger(remotePlayerData); // Uint16
                                    remotePlayers[i].id = i;
                                    break;
                                }
                            }
                        }
                        else
                        {
                            IntPtr nameExtension = IntPtr.Zero;
                            Memory.ReadMemory<IntPtr>(IntPtr.Add(remotePlayer, 0xC), out nameExtension);

                            string name = Memory.ReadString(nameExtension, nameLength);
                            remotePlayers[i].name = name;

                            for (int j = 0; j < player.Length; j++)
                            {
                                if (player[j].ToLower() == name.ToLower())
                                {
                                    //uint remotePlayerData = Memory.ReadUInteger(remotePlayer + Memory.OFFSET_REMOTE_PLAYER_DATA);
                                    id[j] = i; //(int)Memory.ReadUInteger(remotePlayerData); // Uint16
                                    remotePlayers[i].id = i;
                                    break;
                                }
                            }

                        }
                    }
                }
            }

            return id;
        }

        /// <summary>
        /// Get the player name by there id
        /// </summary>
        /// <param name="id">Id to querried</param>
        /// <param name="reloadId">Reload the querried id if it's true</param>
        /// <returns></returns>
        public static string GetPlayerNameById(uint id, bool reloadId = true)
        {
            return GetPlayerNameById(new uint[] { id }, reloadId)[0];
        }

        /// <summary>
        /// Get player names by there id's
        /// </summary>
        /// <param name="id">Array of uint, with the id's of the players</param>
        /// <param name="reloadId">Reload all id's if it's true</param>
        /// <returns></returns>
        public static string[] GetPlayerNameById(uint[] id, bool reloadId = true)
        {
            Memory.Init();

            string[] name = new string[id.Length];

            if (!reloadId)
            {
                for (int i = 0; i < remotePlayers.Length; i++)
                {
                    for (int j = 0; j < id.Length; j++)
                    {
                        if (i == id[j])
                        {
                            name[j] = remotePlayers[i].name;
                            break;
                        }
                    }
                }
            }
            else
            {
                for (int i = 0; i < id.Length; i++)
                {
                    IntPtr remotePlayer = IntPtr.Zero;
                    Memory.ReadMemory<IntPtr>(IntPtr.Add(IntPtr.Add(poolPlayers, 0x2E), (int)id[i] * 4), out remotePlayer);

                    if (remotePlayer != IntPtr.Zero)
                    {
                        int nameLength = 0;
                        Memory.ReadMemory<int>(IntPtr.Add(remotePlayer, 0x20), out nameLength);

                        if (nameLength < 16)
                        {
                            name[i] = Memory.ReadString(IntPtr.Add(remotePlayer, 0xC), nameLength);
                            remotePlayers[id[i]].name = name[i];
                        }
                        else
                        {
                            IntPtr nameExtension = IntPtr.Zero;
                            Memory.ReadMemory<IntPtr>(IntPtr.Add(remotePlayer, 0xC), out nameExtension);
                            name[i] = Memory.ReadString(nameExtension, nameLength);
                            remotePlayers[id[i]].name = name[i];
                        }
                    }
                }
            }

            return name;
        }

        /// <summary>
        /// Get the player score by the player id
        /// </summary>
        /// <param name="id">Id to be querried</param>
        /// <param name="reloadId">Reload the player with the 'id' parameter if it's true</param>
        /// <returns></returns>
        public static int GetPlayerScoreById(uint id, bool reloadId = true)
        {
            Memory.Init();

            int score = -1;

            if (!reloadId)
            {
                for (int i = 0; i < remotePlayers.Length; i++)
                {
                    if (i == id)
                    {
                        score = remotePlayers[i].score;
                        break;
                    }
                }
            }
            else
            {
                IntPtr remotePlayer = IntPtr.Zero;
                Memory.ReadMemory<IntPtr>(IntPtr.Add(IntPtr.Add(poolPlayers, 0x2E), (int)id * 4), out remotePlayer);

                /* UNDONE
                List<byte> data = new List<byte>();

                data.Add(0xB9);
                data.AddRange(BitConverter.GetBytes((uint)Memory.structSamp));

                data.Add(0xE8);
                int offset = (int)address - ((int)Memory.ParameterMemory[Memory.ParameterMemory.Length - 1] + 10);

                data.Add(0xC3);
                */

                if (remotePlayer != IntPtr.Zero)
                {
                    Memory.ReadMemory<int>(IntPtr.Add(remotePlayer, 0x24), out score);
                    remotePlayers[id].score = score;
                }
            }

            return score;
        }

        public static int GetPlayerPingById(uint id, bool reloadId = true)
        {
            Memory.Init();

            int score = -1;

            if (!reloadId && id >= 0)
            {
                for (int i = 0; i < remotePlayers.Length; i++)
                {
                    if (i == id)
                    {
                        score = remotePlayers[i].score;
                        break;
                    }
                }
            }
            else
            {
                IntPtr remotePlayer = IntPtr.Zero;
                Memory.ReadMemory<IntPtr>(IntPtr.Add(IntPtr.Add(poolPlayers, 0x2E), (int)id * 4), out remotePlayer);

                /* UNDONE
                List<byte> data = new List<byte>();

                data.Add(0xB9);
                data.AddRange(BitConverter.GetBytes((uint)Memory.structSamp));

                data.Add(0xE8);
                int offset = (int)address - ((int)Memory.ParameterMemory[Memory.ParameterMemory.Length - 1] + 10);

                data.Add(0xC3);
                */

                if (remotePlayer != IntPtr.Zero)
                {
                    Memory.ReadMemory<int>(IntPtr.Add(remotePlayer, 0x28), out score);
                    remotePlayers[id].score = score;
                }
            }

            return score;
        }



        /// <summary>
        /// Check if the player is connected
        /// </summary>
        /// <param name="player">A string with the player name</param>
        /// <param name="reloadData">Get the current data if it's true</param>
        /// <returns></returns>
        public static bool IsPlayerConnected(string player, bool reloadData = true)
        {
            return IsPlayerConnected(new string[] { player }, reloadData)[0];
        }

        /// <summary>
        /// Check if the players are connected
        /// </summary>
        /// <param name="player">A Array of string with the player names</param>
        /// <param name="reloadData">Get the current data if it's true</param>
        /// <returns></returns>
        public static bool[] IsPlayerConnected(string[] player, bool reloadData = true)
        {
            Memory.Init();

            bool[] connected = new bool[player.Length];

            if (!reloadData)
            {
                for (int i = 0; i < 1003; i++)
                {
                    for (int j = 0; j < player.Length; j++)
                    {
                        if (remotePlayers[i].name.ToLower() == player[j].ToLower())
                        {
                            connected[j] = true;
                            break;
                        }
                    }
                }
            }
            else
            {
                for (int i = 0; i < 1003; i++)
                {
                    IntPtr remotePlayer = IntPtr.Zero;
                    Memory.ReadMemory<IntPtr>(IntPtr.Add(IntPtr.Add(poolPlayers, 0x2E), (int)i * 4), out remotePlayer);

                    if (remotePlayer != IntPtr.Zero)
                    {
                        int nameLength = 0;
                        Memory.ReadMemory<int>(IntPtr.Add(remotePlayer, 0x20), out nameLength);

                        if (nameLength < 16)
                        {
                            string name = Memory.ReadString(IntPtr.Add(remotePlayer, 0xC), nameLength);
                            remotePlayers[i].name = name;

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
                            IntPtr nameExtension = IntPtr.Zero;
                            Memory.ReadMemory<IntPtr>(IntPtr.Add(remotePlayer, 0xC), out nameExtension);

                            string name = Memory.ReadString(nameExtension, nameLength);
                            remotePlayers[i].name = name;

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
                }
            }

            return connected;
        }

        public static void UpdatePlayerData()
        {
            Memory.Init();

            Memory.WriteMemory((uint)tick, new byte[] { 0x0, 0x0, 0x0, 0x0 }, (uint)4);

            List<byte> ptr = new List<byte>();
            ptr.Add(0xB9);
            ptr.AddRange(BitConverter.GetBytes(structSAMP.ToInt32()));
            Memory.Call(updateScoreboardData, ptr.ToArray(), false);
        }
    }
}