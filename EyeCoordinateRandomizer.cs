using PacificEngine.OW_CommonResources;
using PacificEngine.OW_CommonResources.Game.State;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace PacificEngine.OW_Randomizer
{
    public static class EyeCoordinateRandomizer
    {
        private static float _lastUpdate = 0f;
        private static bool _isSet = false;
        private static RandomizerSeeds seeds;
        public static RandomizerSeeds.Type? type { get { return seeds?.type; } }

        public static void Start()
        {
        }

        public static void Update()
        {
            if (Time.time - _lastUpdate > 60f && (BramblePortalRandomizer.type == RandomizerSeeds.Type.Minute || BramblePortalRandomizer.type == RandomizerSeeds.Type.SeedlessMinute))
            {
                _isSet = false;
            }

            if (!_isSet)
            {
                _isSet = true;
                _lastUpdate = Time.time;
                if (type == null)
                {
                    defaultValues();
                }
                else
                {
                    randomizeValues();
                }
            }
        }

        public static void Reset()
        {
            seeds.reset();
            _isSet = false;
        }

        public static void updateSeed(int seed, RandomizerSeeds.Type? type)
        {
            if (!type.HasValue)
            {
                seeds = null;
            }
            else
            {
                seeds = new RandomizerSeeds(seed, type.Value);
            }
            _isSet = false;
        }

        private static void defaultValues()
        {
            EyeCoordinates.setCoordinates(new int[] { 1, 5, 4 }, new int[] { 3, 0, 1, 4 }, new int[] { 1, 2, 3, 0, 5, 4 });
        }

        private static void randomizeValues()
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
