using OWML.Common;
using OWML.ModHelper;
using OWML.ModHelper.Events;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using PacificEngine.OW_CommonResources;
using PacificEngine.OW_CommonResources.Game.State;
using PacificEngine.OW_CommonResources.Config;

namespace PacificEngine.OW_Randomizer
{
    public class MainClass : ModBehaviour
    {
        private bool isEnabled = true;

        void Start()
        {
            if (isEnabled)
            {
                ModHelper.Events.Player.OnPlayerAwake += (player) => onAwake();

                EyeCoordinateRandomizer.Start();
                BramblePortalRandomizer.Start();

                ModHelper.Console.WriteLine("Randomizer: ready!");
            }
        }

        void Destory()
        {
            ModHelper.Console.WriteLine("Randomizer: clean up!");
        }

        private int getSeed(IModConfig config)
        {
            var seed = ConfigHelper.getConfigOrDefault<String>(config, "Seed", "");
            int seedValue = 0;
            if (seed.Length < 1)
            {
                seedValue = new System.Random().Next();
            }
            else if (!int.TryParse(seed, out seedValue))
            {
                seedValue = seed.GetHashCode();
            }
            return seedValue;
        }

        private RandomizerSeeds.Type? getType(IModConfig config, String id)
        {
            var type = ConfigHelper.getConfigOrDefault<String>(config, id, "Off");
            if ("Seed".Equals(type))
            {
                return RandomizerSeeds.Type.Seed;
            }
            if ("Profile".Equals(type))
            {
                return RandomizerSeeds.Type.Profile;
            }
            if ("Death".Equals(type))
            {
                return RandomizerSeeds.Type.Death;
            }
            if ("Minute".Equals(type))
            {
                return RandomizerSeeds.Type.Minute;
            }
            if ("Upon Use".Equals(type))
            {
                return RandomizerSeeds.Type.Use;
            }
            if ("Seedless".Equals(type))
            {
                return RandomizerSeeds.Type.Seedless;
            }
            if ("Seedless Minute".Equals(type))
            {
                return RandomizerSeeds.Type.SeedlessMinute;
            }
            if ("Seedless Upon Use".Equals(type))
            {
                return RandomizerSeeds.Type.SeedlessUse;
            }
            return null;
        }

        public override void Configure(IModConfig config)
        {
            isEnabled = config.Enabled;

            if (isEnabled)
            {
                var seed = getSeed(config);
                ModHelper.Console.WriteLine("Using Seed " + seed);
                EyeCoordinateRandomizer.updateSeed(seed++, getType(config, "EyeCoordinates"));
                BramblePortalRandomizer.updateSeed(seed++, getType(config, "BrambleWarp"), int.Parse(ConfigHelper.getConfigOrDefault<String>(config, "BrambleExit", "2")), int.Parse(ConfigHelper.getConfigOrDefault<String>(config, "BrambleVessel", "1")));
            }
            else
            {
                EyeCoordinateRandomizer.updateSeed(0, null);
                BramblePortalRandomizer.updateSeed(0, null, 5, 1);
            }
            ModHelper.Console.WriteLine("Randomizer: Configured!");
        }

        void OnGUI()
        {
        }

        void onAwake()
        {
            EyeCoordinateRandomizer.Reset();
            BramblePortalRandomizer.Reset();

            ModHelper.Console.WriteLine("Randomizer: Player Awakes");
        }

        void Update()
        {
            EyeCoordinateRandomizer.Update();
            BramblePortalRandomizer.Update();
        }
    }
}
