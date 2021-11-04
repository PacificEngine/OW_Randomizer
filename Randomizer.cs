using OWML.Common;
using OWML.ModHelper;
using OWML.ModHelper.Events;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using PacificEngine.OW_CommonResources;

namespace PacificEngine.OW_Randomizer
{
	/*
	NomaiCoordinateInterface._coordinateX (1, 5, 4)
	NomaiCoordinateInterface._coordinateY (3, 0, 1, 4)
	NomaiCoordinateInterface._coordinateZ (1, 2, 3, 0, 5, 4) // 0 Is top left, and it continues around clockwise.
	
	KeyInfoPromptController._eyeCoordinatesPrompt
	
	InnerFogWarpVolume._linkedOuterWarpVolume
	InnerFogWarpVolume._linkedOuterWarpName
	
	InnerFogWarpVolume._containerWarpVolume
	
	OuterFogWarpVolume._linkedInnerWarpVolume
	OuterFogWarpVolume._name
	
	InnerFogWarpVolume._senderWarps
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

        public override void Configure(IModConfig config)
        {
            isEnabled = config.Enabled;
            EyeCoordinates.randomizeCoordinates();

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
