using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace shadowAPI2
{
    public static class Util
    {
        /// <summary>
        /// Convert a Color to Hex (RGB)
        /// </summary>
        /// <param name="color">Color convert to Hex</param>
        /// <returns></returns>
        public static String ColorToHexRGB(Color color)
        {
            return color.R.ToString("X2") + color.G.ToString("X2") + color.B.ToString("X2");
        }

        //Not used ATM
        /// <summary>
        /// Convert a Color to Hex (ARGB)
        /// </summary>
        /// <param name="color">Color convert to Hex</param>
        /// <returns></returns>
        public static String ColorToHexARGB(Color color)
        {
            return color.A.ToString("X2") + color.R.ToString("X2") + color.G.ToString("X2") + color.B.ToString("X2");
        }
    }
}
