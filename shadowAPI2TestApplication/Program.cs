using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using shadowAPI2;

namespace shadowAPI2TestApplication
{
    class Program 
    {

        static void Main(string[] args)
        {
            Console.ReadLine();

            for (int i = 0; i < 100; i++)
                Console.WriteLine(shadowAPI2.RemotePlayer.GetPlayerNameById((uint)i));
            Console.Read();
        }
    }
}