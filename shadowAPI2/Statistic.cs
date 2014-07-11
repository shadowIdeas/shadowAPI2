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

        public float GetFeetMeters()
        {
            if (!Memory.IsInit)
                Memory.Init();

            float result = Memory.ReadFloat(Memory.statisticFeetMeters);

            return result;
        }

        public float GetStatisticVehicleMeters()
        {
            if (!Memory.IsInit)
                Memory.Init();

            float result = Memory.ReadFloat(Memory.statisticVehicleMeters);

            return result;
        }

        public float GetStatisticBikeMeters()
        {
            if (!Memory.IsInit)
                Memory.Init();

            float result = Memory.ReadFloat(Memory.statisticBikeMeters);

            return result;
        }

        public float GetStatisticHelicopterMeters()
        {
            if (!Memory.IsInit)
                Memory.Init();

            float result = Memory.ReadFloat(Memory.statisticHelicopterMeters);

            return result;
        }

        public float GetStatisticShipMeters()
        {
            if (!Memory.IsInit)
                Memory.Init();

            float result = Memory.ReadFloat(Memory.statisticShipMeters);

            return result;
        }

        public float GetStatisticSwimMeters()
        {
            if (!Memory.IsInit)
                Memory.Init();

            float result = Memory.ReadFloat(Memory.statisticSwimMeters);

            return result;
        }
    }
}
