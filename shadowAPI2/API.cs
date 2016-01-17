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
        public static void Init(Process process)
        {
            Memory.Init();
        }
    }
}
