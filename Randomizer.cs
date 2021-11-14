using OWML.Common;
using OWML.ModHelper;
using OWML.ModHelper.Events;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using PacificEngine.OW_CommonResources;
using PacificEngine.OW_CommonResources.Game.State;

namespace PacificEngine.OW_Randomizer
{
    /*	
	InnerFogWarpVolume._linkedOuterWarpVolume
	InnerFogWarpVolume._linkedOuterWarpName
	
	InnerFogWarpVolume._containerWarpVolume
	
	OuterFogWarpVolume._linkedInnerWarpVolume
	OuterFogWarpVolume._name
	
	InnerFogWarpVolume._senderWarps
	*/

    /**
     * ShipLogFact
     * 
     */

    public class MainClass : ModBehaviour
    {
        private bool isEnabled = true;
        private float _lastUpdate;

        void Start()
        {
            if (isEnabled)
            {
                ModHelper.Events.Player.OnPlayerAwake += (player) => onAwake();
                ModHelper.Console.WriteLine("Randomizer: ready!");
            }
        }

        void Destory()
        {
            ModHelper.Console.WriteLine("Randomizer: clean up!");
        }

        private int getSeed(IModConfig config)
        {
            var seed = Config.getConfigOrDefault<String>(config, "Seed", "");
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
            var type = Config.getConfigOrDefault<String>(config, id, "Off");
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
            if ("Seedless".Equals(type))
            {
                return RandomizerSeeds.Type.Seedless;
            }
            if ("Seedless Minute".Equals(type))
            {
                return RandomizerSeeds.Type.SeedlessMinute;
            }
            return null;
        }

        public override void Configure(IModConfig config)
        {
            isEnabled = config.Enabled;

            var seed = getSeed(config);
            EyeCoordinateRandomizer.updateSeed(seed++, getType(config, "EyeCoordinates"));

            ModHelper.Console.WriteLine("Randomizer: Configured!");
        }

        void OnGUI()
        {
        }

        void onAwake()
        {
            _lastUpdate = Time.time;

            EyeCoordinateRandomizer.reset();

            ModHelper.Console.WriteLine("Randomizer: Player Awakes");
        }

        void Update()
        {
            if (isEnabled)
            {
                if (Time.time - _lastUpdate > 60f)
                {
                    _lastUpdate = Time.time;
                    if (EyeCoordinateRandomizer.type == RandomizerSeeds.Type.Minute || EyeCoordinateRandomizer.type == RandomizerSeeds.Type.SeedlessMinute)
                    {
                        EyeCoordinateRandomizer.Update();
                    }
                }
            }
        }
    }
}
