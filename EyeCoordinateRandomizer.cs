using PacificEngine.OW_CommonResources;
using PacificEngine.OW_CommonResources.Game.State;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PacificEngine.OW_Randomizer
{
    public static class EyeCoordinateRandomizer
    {
        private static RandomizerSeeds seeds;
        public static RandomizerSeeds.Type? type { get { return seeds?.type; } }

        public static void Update()
        {
            randomizeCoordinates();
        }

        public static void reset()
        {
            seeds.reset();
        }

        public static void updateSeed(int seed, RandomizerSeeds.Type? type)
        {
            if (!type.HasValue)
            {
                seeds = null;
                resetCoordinates();
            }
            else
            {
                seeds = new RandomizerSeeds(seed, type.Value);
                randomizeCoordinates();
            }
        }

        private static void resetCoordinates()
        {
            EyeCoordinates.setCoordinates(new int[] { 1, 5, 4 }, new int[] { 3, 0, 1, 4 }, new int[] { 1, 2, 3, 0, 5, 4 });
        }

        private static void randomizeCoordinates()
        {
            EyeCoordinates.setCoordinates(generateCoordinate(), generateCoordinate(), generateCoordinate());
        }

        private static int[] generateCoordinate()
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
            Shuffle(list);

            return list.ToArray();
        }

        private static void Shuffle<T>(this IList<T> list)
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
