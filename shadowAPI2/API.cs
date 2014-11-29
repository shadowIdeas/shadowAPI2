using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace shadowAPI2
{
    public static class API
    {
        /// <summary>
        /// Initialize manually the API (only need the first time)
        /// </summary>
        public static bool TryInit(string processName = "rgn_ac_gta")
        {
            return Memory.Init(processName);
        }

        public static bool TryInit(Process process)
        {
            return Memory.Init(process);
        }

        public static void Init(string processName = "rgn_ac_gta")
        {
            Memory.Init(processName);
        }

        public static void Init(Process process)
        {
            Memory.Init(process);
        }

        /// <summary>
        /// Uninitialize manually the API 
        /// </summary>
        public static void UnInit()
        {
            Memory.UnInit();
        }
    }
}
