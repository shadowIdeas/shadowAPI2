using System;
using System.Collections.Generic;
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
        public static void Init(string processName = "rgn_ac_gta")
        {
            Memory.Init(processName);
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
