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
        private static shadowAPI2.Chat chat = shadowAPI2.Chat.GetInstance();
        private static shadowAPI2.Player player = shadowAPI2.Player.GetInstance();
        private static shadowAPI2.RemotePlayer remotePlayer = shadowAPI2.RemotePlayer.GetInstance();
        static void Main(string[] args)
        {
            shadowAPI2.API.Init(); // Don't needed

            chat.OnChatMessage += OnChatMessage;

            Console.WriteLine("Test Application started, press ENTER to start the test");
            Console.ReadLine();

            Console.WriteLine("Waiting 5 Seconds...");
            System.Threading.Thread.Sleep(5000);

            Console.WriteLine("Sending Test-Message (UTF-8 Symbols included)...");
            chat.Send("This is a test with UTF-8 symbols (üäöéú)");
            chat.AddMessage("My ID: " + player.GetId());

            int id = -1;
            String name = "";
            while (id == -1)
            {
                Console.Write("Which player-name should be queried: ");
                name = Console.ReadLine();
                id = remotePlayer.GetPlayerIdByName(name, true);
                if (id == -1)
                    Console.WriteLine("Player '" + name + "' not found");
            }
            Console.WriteLine("Player '" + name + "' has the ID: " + id);
            Console.WriteLine("Score of the player: " + remotePlayer.GetPlayerScoreById((uint)id));

            Console.WriteLine("Testing AddMessage...");
            chat.AddMessage("ID of " + name + ": " + id, Color.Yellow);
            chat.AddMessage("Name of ID " + id + ": " + remotePlayer.GetPlayerNameById((uint)id), Color.Turquoise);
            chat.AddMessage("Score of ID " + id + ": " + remotePlayer.GetPlayerScoreById((uint)id));

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Test completed [You can go into SA:MP and check your Chat-Messages]");
            Console.ReadLine();
        }

        private static void OnChatMessage(DateTime time, string message)
        {
            if(message.EndsWith("shadowAPI2"))
            {
                chat.AddMessage("Anyone written 'shadowAPI2'!");
            }
        }
    }
}