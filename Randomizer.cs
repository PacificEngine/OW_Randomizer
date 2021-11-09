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
     * HologramProjector (OrbitalCannonHologramProjector):Hologram_EyeCoordinates (UnityEngine.GameObject)	PacificEngine's Common Resources	
Hologram_EyeCoordinates (UnityEngine.Transform)	PacificEngine's Common Resources	
Hologram_EyeCoordinates (TimedHologram)	PacificEngine's Common Resources	
childHologram_EyeCoordinates (UnityEngine.Transform)	PacificEngine's Common Resources	
childHologram_EyeCoordinates (TimedHologram)	PacificEngine's Common Resources	
childEffects_NOM_EyeCoordinates (UnityEngine.Transform)	PacificEngine's Common Resources	
childEffects_NOM_EyeCoordinates (UnityEngine.MeshFilter)	PacificEngine's Common Resources	
childEffects_NOM_EyeCoordinates (UnityEngine.MeshRenderer)	PacificEngine's Common Resources	
childAmbientLight_EyeHologram (UnityEngine.Transform)	PacificEngine's Common Resources	
childAmbientLight_EyeHologram (UnityEngine.Light)	PacificEngine's Common Resources	
childEffects_NOM_HologramDrips (UnityEngine.Transform)	PacificEngine's Common Resources	
childEffects_NOM_HologramDrips (UnityEngine.ParticleSystem)	PacificEngine's Common Resources	
childEffects_NOM_HologramDrips (UnityEngine.ParticleSystemRenderer)	PacificEngine's Common Resources
     */

    public class MainClass : ModBehaviour
    {
        bool isEnabled = true;

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

        public override void Configure(IModConfig config)
        {
            isEnabled = config.Enabled;

            var seed = getSeed(config);
            EyeCoordinates.randomizeCoordinates(new System.Random(seed++));

            ModHelper.Console.WriteLine("Randomizer: Configured!");
        }

        void OnGUI()
        {
        }

        void onAwake()
        {
            ModHelper.Console.WriteLine("Randomizer: Player Awakes");
        }

        void Update()
        {
            if (isEnabled)
            {
            }
        }
    }
}
