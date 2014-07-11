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

            return id;
        }

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