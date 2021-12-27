using PacificEngine.OW_CommonResources.Game;
using PacificEngine.OW_CommonResources.Game.Config;
using PacificEngine.OW_CommonResources.Game.Resource;
using PacificEngine.OW_CommonResources.Game.State;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace PacificEngine.OW_Randomizer
{
    public class WarpPadRandomizer : AbstractRandomizer
    {
        public static WarpPadRandomizer instance { get; } = new WarpPadRandomizer();
        private static Tuple<HeavenlyBody, int> ashTwinProject = Tuple.Create(HeavenlyBodies.AshTwin, -1);

        private static int _ashTwinProjectCount = 1;
        private static bool _allowDuplicates = false;
        private static bool _includeRecievers = false;
        private static bool _allowTeleportToSamePadType = false;

        public override void Start()
        {
            WarpPad.onPadWarp += onPadWarp;
        }

        public void updateSeed(int seed, RandomizerSeeds.Type type, int ashTwinProjectCount, bool allowDuplicateWarps, bool allowRecieverRandomized, bool allowTeleporationToSamePadType)
        {
            _ashTwinProjectCount = ashTwinProjectCount;
            _allowDuplicates = allowDuplicateWarps;
            _includeRecievers = allowRecieverRandomized;
            _allowTeleportToSamePadType = allowTeleporationToSamePadType;

            updateSeed(seed, type);
        }

        protected override void defaultValues()
        {
            WarpPad.mapping = WarpPad.defaultMapping;
        }

        protected override void randomizeValues()
        {
            var mapping = WarpPad.defaultMapping;
            var allPads = new List<Tuple<HeavenlyBody, int>>(mapping.Keys);
            allPads.AddRange(mapping.Values);
            WarpPad.mapping = getRandomizeValues(ref allPads);
        }

        private void addAshTwinProjectWarp(ref Dictionary<Tuple<HeavenlyBody, int>, Tuple<HeavenlyBody, int>> mapping)
        {
            var options = mapping.ToList().FindAll(x => !(x.Key.Item1 == ashTwinProject.Item1 && x.Key.Item2 == ashTwinProject.Item2) && !(x.Value.Item1 == ashTwinProject.Item1 && x.Value.Item2 == ashTwinProject.Item2));
            var r = seeds.Next(options.Count);
            mapping[options[r].Key] = ashTwinProject; 
        }

        private Dictionary<Tuple<HeavenlyBody, int>, Tuple<HeavenlyBody, int>>
            getRandomizeValues(ref List<Tuple<HeavenlyBody, int>> allPads)
        {
            var newMapping = new Dictionary<Tuple<HeavenlyBody, int>, Tuple<HeavenlyBody, int>>();
            var allOptions = new List<Tuple<HeavenlyBody, int>>(allPads.Distinct());
            var allSenders = allOptions.FindAll(x => _includeRecievers || x.Item2 >= 0);

            int maxRetry = 0;
            do
            {
                var remainingOptions = allOptions.FindAll(x => !(x.Item1 == ashTwinProject.Item1 && x.Item2 == ashTwinProject.Item2));
                int ashTwinCount = 0;
                maxRetry++;
                newMapping.Clear();

                foreach (var sender in allSenders)
                {
                    var linked = randomizePlatform(sender, ref remainingOptions);
                    if (linked == null)
                    {
                        linked = ashTwinProject;
                        ashTwinCount++;
                    }
                    if (!_allowDuplicates)
                    {
                        remainingOptions.Remove(linked);
                    }

                    newMapping[sender] = linked;
                }

                for (; ashTwinCount < _ashTwinProjectCount; ashTwinCount++)
                {
                    addAshTwinProjectWarp(ref newMapping);
                }
            } while (false && maxRetry < 500);

            return newMapping;
        }


        private Tuple<HeavenlyBody, int> randomizePlatform(Tuple<HeavenlyBody, int> sender, ref List<Tuple<HeavenlyBody, int>> options)
        {
            var option = options.FindAll(x => !(x.Item1 == sender.Item1 && x.Item2 == sender.Item2) && (_allowTeleportToSamePadType || ((x.Item2 < 0) != (sender.Item2 < 0))));
            if (option.Count <= 0)
            {
                return null;
            }
            var r = seeds.Next(option.Count);
            return option[r];
        }

        private void onPadWarp(OWRigidbody warpObject, Tuple<HeavenlyBody, int> sender, Tuple<HeavenlyBody, int> reciever)
        {
            if (type == RandomizerSeeds.Type.FullUse || type == RandomizerSeeds.Type.SeedlessFullUse)
            {
                randomizeValues();
            }
            else if (type == RandomizerSeeds.Type.Use || type == RandomizerSeeds.Type.SeedlessUse
                || type == RandomizerSeeds.Type.MinuteUse || type == RandomizerSeeds.Type.SeedlessMinuteUse)
            {
                if (_includeRecievers || sender.Item2 >= 0)
                {
                    bool isAshTwinProject = reciever.Item1 == HeavenlyBodies.AshTwin && reciever.Item2 == -1;

                    var mapping = WarpPad.defaultMapping;
                    var allPads = new List<Tuple<HeavenlyBody, int>>(mapping.Keys);
                    allPads.AddRange(mapping.Values);
                    var allOptions = new List<Tuple<HeavenlyBody, int>>(allPads.Distinct()).FindAll(x => !(x.Item1 == ashTwinProject.Item1 && x.Item2 == ashTwinProject.Item2));

                    var linked = randomizePlatform(sender, ref allOptions);
                    if (linked == null)
                    {
                        linked = ashTwinProject;
                    }
                    WarpPad.remapPad(sender, linked);

                    if (isAshTwinProject)
                    {
                        var map = WarpPad.mapping;
                        addAshTwinProjectWarp(ref map);
                        WarpPad.mapping = map;
                    }
                }
            }
        }
    }
}
