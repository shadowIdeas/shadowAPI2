using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace shadowAPI2
{
    public class RemotePlayer
    {
        // TODO Using the struct more
        // TODO Remake of the class (OOP)
        private static RemotePlayer instance;

        private StructureRemotePlayer[] remotePlayers = new StructureRemotePlayer[1003];

        private RemotePlayer()
        {

        }

        public static RemotePlayer GetInstance()
        {
            if (instance == null)
                instance = new RemotePlayer();

            return instance;
        }

        /// <summary>
        /// Get the id of the player by there name
        /// </summary>
        /// <param name="player">The name of the querried player</param>
        /// <param name="reloadData">Get the current data if it's true</param>
        /// <returns></returns>
        public int GetPlayerIdByName(string player, bool reloadData = true)
        {
            if (!Memory.IsInit)
                Memory.Init();

            int id = -1;
            if (!reloadData)
            {
                for (int i = 0; i < remotePlayers.Length; i++)
                {
                    if (remotePlayers[i].name.ToLower() == player.ToLower())
                    {
                        id = i;
                        break;
                    }
                }
            }
            else
            {
                for (int i = 0; i < 1003; i++)
                {
                    uint remotePlayer = BitConverter.ToUInt32(Memory.ReadMemory((uint)(Memory.structPlayerPool + Memory.structRemotePlayersOffset + i * 4), 4), 0);
                    if(remotePlayer != 0)
                    {
                        int nameLength = Memory.ReadInteger(remotePlayer + Memory.remotePlayerStringLengthOffset);

                        if (nameLength < 16)
                        {
                            string name = Memory.ReadString(remotePlayer + Memory.remotePlayerUsernameOffset, (uint)nameLength);
                            remotePlayers[i].name = name;

                            if (player.ToLower() == name.ToLower())
                            {
                                uint remotePlayerData = Memory.ReadUInteger(remotePlayer + Memory.structRemotePlayersDataOffset);
                                id = i; // (int)Memory.ReadUInteger(remotePlayerData); // Uint16
                            }
                        }
                        else
                        {
                            uint nameExtension = BitConverter.ToUInt32(Memory.ReadMemory(remotePlayer + Memory.remotePlayerUsernameOffset, 4), 0);
                            string name = Memory.ReadString(nameExtension, (uint)nameLength);
                            remotePlayers[i].name = name;

                            if (player.ToLower() == name.ToLower())
                            {
                                uint remotePlayerData = Memory.ReadUInteger(remotePlayer + Memory.structRemotePlayersDataOffset);
                                id = i; //(int)Memory.ReadUInteger(remotePlayerData); // Uint16
                            }
                        }
                    }
                }
            }

            return id;
        }

        /// <summary>
        /// Get the id's of player by there names
        /// </summary>
        /// <param name="player">Array of string with the player names</param>
        /// <param name="reloadData">Get the current data if it's true</param>
        /// <returns></returns>
        public int[] GetPlayerIdByName(string[] player, bool reloadData = true)
        {
            if (!Memory.IsInit)
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
                    uint remotePlayer = BitConverter.ToUInt32(Memory.ReadMemory((uint)(Memory.structPlayerPool + Memory.structRemotePlayersOffset + i * 4), 4), 0);
                    int nameLength = Memory.ReadInteger(remotePlayer + Memory.remotePlayerStringLengthOffset);


                    if (nameLength < 16)
                    {
                        string name = Memory.ReadString(remotePlayer + Memory.remotePlayerUsernameOffset, (uint)nameLength);
                        remotePlayers[i].name = name;

                        for (int j = 0; j < player.Length; j++)
                        {
                            if (player[j].ToLower() == name.ToLower())
                            {
                                uint remotePlayerData = Memory.ReadUInteger(remotePlayer + Memory.structRemotePlayersDataOffset);
                                id[j] = i; //(int)Memory.ReadUInteger(remotePlayerData); // Uint16
                                remotePlayers[i].id = (uint)i;
                                break;
                            }
                        }
                    }
                    else
                    {
                        uint nameExtension = BitConverter.ToUInt32(Memory.ReadMemory(remotePlayer + Memory.remotePlayerUsernameOffset, 4), 0);
                        string name = Memory.ReadString(nameExtension, (uint)nameLength);
                        remotePlayers[i].name = name;

                        for (int j = 0; j < player.Length; j++)
                        {
                            if (player[j].ToLower() == name.ToLower())
                            {
                                uint remotePlayerData = Memory.ReadUInteger(remotePlayer + Memory.structRemotePlayersDataOffset);
                                id[j] = i; //(int)Memory.ReadUInteger(remotePlayerData); // Uint16
                                remotePlayers[i].id = (uint)i;
                                break;
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
        public string GetPlayerNameById(uint id, bool reloadId = true)
        {
            if (!Memory.IsInit)
                Memory.Init();

            string name = "";

            if (id <= 1003)
            {
                if (!reloadId)
                {
                    for (int i = 0; i < remotePlayers.Length; i++)
                    {
                        if (i == id)
                        {
                            name = remotePlayers[i].name;
                            break;
                        }
                    }
                }
                else
                {
                    uint remotePlayer = BitConverter.ToUInt32(Memory.ReadMemory((uint)(Memory.structPlayerPool + Memory.structRemotePlayersOffset + id * 4), 4), 0);
                    int nameLength = Memory.ReadInteger(remotePlayer + Memory.remotePlayerStringLengthOffset);

                    if (nameLength < 16)
                    {
                        name = Memory.ReadString(remotePlayer + Memory.remotePlayerUsernameOffset, (uint)nameLength);
                        remotePlayers[id].name = name;
                        remotePlayers[id].id = id;
                    }
                    else
                    {
                        uint nameExtension = BitConverter.ToUInt32(Memory.ReadMemory(remotePlayer + Memory.remotePlayerUsernameOffset, 4), 0);
                        name = Memory.ReadString(nameExtension, (uint)nameLength);
                        remotePlayers[id].name = name;
                        remotePlayers[id].id = id;
                    }
                }
            }

            return name;
        }

        /// <summary>
        /// Get player names by there id's
        /// </summary>
        /// <param name="id">Array of uint, with the id's of the players</param>
        /// <param name="reloadId">Reload all id's if it's true</param>
        /// <returns></returns>
        public string[] GetPlayerNameById(uint[] id, bool reloadId = true)
        {
            if (!Memory.IsInit)
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
                    uint remotePlayer = BitConverter.ToUInt32(Memory.ReadMemory((uint)(Memory.structPlayerPool + Memory.structRemotePlayersOffset + id[i] * 4), 4), 0);
                    int nameLength = Memory.ReadInteger(remotePlayer + Memory.remotePlayerStringLengthOffset);

                    if (nameLength < 16)
                    {
                        name[i] = Memory.ReadString(remotePlayer + Memory.remotePlayerUsernameOffset, (uint)nameLength);
                        remotePlayers[id[i]].name = name[i];
                    }
                    else
                    {
                        uint nameExtension = BitConverter.ToUInt32(Memory.ReadMemory(remotePlayer + Memory.remotePlayerUsernameOffset, 4), 0);
                        name[i] = Memory.ReadString(nameExtension, (uint)nameLength);
                        remotePlayers[id[i]].name = name[i];
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
        public int GetPlayerScoreById(uint id, bool reloadId = true)
        {
            if (!Memory.IsInit)
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

            }

            return score;
        }

        /// <summary>
        /// Check if the player is connected
        /// </summary>
        /// <param name="player">A string with the player name</param>
        /// <param name="reloadData">Get the current data if it's true</param>
        /// <returns></returns>
        public bool IsPlayerConnected(string player, bool reloadData = true)
        {
            if (!Memory.IsInit)
                Memory.Init();

            if (!reloadData)
            {
                for (int i = 0; i < remotePlayers.Length; i++)
                {
                    if (remotePlayers[i].name.ToLower() == player.ToLower())
                        return true;
                }
            }
            else
            {
                for (int i = 0; i < 1003; i++)
                {
                    uint remotePlayer = BitConverter.ToUInt32(Memory.ReadMemory((uint)(Memory.structPlayerPool + Memory.structRemotePlayersOffset + i * 4), 4), 0);
                    int nameLength = Memory.ReadInteger(remotePlayer + Memory.remotePlayerStringLengthOffset);

                    if (nameLength < 16)
                    {
                        string name = Memory.ReadString(remotePlayer + Memory.remotePlayerUsernameOffset, (uint)nameLength);
                        remotePlayers[i].name = name;

                        if (player.ToLower() == name.ToLower())
                            return true;
                    }
                    else
                    {
                        uint nameExtension = BitConverter.ToUInt32(Memory.ReadMemory(remotePlayer + Memory.remotePlayerUsernameOffset, 4), 0);
                        string name = Memory.ReadString(nameExtension, (uint)nameLength);
                        remotePlayers[i].name = name;

                        if (player.ToLower() == name.ToLower())
                            return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Check if the players are connected
        /// </summary>
        /// <param name="player">A Array of string with the player names</param>
        /// <param name="reloadData">Get the current data if it's true</param>
        /// <returns></returns>
        public bool[] IsPlayerConnected(string[] player, bool reloadData = true)
        {
            if (!Memory.IsInit)
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
                    uint remotePlayer = BitConverter.ToUInt32(Memory.ReadMemory((uint)(Memory.structPlayerPool + Memory.structRemotePlayersOffset + i * 4), 4), 0);
                    int nameLength = Memory.ReadInteger(remotePlayer + Memory.remotePlayerStringLengthOffset);

                    if (nameLength < 16)
                    {
                        string name = Memory.ReadString(remotePlayer + Memory.remotePlayerUsernameOffset, (uint)nameLength);
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
                        uint nameExtension = BitConverter.ToUInt32(Memory.ReadMemory(remotePlayer + Memory.remotePlayerUsernameOffset, 4), 0);
                        string name = Memory.ReadString(nameExtension, (uint)nameLength);
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

            return connected;
        }
    }
}