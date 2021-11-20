using PacificEngine.OW_CommonResources;
using PacificEngine.OW_CommonResources.Game.Config;
using PacificEngine.OW_CommonResources.Game.State;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace PacificEngine.OW_Randomizer
{
    public class EyeCoordinateRandomizer : AbstractRandomizer
    {
        public static EyeCoordinateRandomizer instance { get; } = new EyeCoordinateRandomizer();

        public new void updateSeed(int seed, RandomizerSeeds.Type? type)
        {
            base.updateSeed(seed, type);
        }

        protected override void defaultValues()
        {
            EyeCoordinates.coordinates = EyeCoordinates.defaultCoordinates;
        }

        protected override void randomizeValues(int cycles)
        {
            while (cycles-- > 0)
            {
                getRandomizeValues();
            }
            EyeCoordinates.coordinates = getRandomizeValues();
        }

        private Tuple<int[], int[], int[]> getRandomizeValues()
        {
            return Tuple.Create(generateCoordinate(), generateCoordinate(), generateCoordinate());
        }

        private int[] generateCoordinate()
        {
            var coodinate = seeds.Next(0, 63);
            var list = new List<int>();
            if ((coodinate & 0x1) != 0)
            {
                list.Add(1);
            }
            if ((coodinate & 0x2) != 0)
            {
                list.Add(2);
            }
            if ((coodinate & 0x4) != 0)
            {
                list.Add(3);
            }
            if ((coodinate & 0x8) != 0)
            {
                list.Add(4);
            }
            if ((coodinate & 0x10) != 0)
            {
                list.Add(5);
            }
            if ((coodinate & 0x20) != 0)
            {
                list.Add(0);
            }
            Shuffle(ref list);

            return list.ToArray();
        }

        private void Shuffle<T>(ref List<T> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = seeds.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }
    }
}
