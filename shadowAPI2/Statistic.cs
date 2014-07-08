using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace shadowAPI2
{
    public class Statistic
    {
        private static Statistic instance;

        private Statistic()
        {

        }

        public static Statistic GetInstance()
        {
            if (instance == null)
                instance = new Statistic();

            return instance;
        }

        public float StatisticFeetMeters()
        {
            if (!Memory.IsInit)
                Memory.Init();

            float result = Memory.ReadFloat(Memory.statisticFeetMeters);

            return result;
        }

        public float StatisticVehicleMeters()
        {
            if (!Memory.IsInit)
                Memory.Init();

            float result = Memory.ReadFloat(Memory.statisticVehicleMeters);

            return result;
        }

        public float StatisticBikeMeters()
        {
            if (!Memory.IsInit)
                Memory.Init();

            float result = Memory.ReadFloat(Memory.statisticBikeMeters);

            return result;
        }

        public float StatisticHelicopterMeters()
        {
            if (!Memory.IsInit)
                Memory.Init();

            float result = Memory.ReadFloat(Memory.statisticHelicopterMeters);

            return result;
        }

        public float StatisticShipMeters()
        {
            if (!Memory.IsInit)
                Memory.Init();

            float result = Memory.ReadFloat(Memory.statisticShipMeters);

            return result;
        }

        public float StatisticSwimMeters()
        {
            if (!Memory.IsInit)
                Memory.Init();

            float result = Memory.ReadFloat(Memory.statisticSwimMeters);

            return result;
        }
    }
}
