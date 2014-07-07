using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace shadowAPI2
{
    public class Statistic
    {
        public static float StatisticFeetMeters()
        {
            if (!Memory.IsInit)
                Memory.Init();

            float result = Memory.ReadFloat(Memory.statisticFeetMeters);

            return result;
        }
        public static float StatisticVehicleMeters()
        {
            if (!Memory.IsInit)
                Memory.Init();

            float result = Memory.ReadFloat(Memory.statisticVehicleMeters);

            return result;
        }
        public static float StatisticBikeMeters()
        {
            if (!Memory.IsInit)
                Memory.Init();

            float result = Memory.ReadFloat(Memory.statisticBikeMeters);

            return result;
        }
        public static float StatisticHelicopterMeters()
        {
            if (!Memory.IsInit)
                Memory.Init();

            float result = Memory.ReadFloat(Memory.statisticHelicopterMeters);

            return result;
        }
        public static float StatisticShipMeters()
        {
            if (!Memory.IsInit)
                Memory.Init();

            float result = Memory.ReadFloat(Memory.statisticShipMeters);

            return result;
        }
        public static float StatisticSwimMeters()
        {
            if (!Memory.IsInit)
                Memory.Init();

            float result = Memory.ReadFloat(Memory.statisticSwimMeters);

            return result;
        }
    }
}
