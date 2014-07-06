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
            if (!Memory.isInit)
                Memory.Init();

            float result = Memory.ReadFloat(Memory.statisticFeetMeters);

            return result;
        }
        public static float StatisticVehicleMeters()
        {
            if (!Memory.isInit)
                Memory.Init();

            float result = Memory.ReadFloat(Memory.statisticVehicleMeters);

            return result;
        }
        public static float StatisticBikeMeters()
        {
            if (!Memory.isInit)
                Memory.Init();

            float result = Memory.ReadFloat(Memory.statisticBikeMeters);

            return result;
        }
        public static float StatisticHelicopterMeters()
        {
            if (!Memory.isInit)
                Memory.Init();

            float result = Memory.ReadFloat(Memory.statisticHelicopterMeters);

            return result;
        }
        public static float StatisticShipMeters()
        {
            if (!Memory.isInit)
                Memory.Init();

            float result = Memory.ReadFloat(Memory.statisticShipMeters);

            return result;
        }
        public static float StatisticSwimMeters()
        {
            if (!Memory.isInit)
                Memory.Init();

            float result = Memory.ReadFloat(Memory.statisticSwimMeters);

            return result;
        }
    }
}
