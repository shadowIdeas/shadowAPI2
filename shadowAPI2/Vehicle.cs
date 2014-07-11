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
                Memory.Init();

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
            bool state = false;
            if ((vehicle = IsInVehicle()) != 0)
            {
                state = Memory.ReadBoolean(vehicle + Memory.vehicleOffsetLockState);
            }

            return state;
        }

        /// <summary>
        /// Get the model id of the current vehicle
        /// </summary>
        /// <returns>Id</returns>
        public int GetId()
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
                if (Memory.ReadByte(vehicle + Memory.vehicleOffsetEngineState) == 24)
                    enabled = true;
            }

            return enabled;
        }
    }
}