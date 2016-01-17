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
        private static extern IntPtr OpenProcess(uint dwDesiredAccess, int bInheritHandle, uint dwProcessId);
        [DllImport("kernel32.dll")]
        private static extern Int32 CloseHandle(IntPtr hObject);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern IntPtr VirtualAllocEx(IntPtr hProcess, IntPtr lpAddress, uint dwSize, uint flAllocationType, uint flProtect);
        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern IntPtr CreateRemoteThread(IntPtr hProcess, IntPtr lpThreadAttributes, uint dwStackSize, uint lpStartAddress, IntPtr lpParameter, uint dwCreationFlags, IntPtr lpThreadId);
        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern UInt32 WaitForSingleObject(IntPtr hHandle, UInt32 dwMilliseconds);
        #endregion

        private const short ALLOCATE_TIMES = 32;
        private const uint ALLOCATE_SIZE = 1024;

        private static Process _process = null;
        private static uint pid = 0;
        private static IntPtr handle = IntPtr.Zero;

        private static IntPtr sampBase = IntPtr.Zero;
        private static int sampLength = 0;

        private static IntPtr[] memory = new IntPtr[ALLOCATE_TIMES];

        public static bool Init()
        {
            if (_process != null && !_process.HasExited)
                return false;

            Process process = Process.GetProcessesByName("rgn_ac_gta").FirstOrDefault();
            if (process != null && process != default(Process))
            {
                _process = process;
                _process.EnableRaisingEvents = true;
                _process.Exited += _process_Exited;

                foreach (ProcessModule item in process.Modules)
                {
                    if (item.ModuleName == "samp.dll")
                    {
                        sampBase = item.BaseAddress;
                        sampLength = item.ModuleMemorySize;
                        break;
                    }
                }

                if (sampBase != IntPtr.Zero)
                {
                    pid = (uint)process.Id;
                    handle = OpenProcess(0x1F0FFF, 1, pid);
                    if (handle != IntPtr.Zero)
                    {
                        // allocate about 32kb memory

                        IntPtr space = VirtualAllocEx(handle, IntPtr.Zero, (uint)(ALLOCATE_SIZE * ALLOCATE_TIMES), 0x1000 | 0x2000, 0x40);
                        for (int i = 0; i < ALLOCATE_TIMES; i++)
                        {
                            if (space != IntPtr.Zero)
                                memory[i] = IntPtr.Add(space, (int)(ALLOCATE_SIZE * i));
                            else
                                return false;
                        }
                        ZoneManager.Load();
                        GenerateAddresses();

                        return true;
                    }
                }
            }
            return false;

        }

        public static bool ReadMemory<T>(IntPtr address, out T obj)
        {
            obj = default(T);
            var size = 1;
            if (typeof(T) != typeof(byte))
                size = Marshal.SizeOf(typeof(T));
            var buffer = new byte[size];
            uint readed = 0;

            if (ReadProcessMemory(handle, address, buffer, (uint)size, ref readed))
            {
                if (size == readed)
                {
                    if (obj.GetType() == typeof(Int16))
                        obj = (T)Convert.ChangeType(BitConverter.ToInt16(buffer, 0), typeof(T));
                    else if (obj.GetType() == typeof(Int32))
                        obj = (T)Convert.ChangeType(BitConverter.ToInt32(buffer, 0), typeof(T));
                    else if (obj.GetType() == typeof(float))
                        obj = (T)Convert.ChangeType(BitConverter.ToSingle(buffer, 0), typeof(T));
                    else if (obj.GetType() == typeof(IntPtr))
                        obj = (T)Convert.ChangeType(new IntPtr(BitConverter.ToInt32(buffer, 0)), typeof(T));
                    else if (obj.GetType() == typeof(byte))
                        obj = (T)Convert.ChangeType(buffer[0], typeof(T));

                    return true;
                }
            }

            return false;
        }
        public static bool ReadMemory<T>(int address, out T obj)
        {
            obj = default(T);
            var size = 1;
            if (typeof(T) != typeof(byte))
                size = Marshal.SizeOf(typeof(T));
            var buffer = new byte[size];
            uint readed = 0;

            if (ReadProcessMemory(handle, new IntPtr(address), buffer, (uint)size, ref readed))
            {
                if (size == readed)
                {
                    if (obj.GetType() == typeof(Int16))
                        obj = (T)Convert.ChangeType(BitConverter.ToInt16(buffer, 0), typeof(T));
                    else if (obj.GetType() == typeof(Int32))
                        obj = (T)Convert.ChangeType(BitConverter.ToInt32(buffer, 0), typeof(T));
                    else if (obj.GetType() == typeof(float))
                        obj = (T)Convert.ChangeType(BitConverter.ToSingle(buffer, 0), typeof(T));
                    else if (obj.GetType() == typeof(IntPtr))
                        obj = (T)Convert.ChangeType(new IntPtr(BitConverter.ToInt32(buffer, 0)), typeof(T));
                    else if (obj.GetType() == typeof(byte))
                        obj = (T)Convert.ChangeType(buffer[0], typeof(T));
                    return true;
                }
            }

            return false;
        }

        public static string ReadString(IntPtr address, int size)
        {
            byte[] buffer;
            if (ReadMemoryBytes(address, size, out buffer))
            {
                return Encoding.UTF8.GetString(buffer).Split(new char[] {'\0'})[0];
            }

            return "";
        }

        public static bool ReadMemoryBytes(IntPtr address, int size, out byte[] bytes)
        {
            bytes = new byte[size];
            uint readed = 0;

            if (ReadProcessMemory(handle, address, bytes, (uint)size, ref readed))
            {
                if (size == readed)
                    return true;
            }

            return false;
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

        internal static bool WriteString(IntPtr address, string text)
        {
            byte[] bytes = Encoding.Default.GetBytes(text);

            return WriteMemory((uint)address, bytes, (uint)512);
        }

        public static bool WriteMemoryBytes(IntPtr address, byte[] bytes)
        {
            uint written = 0;
            if (WriteProcessMemory(handle, address, bytes, (uint)bytes.Length, ref written))
            {
                if (written == bytes.Length)
                    return true;
            }
            return false;
        }

        internal static void Call(uint address, bool stackClear, params object[] parameter)
        {
            List<byte> data = new List<byte>();

            int usedParameters = 0;

            for (int i = parameter.Length - 1; i >= 0; i--)
            {
                IntPtr memoryAddress = IntPtr.Zero;
                Type type = parameter[i].GetType();
                if (type == typeof(string) && usedParameters <= memory.Length - 1)
                {
                    memoryAddress = memory[usedParameters];
                    if (!WriteString(memoryAddress, (string)parameter[i]))
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
                else if (type == typeof(byte))
                {
                    memoryAddress = new IntPtr(Convert.ToInt32(parameter[i]));
                }
                else if (type == typeof(Single) || type == typeof(Double) || type == typeof(float))
                {
                    if (type == typeof(Single))
                        memoryAddress = new IntPtr(Convert.ToInt32(BitConverter.ToInt32(BitConverter.GetBytes((Single)parameter[i]), 0)));
                    else if (type == typeof(Double))
                        memoryAddress = new IntPtr(Convert.ToInt32(BitConverter.ToInt32(BitConverter.GetBytes((Double)parameter[i]), 0)));
                    else if (type == typeof(float))
                        memoryAddress = new IntPtr(Convert.ToInt32(BitConverter.ToInt32(BitConverter.GetBytes((float)parameter[i]), 0)));
                }
                else
                    return;

                data.Add(0x68);
                data.AddRange(BitConverter.GetBytes((uint)memoryAddress));
            }

            data.Add(0xE8);
            int offset = (int)address - ((int)memory[memory.Length - 1] + (parameter.Length * 5 + 5));
            data.AddRange(BitConverter.GetBytes(offset));

            if (stackClear)
            {
                data.AddRange(new byte[] { 0x83, 0xC4 });
                data.Add(Convert.ToByte(parameter.Length * 4));
            }
            data.Add(0xC3);

            if (!WriteMemoryBytes(memory[memory.Length - 1], data.ToArray()))
                return;

            IntPtr thread = CreateRemoteThread(handle, IntPtr.Zero, 0, (uint)memory[memory.Length - 1], IntPtr.Zero, 0, IntPtr.Zero);
            WaitForSingleObject(thread, 0xFFFFFFFF);
        }

        internal static void Call(uint address, byte[] thisCall, bool stackClear, params object[] parameter)
        {
            List<byte> data = new List<byte>();

            data.AddRange(thisCall);

            int usedParameters = 0;
            for (int i = parameter.Length - 1; i >= 0; i--)
            {
                IntPtr memoryAddress = IntPtr.Zero;
                Type type = parameter[i].GetType();
                if (type == typeof(string) && usedParameters <= memory.Length - 1)
                {
                    memoryAddress = memory[usedParameters];
                    if (!WriteString(memoryAddress, (string)parameter[i]))
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
                else if (type == typeof(byte))
                {
                    memoryAddress = new IntPtr(Convert.ToInt32(parameter[i]));
                }
                else if (type == typeof(Single) || type == typeof(Double) || type == typeof(float))
                {
                    if (type == typeof(Single))
                        memoryAddress = new IntPtr(Convert.ToInt32(BitConverter.ToInt32(BitConverter.GetBytes((Single)parameter[i]), 0)));
                    else if (type == typeof(Double))
                        memoryAddress = new IntPtr(Convert.ToInt32(BitConverter.ToInt32(BitConverter.GetBytes((Double)parameter[i]), 0)));
                    else if (type == typeof(float))
                        memoryAddress = new IntPtr(Convert.ToInt32(BitConverter.ToInt32(BitConverter.GetBytes((float)parameter[i]), 0)));
                }
                /*
            else if (type == typeof(Single) && usedParameters <= parameterMemory.Length - 1)
            {
                memoryAddress = parameterMemory[usedParameters];
                if (!WriteFloat((uint)memoryAddress, (float)parameter[i]))
                    return;
                usedParameters++;
            }*/
                else
                    return;

                data.Add(0x68);
                data.AddRange(BitConverter.GetBytes((uint)memoryAddress));
            }

            data.Add(0xE8);
            int offset = (int)address - ((int)memory[memory.Length - 1] + ((parameter.Length * 5 + 5) + thisCall.Length));
            data.AddRange(BitConverter.GetBytes(offset));

            if (stackClear)
            {
                data.AddRange(new byte[] { 0x83, 0xC4 });
                data.Add(Convert.ToByte(parameter.Length * 4));
            }
            data.Add(0xC3);

            if (!WriteMemoryBytes(memory[memory.Length - 1], data.ToArray()))
                return;

            IntPtr thread = CreateRemoteThread(handle, IntPtr.Zero, 0, (uint)memory[memory.Length - 1], IntPtr.Zero, 0, IntPtr.Zero);
            WaitForSingleObject(thread, 0xFFFFFFFF);
        }

        internal static void Call(IntPtr address, bool stackClear, params object[] parameter)
        {
            List<byte> data = new List<byte>();

            int usedParameters = 0;

            for (int i = parameter.Length - 1; i >= 0; i--)
            {
                IntPtr memoryAddress = IntPtr.Zero;
                Type type = parameter[i].GetType();
                if (type == typeof(string) && usedParameters <= memory.Length - 1)
                {
                    memoryAddress = memory[usedParameters];
                    if (!WriteString(memoryAddress, (string)parameter[i]))
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
                else if (type == typeof(byte))
                {
                    memoryAddress = new IntPtr(Convert.ToInt32(parameter[i]));
                }
                else if (type == typeof(Single) || type == typeof(Double) || type == typeof(float))
                {
                    if (type == typeof(Single))
                        memoryAddress = new IntPtr(Convert.ToInt32(BitConverter.ToInt32(BitConverter.GetBytes((Single)parameter[i]), 0)));
                    else if (type == typeof(Double))
                        memoryAddress = new IntPtr(Convert.ToInt32(BitConverter.ToInt32(BitConverter.GetBytes((Double)parameter[i]), 0)));
                    else if (type == typeof(float))
                        memoryAddress = new IntPtr(Convert.ToInt32(BitConverter.ToInt32(BitConverter.GetBytes((float)parameter[i]), 0)));
                }
                else
                    return;

                data.Add(0x68);
                data.AddRange(BitConverter.GetBytes((uint)memoryAddress));
            }

            data.Add(0xE8);
            int offset = (int)address - ((int)memory[memory.Length - 1] + (parameter.Length * 5 + 5));
            data.AddRange(BitConverter.GetBytes(offset));

            if (stackClear)
            {
                data.AddRange(new byte[] { 0x83, 0xC4 });
                data.Add(Convert.ToByte(parameter.Length * 4));
            }
            data.Add(0xC3);

            if (!WriteMemoryBytes(memory[memory.Length - 1], data.ToArray()))
                return;

            IntPtr thread = CreateRemoteThread(handle, IntPtr.Zero, 0, (uint)memory[memory.Length - 1], IntPtr.Zero, 0, IntPtr.Zero);
            WaitForSingleObject(thread, 0xFFFFFFFF);
        }

        internal static void Call(IntPtr address, byte[] thisCall, bool stackClear, params object[] parameter)
        {
            List<byte> data = new List<byte>();

            data.AddRange(thisCall);

            int usedParameters = 0;
            for (int i = parameter.Length - 1; i >= 0; i--)
            {
                IntPtr memoryAddress = IntPtr.Zero;
                Type type = parameter[i].GetType();
                if (type == typeof(string) && usedParameters <= memory.Length - 1)
                {
                    memoryAddress = memory[usedParameters];
                    if (!WriteString(memoryAddress, (string)parameter[i]))
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
                else if (type == typeof(byte))
                {
                    memoryAddress = new IntPtr(Convert.ToInt32(parameter[i]));
                }
                else if (type == typeof(Single) || type == typeof(Double) || type == typeof(float))
                {
                    if (type == typeof(Single))
                        memoryAddress = new IntPtr(Convert.ToInt32(BitConverter.ToInt32(BitConverter.GetBytes((Single)parameter[i]), 0)));
                    else if (type == typeof(Double))
                        memoryAddress = new IntPtr(Convert.ToInt32(BitConverter.ToInt32(BitConverter.GetBytes((Double)parameter[i]), 0)));
                    else if (type == typeof(float))
                        memoryAddress = new IntPtr(Convert.ToInt32(BitConverter.ToInt32(BitConverter.GetBytes((float)parameter[i]), 0)));
                }
                /*
            else if (type == typeof(Single) && usedParameters <= parameterMemory.Length - 1)
            {
                memoryAddress = parameterMemory[usedParameters];
                if (!WriteFloat((uint)memoryAddress, (float)parameter[i]))
                    return;
                usedParameters++;
            }*/
                else
                    return;

                data.Add(0x68);
                data.AddRange(BitConverter.GetBytes((uint)memoryAddress));
            }

            data.Add(0xE8);
            int offset = (int)address - ((int)memory[memory.Length - 1] + ((parameter.Length * 5 + 5) + thisCall.Length));
            data.AddRange(BitConverter.GetBytes(offset));

            if (stackClear)
            {
                data.AddRange(new byte[] { 0x83, 0xC4 });
                data.Add(Convert.ToByte(parameter.Length * 4));
            }
            data.Add(0xC3);

            if (!WriteMemoryBytes(memory[memory.Length - 1], data.ToArray()))
                return;

            IntPtr thread = CreateRemoteThread(handle, IntPtr.Zero, 0, (uint)memory[memory.Length - 1], IntPtr.Zero, 0, IntPtr.Zero);
            WaitForSingleObject(thread, 0xFFFFFFFF);
        }

        private static void GenerateAddresses()
        {
            if (_process != null && !_process.HasExited && sampBase != IntPtr.Zero)
            {
                byte[] sampBytes;
                if (ReadMemoryBytes(sampBase, sampLength, out sampBytes))
                {
                    Chat.GenerateAddresses(sampBase);
                    Vehicle.GenerateAddresses(sampBase);
                    RemotePlayer.GenerateAddresses(sampBase);
                    Player.GenerateAddresses(sampBase);
                }
            }
        }

        private static void _process_Exited(object sender, EventArgs e)
        {
            _process = null;
            pid = 0;
            handle = IntPtr.Zero;
        }
    }
}