using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace shadowAPI2
{
    public class Vehicle
    {
        static uint IsInVehicle()
        {
            if (!Memory.isInit)
                Memory.Init();

            uint result = BitConverter.ToUInt32(Memory.ReadMemory(Memory.vehicleOffsetBase, 4), 0);

            return result;
        }

        public static float VehicleDamage()
        {
            uint vehicle = 0;
            if ((vehicle = IsInVehicle()) == 0)
                return 0.0f;

            float result = Memory.ReadFloat(vehicle + Memory.vehicleOffsetDamage);

            return result;
        }

        public static float VehicleSpeed() // Buggy
        {
            uint vehicle = 0;
            if ((vehicle = IsInVehicle()) == 0)
                return 0.0f;

            float x = Math.Abs(Memory.ReadFloat(vehicle + Memory.vehicleOffsetSpeedX));
            float y = Math.Abs(Memory.ReadFloat(vehicle + Memory.vehicleOffsetSpeedY));
            float z = Math.Abs(Memory.ReadFloat(vehicle + Memory.vehicleOffsetSpeedZ));

            float result = (x + y + z) * 140; ;

            return result;
        }

        public static int VehicleCurrentId()
        {
            uint vehicle = 0;
            if ((vehicle = IsInVehicle()) == 0)
                return 0;

            int result = Memory.ReadInteger(vehicle + Memory.vehicleOffsetId);

            return result;
        }

        public static float VehicleCollideStatus()
        {
            uint vehicle = 0;
            if ((vehicle = IsInVehicle()) == 0)
                return 0.0f;

            float result = Memory.ReadFloat(vehicle + Memory.vehicleOffsetCollideStatus);

            return result;
        }
    }
}
