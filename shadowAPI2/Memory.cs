using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace shadowAPI2
{
    internal class Memory
    {
        #region DLLImport
        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool ReadProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, byte[] lpBuffer, UInt32 nSize, ref UInt32 lpNumberOfBytesRead);
        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool WriteProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, byte[] lpBuffer, UInt32 nSize, ref UInt32 lpNumberOfBytesWritten);
        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern IntPtr OpenProcess(UInt32 dwDesiredAccess, Int32 bInheritHandle, UInt32 dwProcessId);
        [DllImport("kernel32.dll")]
        private static extern Int32 CloseHandle(IntPtr hObject);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern IntPtr VirtualAllocEx(IntPtr hProcess, IntPtr lpAddress, uint dwSize, uint flAllocationType, uint flProtect);
        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern IntPtr CreateRemoteThread(IntPtr hProcess, IntPtr lpThreadAttributes, uint dwStackSize, uint lpStartAddress, IntPtr lpParameter, uint dwCreationFlags, IntPtr lpThreadId);
        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern UInt32 WaitForSingleObject(IntPtr hHandle, UInt32 dwMilliseconds);
        #endregion

        private const int RESERVE = 25;

        // GTA and more
        private static uint pid = 0;
        private static Process gtaProcess;
        public static IntPtr handle = IntPtr.Zero;
        private static uint sampModule = 0;
        private static uint gtaModule = 0;

        public static string _processName = "rgn_ac_gta";
        private static bool isInit = false;

        private static IntPtr allocMemory = IntPtr.Zero;
        private static IntPtr[] parameterMemory = new IntPtr[RESERVE];

        // Read addresses
        #region SAMP specific and used for more classes
        private static uint structSampOffset = 0x212A80;
        private static uint structPlayersPoolOffset = 0x3D9;
        private static uint structPlayersOffset = 0x14;
        #endregion
        #region Need for Player class
        // Player
        private static uint playerOffsetBase = 0xB6F5F0;

        private static uint playerBase = 0;

        public static uint playerMoney = 0x0B7CE54;

        public static uint playerWeapon = 0x0BAA410;

        // Health and armor
        private static uint playerOffsetHealth = 0x540;
        private static uint playerOffsetArmor = 0x548;

        public static uint playerHealth = 0;
        public static uint playerArmor = 0;

        // Position
        private static uint playerOffsetMatrix = 0x14;
        private static uint playerOffsetPositionRotation = 0x14;
        private static uint playerOffsetPositionX = 0x30;
        private static uint playerOffsetPositionY = 0x34;
        private static uint playerOffsetPositionZ = 0x38;

        private static uint playerPosition = 0;
        public static uint playerPositionRotation = 0;
        public static uint playerPositionX = 0;
        public static uint playerPositionY = 0;
        public static uint playerPositionZ = 0;

        // Location
        private static uint playerOffsetLocation = 0x2F;
        public static uint playerLocation = 0;

        // SAMP informations
        private static uint playerOffsetName = 0x2123F7;
        public static uint playerName = 0;

        private static uint playerOffsetId = 0x04;
        public static uint playerId = 0;

        //public static uint playerNameLength = 
        #endregion
        #region Need for Vehicle class
        // Car
        public static uint vehicleOffsetBase = 0xBA18FC;

        public static uint vehicleOffsetModelId = 0x22;

        public static uint vehicleOffsetDamage = 0x4C0;

        public static uint vehicleOffsetSpeedX = 0x44;
        public static uint vehicleOffsetSpeedY = 0x48;
        public static uint vehicleOffsetSpeedZ = 0x4C;

        public static uint vehicleOffsetCollideStatus = 0xD8;

        public static uint vehicleOffsetLockState = 0x4F8;

        public static uint vehicleOffsetSeats = 0x460; // Address to the pointer of the driver(+0x04 for other passenger pointer)

        public static uint vehicleOffsetEngineState = 0x428;

        private static uint structOffsetLocalPlayer = 0x22;
        private static uint vehicleOffsetId = 0x8;

        public static uint vehicleId = 0;

        #endregion
        #region Need for Statistic class
        // Stats
        public static uint statisticFeetMeters = 0xB7938C;
        public static uint statisticVehicleMeters = 0xB79390;
        public static uint statisticBikeMeters = 0xB79394;
        public static uint statisticHelicopterMeters = 0xB793A0;
        public static uint statisticShipMeters = 0xB79398;
        public static uint statisticSwimMeters = 0xB793E8;
        #endregion
        #region Need for Chat class
        // Chat
        private static uint chatMessageOffset = 0x212A6C;
        private static uint chatOffset = 0x212A94;
        private static uint isChatOpenOffset = 0x55;

        public static uint chatMessage = 0;
        private static uint chat = 0;
        public static uint isChatOpen = 0;

        // Dialog
        private static uint dialogOffset = 0x212A40;
        private static uint isDialogOpenOffset = 0x28;

        private static uint dialog = 0;
        public static uint isDialogOpen = 0;
        #endregion
        #region Need for RemotePlayer class
        public static uint structRemotePlayersOffset = 0x2E;
        public static uint structRemotePlayersDataOffset = 0x08;
        public static uint remotePlayerScoreOffset = 0x04;
        public static uint remotePlayerStringLengthOffset = 0x24;
        public static uint remotePlayerUsernameOffset = 0x14;

        public static uint structSamp = 0;
        private static uint structSampPools = 0;
        public static uint structPlayerPool = 0;
        #endregion

        // Function addresses
        #region Need for Chat class
        // Chat
        private static uint functionSendSayOffset = 0x4CA0;
        private static uint functionSendCommandOffset = 0x7BDD0;
        private static uint functionAddChatMessageOffset = 0x7AA00;

        public static uint functionSendSay = 0;
        public static uint functionSendCommand = 0;
        public static uint functionAddChatMessage = 0;
        #endregion

        internal static void Init(string processName = "rgn_ac_gta")
        {
            _processName = processName;

            Process[] processes = Process.GetProcessesByName(_processName);
            if(processes.Length > 0 && !isInit)
            {
                gtaProcess = processes[0];
                gtaProcess.EnableRaisingEvents = true;
                gtaProcess.Exited += OnGtaExited;

                pid = (uint)processes[0].Id;
                ProcessModuleCollection modules = processes[0].Modules;
                foreach (ProcessModule item in modules)
                {
                    if (item.ModuleName == "samp.dll")
                    {
                        sampModule = (uint)item.BaseAddress;
                    }
                    else if (item.ModuleName == _processName + ".exe")
                    {
                        gtaModule = (uint)item.BaseAddress;
                    }
                }

                handle = OpenProcess(0x1F0FFF, 1, pid);

                //// Allocate
                allocMemory = VirtualAllocEx(handle, IntPtr.Zero, RESERVE * 1024, 0x1000 | 0x2000, 0x40); // TODO Beim refresehen memory frei lassen
                int x = Marshal.GetLastWin32Error();

                for (int i = 0; i < parameterMemory.Length; i++)
                {
                    parameterMemory[i] = allocMemory + (1024 * i);
                }

                InitVariables();

                isInit = true;
            }
        }

        private static void InitVariables()
        {
            // Variables
            #region SAMP specified
            structSamp = BitConverter.ToUInt32(ReadMemory(sampModule + structSampOffset, 4), 0);
            structSampPools = BitConverter.ToUInt32(ReadMemory(structSamp + structPlayersPoolOffset, 4), 0);
            structPlayerPool = BitConverter.ToUInt32(ReadMemory(structSampPools + structPlayersOffset, 4), 0);
            #endregion
            #region Player
            // Base of Player
            playerBase = BitConverter.ToUInt32(ReadMemory(playerOffsetBase, 4), 0);

            // HP
            playerHealth = playerBase + playerOffsetHealth;
            playerArmor = playerBase + playerOffsetArmor;

            // Position
            playerPosition = BitConverter.ToUInt32(ReadMemory(playerBase + playerOffsetMatrix, 4), 0);
            playerPositionRotation = playerPosition + playerOffsetPositionRotation;
            playerPositionX = playerPosition + playerOffsetPositionX;
            playerPositionY = playerPosition + playerOffsetPositionY;
            playerPositionZ = playerPosition + playerOffsetPositionZ;

            // Interior Boolean
            playerLocation = playerBase + playerOffsetLocation;

            // SAMP informations
            playerName = sampModule + playerOffsetName;
            playerId = structPlayerPool + playerOffsetId;
            #endregion
            #region Chat
            // Chat
            while ((chatMessage = BitConverter.ToUInt32(ReadMemory((uint)sampModule + chatMessageOffset, 4), 0)) == 0)
                System.Threading.Thread.Sleep(100);
            while ((chat = BitConverter.ToUInt32(ReadMemory((uint)sampModule + chatOffset, 4), 0)) == 0)
                System.Threading.Thread.Sleep(100);
            isChatOpen = chat + isChatOpenOffset;
            #endregion
            #region Dialog
            // Dialog
            dialog = BitConverter.ToUInt32(ReadMemory((uint)sampModule + dialogOffset, 4), 0);
            isDialogOpen = dialog + isDialogOpenOffset;
            #endregion
            #region Vehicle
            vehicleId = ReadUInteger(structPlayerPool + structOffsetLocalPlayer) + vehicleOffsetId;
            #endregion
            #region Player Infos

            #endregion
            #region World
            #endregion

            // Functions
            #region Chat functions
            functionSendSay = sampModule + functionSendSayOffset;
            functionSendCommand = sampModule + functionSendCommandOffset;
            functionAddChatMessage = sampModule + functionAddChatMessageOffset;
            #endregion
        }

        internal static void UnInit()
        {
            if(!isInit && handle != IntPtr.Zero)
            {
                Process[] processes = Process.GetProcessesByName("rgn_ac_gta");
                if(processes.Length > 0 && processes[0].Id == pid)
                {
                    CloseHandle(handle);
                }

                isInit = false;
                pid = 0;
                handle = IntPtr.Zero;
                sampModule = 0;
                gtaModule = 0;
            }
        }

        internal static bool ReadBoolean(uint address)
        {
            byte[] bytes = ReadMemory(address, 1);

            bool result = BitConverter.ToBoolean(bytes, 0);

            return result;
        }

        internal static string ReadString(uint address, uint length)
        {
            byte[] bytes = ReadMemory(address, length);

            string result = Encoding.ASCII.GetString(bytes);

            return result;
        }

        internal static int ReadInteger(uint address)
        {
            byte[] bytes = ReadMemory(address, 4);

            int result = BitConverter.ToInt32(bytes, 0);

            return result;
        }

        internal static Int16 ReadInteger16(uint address)
        {
            byte[] bytes = ReadMemory(address, 2);

            Int16 result = BitConverter.ToInt16(bytes, 0);

            return result;
        }

        internal static uint ReadUInteger(uint address)
        {
            byte[] bytes = ReadMemory(address, 4);

            uint result = BitConverter.ToUInt32(bytes, 0);

            return result;
        }

        internal static float ReadFloat(uint address)
        {
            byte[] bytes = ReadMemory(address, 4);

            float result = BitConverter.ToSingle(bytes, 0);

            return result;
        }

        internal static byte ReadByte(uint address)
        {
            byte[] bytes = ReadMemory(address, 1);

            byte result = bytes[0];

            return result;
        }

        internal static byte[] ReadMemory(uint address, uint size)
        {
            byte[] bytes = new byte[size];
            uint bytesReaded = 0;

            ReadProcessMemory(handle, (IntPtr)address, bytes, size, ref bytesReaded);

            return bytes;
        }

        internal static void WriteBoolean(uint address, bool boolean)
        {
            if (boolean)
                WriteMemory(address, new byte[] { 0 }, 1);
            else
                WriteMemory(address, new byte[] { 1 }, 1);
        }

        internal static void WriteByte(uint address, byte value)
        {
            WriteMemory(address, new byte[] { value }, 1);
        }

        internal static bool WriteMemory(uint address, byte[] bytes, uint size)
        {
            uint bytesWritten = 0;
            bool result = false;

            if (WriteProcessMemory(handle, (IntPtr)address, bytes, size, ref bytesWritten))
                result = true;

            return result;
        }

        internal static bool WriteString(uint address, string text)
        {
            byte[] bytes = Encoding.Default.GetBytes(text);

            return WriteMemory(address, bytes, (uint)512);
        }

        internal static bool WriteFloat(uint address, float dec)
        {
            byte[] bytes = BitConverter.GetBytes(dec);

            return WriteMemory(address, bytes, (uint)512);
        }

        internal static void Call(uint address, object[] parameter, bool stackClear)
        {
            List<byte> data = new List<byte>();

            int usedParameters = 0;
            for (int i = parameter.Length - 1; i >= 0; i--)
            {
                IntPtr memoryAddress = IntPtr.Zero;
                Type type = parameter[i].GetType();
                if (type == typeof(string) && usedParameters <= parameterMemory.Length - 1)
                {
                    memoryAddress = parameterMemory[usedParameters];
                    if (!WriteString((uint)memoryAddress, (string)parameter[i]))
                        return;
                    usedParameters++;
                }
                else if (type == typeof(uint))
                {
                    memoryAddress = new IntPtr(Convert.ToUInt32(parameter[i]));
                }
                else if (type == typeof(int))
                {
                    memoryAddress = new IntPtr(Convert.ToInt32(parameter[i]));
                }
                else if (type == typeof(Single) && usedParameters <= parameterMemory.Length - 1)
                {
                    memoryAddress = parameterMemory[usedParameters];
                    if (!WriteFloat((uint)memoryAddress, (float)parameter[i]))
                        return;
                    usedParameters++;
                }
                else
                    return;

                data.Add(0x68);
                data.AddRange(BitConverter.GetBytes((uint)memoryAddress));
            }

            data.Add(0xE8);
            int offset = (int)address - ((int)parameterMemory[parameterMemory.Length - 1] + (parameter.Length * 5 + 5));
            data.AddRange(BitConverter.GetBytes(offset));

            if (stackClear)
            {
                data.AddRange(new byte[] { 0x83, 0xC4 });
                data.Add(Convert.ToByte(parameter.Length * 4));
            }
            data.Add(0xC3);

            if (!WriteMemory((uint)parameterMemory[parameterMemory.Length - 1], data.ToArray(), (uint)data.Count))
                return;

            IntPtr thread = CreateRemoteThread(handle, IntPtr.Zero, 0, (uint)parameterMemory[parameterMemory.Length - 1], IntPtr.Zero, 0, IntPtr.Zero);
            WaitForSingleObject(thread, 0xFFFFFFFF);
        }


        private static void OnGtaExited(object sender, EventArgs e)
        {
            isInit = false;
        }


        internal static bool IsInit
        {
            get { return isInit; }
        }

        internal static IntPtr[] ParameterMemory
        {
            get { return parameterMemory; }
        }
    }
}