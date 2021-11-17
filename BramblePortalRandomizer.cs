using PacificEngine.OW_CommonResources;
using PacificEngine.OW_CommonResources.Config;
using PacificEngine.OW_CommonResources.Game;
using PacificEngine.OW_CommonResources.Game.Resource;
using PacificEngine.OW_CommonResources.Game.State;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace PacificEngine.OW_Randomizer
{
    public static class BramblePortalRandomizer
    {
        private static int _totalCycles = 0;
        private static int _vesselSpawnCount = 1;
        private static int _exitSpawnCount = 2;
        private static float _lastUpdate = 0f;
        private static bool _isSet = false;
        private static RandomizerSeeds seeds;
        public static RandomizerSeeds.Type? type { get { return seeds?.type; } }

        public static void Start()
        {
            BramblePortals.onBrambleWarp += onBrambleWarp;
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

        public static void updateSeed(int seed, RandomizerSeeds.Type? type, int exits, int vessels)
        {
            if (!type.HasValue)
            {
                seeds = null;
            }
            else
            {
                seeds = new RandomizerSeeds(seed, type.Value);
            }

            _exitSpawnCount = exits;
            _vesselSpawnCount = vessels;

            _isSet = false;

            if (BramblePortalRandomizer.type == RandomizerSeeds.Type.Minute || BramblePortalRandomizer.type == RandomizerSeeds.Type.SeedlessMinute)
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
            BramblePortals.mapping = BramblePortals.defaultMapping;
        }

        private static void randomizeValues(int cycles)
        {
            var mapping = BramblePortals.mapping;
            while (cycles-- > 0)
            {
                getRandomizeValues(ref mapping);
            }
            BramblePortals.mapping = getRandomizeValues(ref mapping);
        }

        private static Tuple<Dictionary<Tuple<Position.HeavenlyBodies, int>, Tuple<Position.HeavenlyBodies, int>>, Dictionary<Tuple<Position.HeavenlyBodies, int>, Tuple<Position.HeavenlyBodies, int>>>
            getRandomizeValues(ref Tuple<Dictionary<Tuple<Position.HeavenlyBodies, int>, Tuple<Position.HeavenlyBodies, int>>, Dictionary<Tuple<Position.HeavenlyBodies, int>, Tuple<Position.HeavenlyBodies, int>>> mapping)
        {
            var outerMapping = mapping.Item1;
            var innerMapping = mapping.Item2;

            var newInner = new Dictionary<Tuple<Position.HeavenlyBodies, int>, Tuple<Position.HeavenlyBodies, int>>();
            var newOuter = new Dictionary<Tuple<Position.HeavenlyBodies, int>, Tuple<Position.HeavenlyBodies, int>>();

            int maxRetry = 0;
            do
            {
                maxRetry++;

                newInner.Clear();
                newOuter.Clear();
                foreach (var portal in outerMapping)
                {
                    var keys = new List<Tuple<Position.HeavenlyBodies, int>>(innerMapping.Keys);
                    newOuter[portal.Key] = randomizeOuterPortal(ref keys, portal.Key);
                }

                foreach (var portal in innerMapping)
                {
                    var keys = new List<Tuple<Position.HeavenlyBodies, int>>(outerMapping.Keys);
                    newInner[portal.Key] = randomizeInnerPortal(ref keys, portal.Key);
                }

                for (int i = 0; i < _exitSpawnCount; i++)
                {
                    addExit(ref newOuter);
                }
                for (int i = 0; i < _vesselSpawnCount; i++)
                {
                    addVessel(ref newInner);
                }
            } while (!verifyAccessible(ref newOuter, ref newInner) && maxRetry < 500);

            return Tuple.Create(newOuter, newInner);
        }

        private static bool verifyAccessible(ref Dictionary<Tuple<Position.HeavenlyBodies, int>, Tuple<Position.HeavenlyBodies, int>> outer, 
            ref Dictionary<Tuple<Position.HeavenlyBodies, int>, Tuple<Position.HeavenlyBodies, int>> inner)
        {
            Dictionary<Position.HeavenlyBodies, bool[]> access = new Dictionary<Position.HeavenlyBodies, bool[]>();
            access[Position.HeavenlyBodies.InnerDarkBramble_Hub] = new bool[] { false, false };
            access[Position.HeavenlyBodies.InnerDarkBramble_EscapePod] = new bool[] { false, false };
            access[Position.HeavenlyBodies.InnerDarkBramble_Nest] = new bool[] { false, false };
            access[Position.HeavenlyBodies.InnerDarkBramble_Feldspar] = new bool[] { false, false };
            access[Position.HeavenlyBodies.InnerDarkBramble_Gutter] = new bool[] { false, false };
            access[Position.HeavenlyBodies.InnerDarkBramble_Vessel] = new bool[] { false, false };
            access[Position.HeavenlyBodies.InnerDarkBramble_Maze] = new bool[] { false, false };
            access[Position.HeavenlyBodies.InnerDarkBramble_SmallNest] = new bool[] { false, false };

            if (_exitSpawnCount < 1)
            {
                spreadAccess(ref outer, ref inner, ref access, Position.HeavenlyBodies.InnerDarkBramble_Feldspar, true, false);
            }
            else
            {
                var linked = outer.First(x => x.Value.Item1 == Position.HeavenlyBodies.DarkBramble);
                spreadAccess(ref outer, ref inner, ref access, linked.Key.Item1, true, false);
            }

            foreach(var key in access.Keys)
            {
                if (access[key][0] && access[key][1])
                {
                    continue;
                }


                Helper.helper.Console.WriteLine("Failed to generate Dark Bramble, Retrying!");
                return false;
            }
            return true;
        }

        private static void spreadAccess(ref Dictionary<Tuple<Position.HeavenlyBodies, int>, Tuple<Position.HeavenlyBodies, int>> outer, 
            ref Dictionary<Tuple<Position.HeavenlyBodies, int>, Tuple<Position.HeavenlyBodies, int>> inner, 
            ref Dictionary<Position.HeavenlyBodies, bool[]>  access, Position.HeavenlyBodies next, bool entrance, bool exit)
        {
            bool hasChange = false;
            if (entrance && access[next][0] != true)
            {
                access[next][0] = true;
                hasChange = true;
            }
            if (exit && access[next][1] != true)
            {
                access[next][1] = true;
                hasChange = true;
            }

            if (!hasChange)
            {
                return;
            }

            foreach (var portal in outer.Where(x => x.Key.Item1 == next))
            {
                var linked = portal.Value;
                if (linked.Item2 >= 0)
                {
                    if (linked.Item1 == Position.HeavenlyBodies.DarkBramble)
                    {
                        access[next][1] = true;
                    }
                    else
                    {
                        spreadAccess(ref outer, ref inner, ref access, linked.Item1, access[next][0], access[next][1]);
                    }
                }
            }

            foreach (var portal in inner.Where(x => x.Key.Item1 == next))
            {
                var linked = portal.Value;
                if (linked.Item2 >= 0)
                {
                    spreadAccess(ref outer, ref inner, ref access, linked.Item1, access[next][0], access[next][1]);
                }
            }
        }


        private static void addExit(ref Dictionary<Tuple<Position.HeavenlyBodies, int>, Tuple<Position.HeavenlyBodies, int>> outerMapping)
        {
            var map = outerMapping;
            var outerOptions = new List<Tuple<Position.HeavenlyBodies, int>>(map.Keys);
            outerOptions.RemoveAll(x =>
                x.Item2 < 0
                || x.Item1 == Position.HeavenlyBodies.InnerDarkBramble_Vessel // Remove all options to the Vessel
                || map[x].Item1 == Position.HeavenlyBodies.DarkBramble); // Remove any options where we already have an Exit

            if (outerOptions.Count > 0)
            {
                var r = seeds.Next(outerOptions.Count);
                outerMapping[outerOptions[r]] = Tuple.Create(Position.HeavenlyBodies.DarkBramble, 0);
            }
            else
            {
                Helper.helper.Console.WriteLine("Failed to add Exit To Dark Bramble!");
            }
        }

        private static void addVessel(ref Dictionary<Tuple<Position.HeavenlyBodies, int>, Tuple<Position.HeavenlyBodies, int>> innerMapping)
        {
            var map = innerMapping;
            var innerOptions = new List<Tuple<Position.HeavenlyBodies, int>>(map.Keys);
            innerOptions.RemoveAll(x =>
                x.Item2 < 0
                || x.Item1 == Position.HeavenlyBodies.DarkBramble // Remove all options to the Entrance
                || map[x].Item1 == Position.HeavenlyBodies.InnerDarkBramble_Vessel); // Remove any options where we already have a Vessel

            if (innerOptions.Count > 0)
            {
                var r = seeds.Next(innerOptions.Count);
                innerMapping[innerOptions[r]] = Tuple.Create(Position.HeavenlyBodies.InnerDarkBramble_Vessel, 0);
            }
            else
            {
                Helper.helper.Console.WriteLine("Failed to add Vessel To Dark Bramble!");
            }
        }

        private static Tuple<Position.HeavenlyBodies, int> randomizeInnerPortal(ref List<Tuple<Position.HeavenlyBodies, int>> options, Tuple<Position.HeavenlyBodies, int> portal)
        {
            if (portal.Item2 >= 0)
            {
                options.RemoveAll(x => x.Item2 < 0 || x.Item1 == Position.HeavenlyBodies.InnerDarkBramble_Vessel); // Vessel is added at a different step
            }
            return randomizePortal(ref options);
        }

        private static Tuple<Position.HeavenlyBodies, int> randomizeOuterPortal(ref List<Tuple<Position.HeavenlyBodies, int>> options, Tuple<Position.HeavenlyBodies, int> portal)
        {
            if (portal.Item2 >= 0)
            {
                options.RemoveAll(x => x.Item2 < 0 || x.Item1 == Position.HeavenlyBodies.DarkBramble); // Exit is added at a different step
            }
            return randomizePortal(ref options);
        }

        private static Tuple<Position.HeavenlyBodies, int> randomizePortal(ref List<Tuple<Position.HeavenlyBodies, int>> options)
        {
            var r = seeds.Next(options.Count);
            return options[r];
        }

        private static void onBrambleWarp(FogWarpDetector.Name warpObject, bool isInnerPortal, Tuple<Position.HeavenlyBodies, int> portal)
        {
            if (type == RandomizerSeeds.Type.Use || type == RandomizerSeeds.Type.SeedlessUse)
            {
                var mapping = BramblePortals.mapping;
                bool isExit = false;
                bool isVessel = false;
                if (portal.Item2 > 0)
                {
                    if (isInnerPortal)
                    {
                        isVessel = mapping.Item2[portal].Item1 == Position.HeavenlyBodies.InnerDarkBramble_Vessel;
                    }
                    else
                    {
                        isExit = mapping.Item2[portal].Item1 == Position.HeavenlyBodies.DarkBramble;
                    }
                }


                var outerMapping = mapping.Item1;
                var innerMapping = mapping.Item2;
                if (isInnerPortal)
                {
                    var keys = new List<Tuple<Position.HeavenlyBodies, int>>(outerMapping.Keys);
                    innerMapping[portal] = randomizeInnerPortal(ref keys, portal);
                }
                else
                {
                    var keys = new List<Tuple<Position.HeavenlyBodies, int>>(innerMapping.Keys);
                    outerMapping[portal] = randomizeOuterPortal(ref keys, portal);
                }

                if (isVessel)
                {
                    addVessel(ref innerMapping);
                }
                if (isExit)
                {
                    addExit(ref outerMapping);
                }

                BramblePortals.mapping = Tuple.Create(outerMapping, innerMapping);
            }
        }
    }
}
