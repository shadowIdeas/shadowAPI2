using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace shadowAPI2
{
    public class Vehicle
    {
        private static IntPtr numberplate;
        private static string[] vehicleNames = {
                                            "Landstalker","Bravura","Buffalo","Linerunner","Perrenial","Sentinel",
                                            "Dumper","Firetruck","Trashmaster","Stretch","Manana","Infernus",
                                            "Voodoo","Pony","Mule","Cheetah","Ambulance","Leviathan","Moonbeam",
                                            "Esperanto","Taxi","Washington","Bobcat","Whoopee","BF Injection",
                                            "Hunter","Premier","Enforcer","Securicar","Banshee","Predator","Bus",
                                            "Rhino","Barracks","Hotknife","Trailer","Previon","Coach","Cabbie",
                                            "Stallion","Rumpo","RC Bandit","Romero","Packer","Monster","Admiral",
                                            "Squalo","Seasparrow","Pizzaboy","Tram","Trailer","Turismo","Speeder",
                                            "Reefer", "Tropic", "Flatbed", "Yankee", "Caddy", "Solair", "Berkley's RC Van",
                                            "Skimmer", "PCJ-600", "Faggio", "Freeway", "RC Baron", "RC Raider", "Glendale",
                                            "Oceanic","Sanchez", "Sparrow", "Patriot", "Quad", "Coastguard", "Dinghy",
                                            "Hermes", "Sabre", "Rustler", "ZR-350", "Walton", "Regina", "Comet", "BMX",
                                            "Burrito", "Camper", "Marquis", "Baggage", "Dozer", "Maverick", "News Chopper",
                                            "Rancher", "FBI Rancher", "Virgo", "Greenwood", "Jetmax", "Hotring", "Sandking",
                                            "Blista Compact", "Police Maverick", "Boxvillde", "Benson", "Mesa", "RC Goblin",
                                            "Hotring Racer A", "Hotring Racer B", "Bloodring Banger", "Rancher", "Super GT",
                                            "Elegant", "Journey", "Bike", "Mountain Bike", "Beagle", "Cropduster", "Stunt",
                                            "Tanker", "Roadtrain", "Nebula", "Majestic", "Buccaneer", "Shamal", "hydra",
                                            "FCR-900", "NRG-500", "HPV1000", "Cement Truck", "Tow Truck", "Fortune",
                                            "Cadrona", "FBI Truck", "Willard", "Forklift", "Tractor", "Combine", "Feltzer",
                                            "Remington", "Slamvan", "Blade", "Freight", "Streak", "Vortex", "Vincent",
                                            "Bullet", "Clover", "Sadler", "Firetruck", "Hustler", "Intruder", "Primo",
                                            "Cargobob", "Tampa", "Sunrise", "Merit", "Utility", "Nevada", "Yosemite",
                                            "Windsor", "Monster", "Monster", "Uranus", "Jester", "Sultan", "Stratum",
                                            "Elegy", "Raindance", "RC Tiger", "Flash", "Tahoma", "Savanna", "Bandito",
                                            "Freight Flat", "Streak Carriage", "Kart", "Mower", "Dune", "Sweeper",
                                            "Broadway","Tornado","AT-400","DFT-30","Huntley","Stafford","BF-400",
                                            "News Van","Tug","Trailer","Emperor","Wayfarer","Euros","Hotdog","Club",
                                            "Freight Box","Trailer","Andromada","Dodo","RC Cam","Launch","Police Car",
                                            "Police Car","Police Car","Police Ranger","Picador","S.W.A.T","Alpha",
                                            "Phoenix","Glendale Shit","Sadler Shit","Luggage","Luggage","Stairs","Boxville",
                                            "Tiller","Utility Trailer"
                                            };

        public static void GenerateAddresses(IntPtr sampBase)
        {
            numberplate = IntPtr.Add(sampBase, 0xD4174);
        }

        public static IntPtr GetVehiclePointer()
        {
            Memory.Init();

            var pointer = IntPtr.Zero;
            Memory.ReadMemory<IntPtr>(0xBA18FC, out pointer);
            return pointer;
        }

        /// <summary>
        /// Get the lock state of the current vehicle
        /// </summary>
        /// <returns>Lock state</returns>
        public static bool IsLocked()
        {
            Memory.Init();

            var state = 0;
            if (Memory.ReadMemory<int>(GetVehiclePointer() + 0x4F8, out state))
                if (state == 2)
                    return true;

            return false;
        }

        /// <summary>
        /// Get the Dl of the vehicle
        /// </summary>
        /// <returns>Dl</returns>
        public static float GetHealth()
        {
            Memory.Init();

            var health = 0.0f;
            Memory.ReadMemory<float>(GetVehiclePointer() + 0x4C0, out health);
            return health;
        }

        /// <summary>
        /// Get the model id of the current vehicle
        /// </summary>
        /// <returns>Id</returns>
        public static int GetModelId()
        {
            Memory.Init();

            Int16 id = 0;
            Memory.ReadMemory<Int16>(GetVehiclePointer() + 0x22, out id);
            return (int)id;
        }

        public static string GetNumberplate()
        {
            Memory.Init();

            var numberplateString = Memory.ReadString(numberplate, 8);
            return numberplateString;
        }

        /// <summary>
        /// Get the speed of the current vehicle
        /// </summary>
        /// <returns>Speed</returns>
        public static float GetSpeed()
        {
            IntPtr vehicle = IntPtr.Zero;
            float speed = -1.0f;
            if ((vehicle = GetVehiclePointer()) != IntPtr.Zero)
            {
                float x = 0;
                float y = 0;
                float z = 0;
                Memory.ReadMemory<float>(IntPtr.Add(vehicle, 0x44), out x);
                Memory.ReadMemory<float>(IntPtr.Add(vehicle, 0x48), out y);
                Memory.ReadMemory<float>(IntPtr.Add(vehicle, 0x4C), out z);
                x = Math.Abs(x);
                y = Math.Abs(y);
                z = Math.Abs(z);

                speed = (float)(Math.Pow((Math.Pow(x, 2) + Math.Pow(y, 2) + Math.Pow(z, 2)), 0.5) * 1.42 * 100);
            }

            return speed;
        }

        /// <summary>
        /// Get the model id of the current vehicle
        /// </summary>
        /// <returns>Id</returns>
        public static bool IsBike()
        {
            IntPtr vehicle = IntPtr.Zero;
            bool isBike = false;
            if ((vehicle = GetVehiclePointer()) != IntPtr.Zero)
            {
                switch (GetModelId())
                {
                    case 448:
                    case 581:
                    case 522:
                    case 461:
                    case 523:
                    case 463:
                    case 586:
                    case 471:
                        isBike = true;
                        break;
                }
            }
            return isBike;
        }

        /// <summary>
        /// Get the SAMP spawn id of the current vehicle
        /// </summary>
        /// <returns>SAMP spawn id</returns>
        /*public int GetSpawnId()
        {
            IntPtr vehicle = IntPtr.Zero;
            int id = -1;
            if ((vehicle = GetVehiclePointer()) != IntPtr.Zero)
            {
                id = Memory.ReadInteger16(Memory.vehicleId);
            }

            return id;
        }*/

        /// <summary>
        /// Get the state of the seats in the current vehicle (seat is used or not)
        /// </summary>
        /// <returns>A bool-Array with the seats\n0 is front left\n 1 is front right\n2 is back left\n3 is back right </returns>
        public static bool[] GetCurrentSeatStates()
        {
            IntPtr vehicle = IntPtr.Zero;
            bool[] seatStates = new bool[4];
            if ((vehicle = GetVehiclePointer()) != IntPtr.Zero)
            {
                for (int i = 0; i < seatStates.Length; i++)
                {
                    int temp = 0;
                    if (Memory.ReadMemory<int>(IntPtr.Add(vehicle, (0x460 + (i * 0x4))), out temp))
                        if (temp != 0)
                            seatStates[i] = true;
                }
            }

            return seatStates;
        }

        /// <summary>
        /// Check if the engine of the current vehicle is enabled
        /// </summary>
        /// <returns>True if it's enabled, false if not</returns>
        public static bool IsEngineEnabled()
        {
            IntPtr vehicle = IntPtr.Zero;
            bool enabled = false;
            if ((vehicle = GetVehiclePointer()) != IntPtr.Zero)
            {
                byte state = 0;
                Memory.ReadMemory<byte>(IntPtr.Add(vehicle, 0x428), out state);
                if (state == 24 || state == 56 || state == 88 || state == 120)
                    enabled = true;
            }

            return enabled;
        }

        /// <summary>
        /// Get the vehicle model name of the current vehicle
        /// </summary>
        /// <returns>Model name</returns>
        public static string GetModelName()
        {
            IntPtr vehicle = IntPtr.Zero;
            string vehicleMame = "";
            if ((vehicle = GetVehiclePointer()) != IntPtr.Zero)
            {
                int modelid = GetModelId();
                if (modelid > 400 && modelid < 611)
                {
                    vehicleMame = vehicleNames[modelid - 400];
                }
            }
            return vehicleMame;
        }

        /// <summary>
        /// Get the model name by id
        /// </summary>
        /// <param name="modelId"></param>
        /// <returns></returns>
        public static string GetModelNameByModelId(int modelId)
        {
            string vehicleName = "";
            if (modelId > 400 && modelId < 611)
            {
                vehicleName = vehicleNames[modelId - 400];
            }

            return vehicleName;
        }
    }
}