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
        private static int _cycles = 0;
        private static int _totalCycles = 0;
        private static float _lastUpdate = 0f;
        private static bool _isSet = false;
        private static RandomizerSeeds seeds;
        public static RandomizerSeeds.Type? type { get { return seeds?.type; } }

        public static void Start()
        {
        }

        public static void Update()
        {
            if (Time.time - _lastUpdate > 60f)
            {
                _totalCycles++;
                _lastUpdate = Time.time;
                if (type == RandomizerSeeds.Type.Minute || type == RandomizerSeeds.Type.SeedlessMinute)
                {
                    _isSet = false;
                }
            }

            updateValues(0);
        }

        public static void Reset()
        {
            seeds.reset();
            _isSet = false;
            _totalCycles = 0;
            _lastUpdate = Time.time;

            updateValues(0);
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
            _cycles = 0;
            _isSet = false;

            if (EyeCoordinateRandomizer.type == RandomizerSeeds.Type.Minute || EyeCoordinateRandomizer.type == RandomizerSeeds.Type.SeedlessMinute)
            {
                updateValues(_totalCycles);
            }
            updateValues(0);
        }


        private static void updateValues(int cycles)
        {
            if (!_isSet)
            {
                _isSet = true;
                if (type == null)
                {
                    defaultValues();
                }
                else
                {
                    randomizeValues(cycles);
                }
            }
        }

        private static void defaultValues()
        {
            EyeCoordinates.coordinates = EyeCoordinates.defaultCoordinates;
        }

        private static void randomizeValues(int cycles)
        {
            while (cycles-- > 0)
            {
                getRandomizeValues();
            }
            EyeCoordinates.coordinates = getRandomizeValues();
        }

        private static Tuple<int[], int[], int[]> getRandomizeValues()
        {
            return Tuple.Create(generateCoordinate(), generateCoordinate(), generateCoordinate());
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
