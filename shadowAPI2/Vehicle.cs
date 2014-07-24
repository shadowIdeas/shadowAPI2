using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace shadowAPI2
{
    public class Vehicle
    {
        private static Vehicle instance;
        private string[] vehicleNames = {
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

        private Vehicle()
        {

        }

        public static Vehicle GetInstance()
        {
            if (instance == null)
                instance = new Vehicle();

            return instance;
        }


        internal uint IsInVehicle() // TODO In Player class?
        {
            if (!Memory.IsInit)
                Memory.Init(Memory._processName);

            uint result = BitConverter.ToUInt32(Memory.ReadMemory(Memory.vehicleOffsetBase, 4), 0);

            return result;
        }

        /// <summary>
        /// Get the Dl of the vehicle
        /// </summary>
        /// <returns>Dl</returns>
        public float GetDl()
        {
            uint vehicle = 0;
            float damage = -1.0f;
            if ((vehicle = IsInVehicle()) != 0)
            {
                damage = Memory.ReadFloat(vehicle + Memory.vehicleOffsetDamage);
            }

            return damage;
        }

        /// <summary>
        /// Get the speed of the current vehicle
        /// </summary>
        /// <returns>Speed</returns>
        public float GetSpeed() // Buggy
        {
            uint vehicle = 0;
            float speed = -1.0f;
            if ((vehicle = IsInVehicle()) != 0)
            {
                float x = Math.Abs(Memory.ReadFloat(vehicle + Memory.vehicleOffsetSpeedX));
                float y = Math.Abs(Memory.ReadFloat(vehicle + Memory.vehicleOffsetSpeedY));
                float z = Math.Abs(Memory.ReadFloat(vehicle + Memory.vehicleOffsetSpeedZ));

                speed = (x + y + z) * 140;
            }

            return speed;
        }

        /// <summary>
        /// Get the lock state of the current vehicle
        /// </summary>
        /// <returns>Lock state</returns>
        public bool IsVehicleLocked()
        {
            uint vehicle = 0;
            bool locked = false;
            if ((vehicle = IsInVehicle()) != 0)
            {
                int state = Memory.ReadInteger(vehicle + Memory.vehicleOffsetLockState);
                if (state == 2)
                    locked = true;
            }

            return locked;
        }

        /// <summary>
        /// Get the model id of the current vehicle
        /// </summary>
        /// <returns>Id</returns>
        public int GetModelId()
        {
            uint vehicle = 0;
            int id = -1;
            if ((vehicle = IsInVehicle()) != 0)
            {
                id = Memory.ReadInteger16(vehicle + Memory.vehicleOffsetModelId);
            }

            return id;
        }

        /// <summary>
        /// Get the SAMP spawn id of the current vehicle
        /// </summary>
        /// <returns>SAMP spawn id</returns>
        public int GetSpawnId()
        {
            uint vehicle = 0;
            int id = -1;
            if ((vehicle = IsInVehicle()) != 0)
            {
                id = Memory.ReadInteger16(Memory.vehicleId);
            }

            return id;
        }

        public float GetCollideStatus()
        {
            uint vehicle = 0;
            float collideStatus = -1.0f;
            if ((vehicle = IsInVehicle()) != 0)
            {
                collideStatus = Memory.ReadFloat(vehicle + Memory.vehicleOffsetCollideStatus);
            }

            return collideStatus;
        }

        /// <summary>
        /// Get the state of the seats in the current vehicle (seat is used or not)
        /// </summary>
        /// <returns>A bool-Array with the seats\n0 is front left\n 1 is front right\n2 is back left\n3 is back right </returns>
        public bool[] GetCurrentSeatStates()
        {
            uint vehicle = 0;
            bool[] seatStates = new bool[4];
            if ((vehicle = IsInVehicle()) != 0)
            {
                for (int i = 0; i < seatStates.Length; i++)
                {
                    if (Memory.ReadInteger(vehicle + (Memory.vehicleOffsetSeats * 4)) != 0)
                        seatStates[i] = true;
                }
            }

            return seatStates;
        }

        /// <summary>
        /// Check if the engine of the current vehicle is enabled
        /// </summary>
        /// <returns>True if it's enabled, false if not</returns>
        public bool IsEngineEnabled()
        {
            uint vehicle = 0;
            bool enabled = false;
            if ((vehicle = IsInVehicle()) != 0)
            {
                byte state = Memory.ReadByte(vehicle + Memory.vehicleOffsetEngineState);
                if (state == 24 || state == 56 || state == 88 || state == 120)
                    enabled = true;
            }

            return enabled;
        }

        /// <summary>
        /// Get the vehicle model name of the current vehicle
        /// </summary>
        /// <returns>Model name</returns>
        public string GetModelName()
        {
            uint vehicle = 0;
            string vehicleMame = "";
            if ((vehicle = IsInVehicle()) != 0)
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
        /// Get the 
        /// </summary>
        /// <param name="modelId"></param>
        /// <returns></returns>
        public string GetModelNameByModelId(int modelId)
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