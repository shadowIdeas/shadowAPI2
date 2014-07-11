using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace shadowAPI2TestApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Test Application started, press ENTER to start the test");
            Console.ReadLine();
            shadowAPI2.API.Init();

            Console.WriteLine("Waiting 5 Seconds...");
            System.Threading.Thread.Sleep(5000);

            Console.WriteLine("Sending Test-Message (UTF-8 Symbols included)...");
            shadowAPI2.Chat.GetInstance().Send("UTF8: äüéö <= Gehts? :>");            

            int id = -1;
            String name = "";
            while(id == -1)
            {
                Console.Write("Which player-name should be queried: ");
                name = Console.ReadLine();
                id = shadowAPI2.RemotePlayer.GetInstance().GetPlayerIdByName(name,true);
                if (id == -1)
                    Console.WriteLine("Player '" + name + "' not found");
            }
            Console.WriteLine("Player '" + name + "' has the ID: " + id);

            Console.WriteLine("Testing AddChatMessage...");
            shadowAPI2.Chat.GetInstance().AddMessage("ID von " + name + ": " + id,Color.Yellow);
            shadowAPI2.Chat.GetInstance().AddMessage("Name von ID " + id + ": " + shadowAPI2.RemotePlayer.GetInstance().GetPlayerNameById((uint)id),Color.Turquoise);

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Test completed [You can go into SA:MP and check your Chat-Messages]");
            Console.ReadLine();

            //Old
            //shadowAPI2.Chat.SendChat("/b Test");
            //shadowAPI2.Chat.SendChat("/b Test", 2);
            //shadowAPI2.Chat.SendChat("Test");
        }
    }
}
