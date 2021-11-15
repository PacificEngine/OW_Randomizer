﻿using PacificEngine.OW_CommonResources;
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
            if (Time.time - _lastUpdate > 60f && (BramblePortalRandomizer.type == RandomizerSeeds.Type.Minute || BramblePortalRandomizer.type == RandomizerSeeds.Type.SeedlessMinute))
            {
                _isSet = false;
            }
            
            if (!_isSet)
            {
                updateValues();
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

        private static void updateValues()
        {
            if (!_isSet && BramblePortals.getOuterVolumes().Count > 0)
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

        private static void defaultValues()
        {         
            BramblePortals.remapOuterPortal(BramblePortals.getOuterVolumes(Position.HeavenlyBodies.InnerDarkBramble_Hub)[0].Item2, BramblePortals.getInnerVolumes(Position.HeavenlyBodies.DarkBramble)[0].Item2);
            BramblePortals.remapOuterPortal(BramblePortals.getOuterVolumes(Position.HeavenlyBodies.InnerDarkBramble_EscapePod)[0].Item2, BramblePortals.getInnerVolumes(Position.HeavenlyBodies.InnerDarkBramble_Hub)[2].Item2);
            BramblePortals.remapOuterPortal(BramblePortals.getOuterVolumes(Position.HeavenlyBodies.InnerDarkBramble_Nest)[0].Item2, BramblePortals.getInnerVolumes(Position.HeavenlyBodies.DarkBramble)[0].Item2);
            BramblePortals.remapOuterPortal(BramblePortals.getOuterVolumes(Position.HeavenlyBodies.InnerDarkBramble_Feldspar)[0].Item2, BramblePortals.getInnerVolumes(Position.HeavenlyBodies.DarkBramble)[0].Item2);
            BramblePortals.remapOuterPortal(BramblePortals.getOuterVolumes(Position.HeavenlyBodies.InnerDarkBramble_Gutter)[0].Item2, BramblePortals.getInnerVolumes(Position.HeavenlyBodies.DarkBramble)[0].Item2);
            BramblePortals.remapOuterPortal(BramblePortals.getOuterVolumes(Position.HeavenlyBodies.InnerDarkBramble_Vessel)[0].Item2, BramblePortals.getInnerVolumes(Position.HeavenlyBodies.DarkBramble)[0].Item2);
            BramblePortals.remapOuterPortal(BramblePortals.getOuterVolumes(Position.HeavenlyBodies.InnerDarkBramble_Maze)[0].Item2, BramblePortals.getInnerVolumes(Position.HeavenlyBodies.InnerDarkBramble_Hub)[3].Item2);
            BramblePortals.remapOuterPortal(BramblePortals.getOuterVolumes(Position.HeavenlyBodies.InnerDarkBramble_SmallNest)[0].Item2, BramblePortals.getInnerVolumes(Position.HeavenlyBodies.InnerDarkBramble_Hub)[1].Item2);

            BramblePortals.remapInnerPortal(BramblePortals.getOuterVolumes(Position.HeavenlyBodies.InnerDarkBramble_Feldspar)[0].Item2, BramblePortals.getInnerVolumes(Position.HeavenlyBodies.InnerDarkBramble_Maze)[0].Item2);
            BramblePortals.remapInnerPortal(BramblePortals.getOuterVolumes(Position.HeavenlyBodies.InnerDarkBramble_Gutter)[0].Item2, BramblePortals.getInnerVolumes(Position.HeavenlyBodies.InnerDarkBramble_Maze)[1].Item2);
            BramblePortals.remapInnerPortal(BramblePortals.getOuterVolumes(Position.HeavenlyBodies.InnerDarkBramble_Feldspar)[0].Item2, BramblePortals.getInnerVolumes(Position.HeavenlyBodies.InnerDarkBramble_Maze)[2].Item2);
            BramblePortals.remapInnerPortal(BramblePortals.getOuterVolumes(Position.HeavenlyBodies.InnerDarkBramble_Gutter)[0].Item2, BramblePortals.getInnerVolumes(Position.HeavenlyBodies.InnerDarkBramble_Maze)[3].Item2);
            BramblePortals.remapInnerPortal(BramblePortals.getOuterVolumes(Position.HeavenlyBodies.InnerDarkBramble_Gutter)[0].Item2, BramblePortals.getInnerVolumes(Position.HeavenlyBodies.InnerDarkBramble_Maze)[4].Item2);
            BramblePortals.remapInnerPortal(BramblePortals.getOuterVolumes(Position.HeavenlyBodies.InnerDarkBramble_Gutter)[0].Item2, BramblePortals.getInnerVolumes(Position.HeavenlyBodies.InnerDarkBramble_Maze)[5].Item2);
            BramblePortals.remapInnerPortal(BramblePortals.getOuterVolumes(Position.HeavenlyBodies.InnerDarkBramble_Gutter)[0].Item2, BramblePortals.getInnerVolumes(Position.HeavenlyBodies.InnerDarkBramble_Maze)[6].Item2);
            BramblePortals.remapInnerPortal(BramblePortals.getOuterVolumes(Position.HeavenlyBodies.InnerDarkBramble_Feldspar)[0].Item2, BramblePortals.getInnerVolumes(Position.HeavenlyBodies.InnerDarkBramble_Maze)[7].Item2);
            BramblePortals.remapInnerPortal(BramblePortals.getOuterVolumes(Position.HeavenlyBodies.InnerDarkBramble_Gutter)[0].Item2, BramblePortals.getInnerVolumes(Position.HeavenlyBodies.InnerDarkBramble_Maze)[8].Item2);

            BramblePortals.remapInnerPortal(BramblePortals.getOuterVolumes(Position.HeavenlyBodies.InnerDarkBramble_Nest)[0].Item2, BramblePortals.getInnerVolumes(Position.HeavenlyBodies.InnerDarkBramble_EscapePod)[0].Item2);
            BramblePortals.remapInnerPortal(BramblePortals.getOuterVolumes(Position.HeavenlyBodies.InnerDarkBramble_Vessel)[0].Item2, BramblePortals.getInnerVolumes(Position.HeavenlyBodies.InnerDarkBramble_EscapePod)[1].Item2);

            BramblePortals.remapInnerPortal(BramblePortals.getOuterVolumes(Position.HeavenlyBodies.InnerDarkBramble_Vessel)[0].Item2, BramblePortals.getInnerVolumes(Position.HeavenlyBodies.InnerDarkBramble_Nest)[0].Item2);
            BramblePortals.remapInnerPortal(BramblePortals.getOuterVolumes(Position.HeavenlyBodies.InnerDarkBramble_Gutter)[0].Item2, BramblePortals.getInnerVolumes(Position.HeavenlyBodies.InnerDarkBramble_Nest)[1].Item2);
            BramblePortals.remapInnerPortal(BramblePortals.getOuterVolumes(Position.HeavenlyBodies.InnerDarkBramble_Gutter)[0].Item2, BramblePortals.getInnerVolumes(Position.HeavenlyBodies.InnerDarkBramble_Nest)[2].Item2);
            BramblePortals.remapInnerPortal(BramblePortals.getOuterVolumes(Position.HeavenlyBodies.InnerDarkBramble_Gutter)[0].Item2, BramblePortals.getInnerVolumes(Position.HeavenlyBodies.InnerDarkBramble_Nest)[3].Item2);

            BramblePortals.remapInnerPortal(BramblePortals.getOuterVolumes(Position.HeavenlyBodies.InnerDarkBramble_Gutter)[0].Item2, BramblePortals.getInnerVolumes(Position.HeavenlyBodies.InnerDarkBramble_Gutter)[0].Item2);
            BramblePortals.remapInnerPortal(BramblePortals.getSecretVolume()[0].Item2, BramblePortals.getInnerVolumes(Position.HeavenlyBodies.InnerDarkBramble_Gutter)[1].Item2);

            BramblePortals.remapInnerPortal(BramblePortals.getOuterVolumes(Position.HeavenlyBodies.InnerDarkBramble_Nest)[0].Item2, BramblePortals.getInnerVolumes(Position.HeavenlyBodies.InnerDarkBramble_Hub)[0].Item2);
            BramblePortals.remapInnerPortal(BramblePortals.getOuterVolumes(Position.HeavenlyBodies.InnerDarkBramble_SmallNest)[0].Item2, BramblePortals.getInnerVolumes(Position.HeavenlyBodies.InnerDarkBramble_Hub)[1].Item2);
            BramblePortals.remapInnerPortal(BramblePortals.getOuterVolumes(Position.HeavenlyBodies.InnerDarkBramble_EscapePod)[0].Item2, BramblePortals.getInnerVolumes(Position.HeavenlyBodies.InnerDarkBramble_Hub)[2].Item2);
            BramblePortals.remapInnerPortal(BramblePortals.getOuterVolumes(Position.HeavenlyBodies.InnerDarkBramble_Maze)[0].Item2, BramblePortals.getInnerVolumes(Position.HeavenlyBodies.InnerDarkBramble_Hub)[3].Item2);

            BramblePortals.remapInnerPortal(BramblePortals.getOuterVolumes(Position.HeavenlyBodies.InnerDarkBramble_Feldspar)[0].Item2, BramblePortals.getInnerVolumes(Position.HeavenlyBodies.InnerDarkBramble_Feldspar)[0].Item2);

            BramblePortals.remapInnerPortal(BramblePortals.getOuterVolumes(Position.HeavenlyBodies.InnerDarkBramble_Hub)[0].Item2, BramblePortals.getInnerVolumes(Position.HeavenlyBodies.DarkBramble)[0].Item2);
            BramblePortals.remapInnerPortal(BramblePortals.getOuterVolumes(Position.HeavenlyBodies.InnerDarkBramble_Feldspar)[0].Item2, BramblePortals.getInnerVolumes(Position.HeavenlyBodies.TimberHearth)[0].Item2);
        }

        private static void randomizeValues()
        {
            int maxRetry = 0;
            do
            {
                maxRetry++;

                foreach (var portal in BramblePortals.getOuterVolumes())
                {
                    randomizePortal(portal.Item2);
                }

                foreach (var portal in BramblePortals.getInnerVolumes())
                {
                    randomizePortal(portal.Item2);
                }


                addExit();
                addVessel();
            } while (!verifyAccessible() && maxRetry < 500);
        }

        private static bool verifyAccessible()
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

            var linked = BramblePortals.getInnerVolumes(Position.HeavenlyBodies.DarkBramble)[0].Item2.GetLinkedFogWarpVolume();
            spreadAccess(ref access, BramblePortals.findBody(linked), true, false);

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

        private static void spreadAccess(ref Dictionary<Position.HeavenlyBodies, bool[]> access, Position.HeavenlyBodies next, bool entrance, bool exit)
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

            foreach (var portal in BramblePortals.getOuterVolumes(next))
            {
                var linked = portal.Item2.GetLinkedFogWarpVolume();
                if (!linked.IsProbeOnly())
                {
                    if (portal.Item2.GetLinkedFogWarpVolume() == BramblePortals.getInnerVolumes(Position.HeavenlyBodies.DarkBramble)[0].Item2)
                    {
                        access[next][1] = true;
                    }
                    else
                    {
                        spreadAccess(ref access, BramblePortals.findBody(linked), access[next][0], access[next][1]);
                    }
                }
            }

            foreach (var portal in BramblePortals.getInnerVolumes(next))
            {
                var linked = portal.Item2.GetLinkedFogWarpVolume();
                if (!linked.IsProbeOnly())
                {
                    spreadAccess(ref access, BramblePortals.findBody(linked), access[next][0], access[next][1]);
                }
            }
        }


        private static void addExit()
        {
            var outerOptions = BramblePortals.getOuterVolumes();
            outerOptions.RemoveAll(x =>
                x.Item2.IsProbeOnly()
                || x.Item2 == BramblePortals.getOuterVolumes(Position.HeavenlyBodies.InnerDarkBramble_Vessel)[0].Item2 // Remove all options to the Vessel
                || x.Item2?.GetLinkedFogWarpVolume() == BramblePortals.getInnerVolumes(Position.HeavenlyBodies.DarkBramble)[0].Item2); // Remove any options where we already have an Exit

            if (outerOptions.Count > 0)
            {
                var r = seeds.Next(outerOptions.Count);
                BramblePortals.remapOuterPortal(outerOptions[r].Item2, BramblePortals.getInnerVolumes(Position.HeavenlyBodies.DarkBramble)[0].Item2);
            }
            else
            {
                Helper.helper.Console.WriteLine("Failed to add Exit To Dark Bramble!");
            }
        }

        private static void addVessel()
        {
            var innerOptions = BramblePortals.getInnerVolumes();
            innerOptions.RemoveAll(x =>
                x.Item2.IsProbeOnly()
                || x.Item2 == BramblePortals.getInnerVolumes(Position.HeavenlyBodies.DarkBramble)[0].Item2 // Remove all options to the outside
                || x.Item2?.GetLinkedFogWarpVolume() == BramblePortals.getOuterVolumes(Position.HeavenlyBodies.InnerDarkBramble_Vessel)[0].Item2); // Remove any options where we already have a Vessel

            if (innerOptions.Count > 0)
            {
                var r = seeds.Next(innerOptions.Count);
                BramblePortals.remapInnerPortal(BramblePortals.getOuterVolumes(Position.HeavenlyBodies.InnerDarkBramble_Vessel)[0].Item2, innerOptions[r].Item2);
            }
            else
            {
                Helper.helper.Console.WriteLine("Failed to add Vessel To Dark Bramble!");
            }
        }

        private static void randomizePortal(FogWarpVolume portal)
        {
            if (portal is OuterFogWarpVolume)
            {
                var innerOptions = BramblePortals.getInnerVolumes();
                innerOptions.RemoveAll(x => x.Item2.IsProbeOnly() 
                    || x.Item2 == BramblePortals.getInnerVolumes(Position.HeavenlyBodies.DarkBramble)[0].Item2); // Exit is added at a different step
                var r = seeds.Next(innerOptions.Count);
                BramblePortals.remapOuterPortal(portal as OuterFogWarpVolume, innerOptions[r].Item2);
            }
            else if (portal is InnerFogWarpVolume)
            {
                var outerOptions = BramblePortals.getOuterVolumes();
                if (portal.IsProbeOnly())
                {
                    var secret = BramblePortals.getSecretVolume()[0];
                    outerOptions.Add(Tuple.Create(secret.Item1, secret.Item2 as OuterFogWarpVolume));
                }
                else
                {
                    outerOptions.RemoveAll(x => x.Item2.IsProbeOnly()
                        || x.Item2 == BramblePortals.getOuterVolumes(Position.HeavenlyBodies.InnerDarkBramble_Vessel)[0].Item2); // Vessel is added at a different step
                }
                var r = seeds.Next(outerOptions.Count);
                BramblePortals.remapInnerPortal(outerOptions[r].Item2, portal as InnerFogWarpVolume);
            }
        }

        private static void onBrambleWarp(FogWarpDetector.Name warpObject, Position.HeavenlyBodies portalParent, FogWarpVolume portal)
        {
            if (type == RandomizerSeeds.Type.Use || type == RandomizerSeeds.Type.SeedlessUse)
            {
                bool isExit = false;
                bool isVessel = false;
                if (!portal.IsProbeOnly())
                {
                    if (portal is InnerFogWarpVolume)
                    {
                        isVessel = (portal as InnerFogWarpVolume).GetLinkedFogWarpVolume() == BramblePortals.getOuterVolumes(Position.HeavenlyBodies.InnerDarkBramble_Vessel)[0].Item2;
                    }
                    if (portal is OuterFogWarpVolume)
                    {
                        isExit = (portal as OuterFogWarpVolume).GetLinkedFogWarpVolume() == BramblePortals.getInnerVolumes(Position.HeavenlyBodies.DarkBramble)[0].Item2;
                    }
                }

                randomizePortal(portal);

                if (isVessel)
                {
                    addVessel();
                }
                if (isExit)
                {
                    addExit();
                }
            }
        }
    }
}