using OWML.Common;
using OWML.ModHelper;
using OWML.ModHelper.Events;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using PacificEngine.OW_CommonResources;
using PacificEngine.OW_CommonResources.Game.State;
using PacificEngine.OW_CommonResources.Game.Display;
using PacificEngine.OW_CommonResources.Game.Config;

namespace PacificEngine.OW_Randomizer
{
    public class MainClass : ModBehaviour
    {
        private const string verison = "0.6.0";

        /*  TH_ZERO_G_CAVE_R1(TH_ZERO_G_CAVE) 
         *  SCIENTIST_1(True)
         *  CrashedModelRocket(True)	
         *  LandedModelRocket(False)
         *  CrashedModelRocket(True)
         *  LandedModelRocket(False)
         *  PorphySaysHi(True)
         *  MayorSaysHi(True)
         *  Tree(True)
         *  SayHiToEsker(True)
         * FACT: TM_ESKER_R1(TM_ESKER)
         * TalkedToGneiss(True)
         * SaidHiToSpinel(True)
         * BeginHideAndSeek(True)
         * FoundKidOne(True)
         * LastKidToFind(True)
         * Arkose_loves_ghost_matter(True)
         * HasTalkedtoMoraine(True)
         * CoachSaysHi_1(True)
         * TalkedToTuff(True)
         * ScaredMinerSecondary(True)
         *  HAS_USED_JETPACK(True)
         *  TH_ZERO_G_CAVE_X1(TH_ZERO_G_CAVE) 
         *  PostZeroG(True)
         *  TH_ZERO_G_CAVE_X2(TH_ZERO_G_CAVE)
         *  TooSick(True)
         *  Condition: TuffTooSick(True)
         *  TuffTooSick(True)
         *   EndHideAndSeek(True)
         *   LastKidToFind(True)
         *    FriendSaysHi(True)
         *    TalkedToHornfels(True)
         *    LAUNCH_CODES_GIVEN(True)
         *     SCIENTIST_3(True)
         *     
         *     
         *     PREFLIGHT_CHECKLIST_UNLOCKED(True)
         *     : SUIT_BOOSTER_FIRED(True)
         *     TH_VILLAGE_X2(TH_VILLAGE) -
         *      HIDE_TEMPLE_BASEMENT_ENTRIES(True)
         *      FriendSaysHiAgain(True)
         *      HornfelsStatueEyes(True)
         *      TalkedToWeirdKid(True)
         *       HAS_USED_PREFLIGHT_CHECKLIST(True)
         *       HAS_USED_SHIPLOG(True)
         */

        private bool isEnabled = true;

        void Start()
        {
            if (isEnabled)
            {
                ModHelper.Events.Player.OnPlayerAwake += (player) => onAwake();

                EyeCoordinateRandomizer.instance.Start();
                BramblePortalRandomizer.instance.Start();
                WarpPadRandomizer.instance.Start();
                PlanetRandomizer.instance.Start();

                ModHelper.Console.WriteLine("Randomizer: ready!");
            }
        }

        void Destory()
        {
            EyeCoordinateRandomizer.instance.Destroy();
            BramblePortalRandomizer.instance.Destroy();
            WarpPadRandomizer.instance.Destroy();
            PlanetRandomizer.instance.Destroy();

            ModHelper.Console.WriteLine("Randomizer: clean up!");
        }

        private Tuple<string, int> getSeed(IModConfig config)
        {
            var seed = ConfigHelper.getConfigOrDefault<String>(config, "Seed", "");
            int seedValue = 0;
            if (seed.Length < 1)
            {
                seedValue = new System.Random().Next();
                seed = seedValue.ToString();
            }
            else if (!int.TryParse(seed, out seedValue))
            {
                seedValue = seed.GetHashCode();
            }
            return Tuple.Create(seed, seedValue);
        }

        private RandomizerSeeds.Type getType(IModConfig config, String id)
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
            if ("Upon Use".Equals(type)
                || "On Use".Equals(type))
            {
                return RandomizerSeeds.Type.Use;
            }
            if ("Upon Use & Minute".Equals(type)
                || "Minute + Use".Equals(type))
            {
                return RandomizerSeeds.Type.MinuteUse;
            }
            if ("Full Regeneration Upon Use".Equals(type)
                || "Full On Use".Equals(type))
            {
                return RandomizerSeeds.Type.FullUse;
            }
            if ("Seedless".Equals(type))
            {
                return RandomizerSeeds.Type.Seedless;
            }
            if ("Seedless Minute".Equals(type))
            {
                return RandomizerSeeds.Type.SeedlessMinute;
            }
            if ("Seedless Upon Use".Equals(type)
                || "Seedless On Use".Equals(type))
            {
                return RandomizerSeeds.Type.SeedlessUse;
            }
            if ("Seedless Upon Use & Minute".Equals(type)
                || "Seedless Minute + Use".Equals(type))
            {
                return RandomizerSeeds.Type.SeedlessMinuteUse;
            }
            if ("Seedless Full Regeneration Upon Use".Equals(type)
                || "Seedless Full On Use".Equals(type))
            {
                return RandomizerSeeds.Type.SeedlessFullUse;
            }
            return RandomizerSeeds.Type.None;
        }

        public override void Configure(IModConfig config)
        {
            isEnabled = config.Enabled;
            
            if (isEnabled)
            {
                var seed = getSeed(config);
                var seedValue = seed.Item2;
                ModHelper.Console.WriteLine("Seed v" + verison + ": " + seed.Item1);

                EyeCoordinateRandomizer.instance.updateSeed(seedValue++, getType(config, "EyeCoordinates"));
                BramblePortalRandomizer.instance.updateSeed(seedValue++, getType(config, "BrambleWarp"), int.Parse(ConfigHelper.getConfigOrDefault<string>(config, "BrambleExit", "2")), int.Parse(ConfigHelper.getConfigOrDefault<string>(config, "BrambleVessel", "1")));
                WarpPadRandomizer.instance.updateSeed(seedValue++, getType(config, "PadWarp"), int.Parse(ConfigHelper.getConfigOrDefault<string>(config, "PadsToAshTwinProject", "1")), ConfigHelper.getConfigOrDefault<bool>(config, "PadDuplication", false), !ConfigHelper.getConfigOrDefault<bool>(config, "PadMirroring", true), ConfigHelper.getConfigOrDefault<bool>(config, "PadChaos", false));
                PlanetRandomizer.instance.updateSeed(seedValue++, getType(config, "Planets"));

                DisplayConsole.getConsole(ConsoleLocation.BottomRight).setElement("PacificEngine.OW_Randomizer.MainClass.Seed", "Seed v" + verison + ": " + seed.Item1, 100f);
            }
            else
            {
                EyeCoordinateRandomizer.instance.updateSeed(0, RandomizerSeeds.Type.None);
                BramblePortalRandomizer.instance.updateSeed(0, RandomizerSeeds.Type.None, 5, 1);
                WarpPadRandomizer.instance.updateSeed(0, RandomizerSeeds.Type.None, 1, false, false, false);
                PlanetRandomizer.instance.updateSeed(0, RandomizerSeeds.Type.None);

                DisplayConsole.getConsole(ConsoleLocation.BottomRight).setElement("PacificEngine.OW_Randomizer.MainClass.Seed", "", 0f);
            }
            ModHelper.Console.WriteLine("Randomizer: Configured!");
        }

        void OnGUI()
        {
        }

        void onAwake()
        {
            EyeCoordinateRandomizer.instance.Awake();
            BramblePortalRandomizer.instance.Awake();
            WarpPadRandomizer.instance.Awake();
            PlanetRandomizer.instance.Awake();

            ModHelper.Console.WriteLine("Randomizer: Player Awakes");
        }

        void Update()
        {
            EyeCoordinateRandomizer.instance.Update();
            BramblePortalRandomizer.instance.Update();
            WarpPadRandomizer.instance.Update();
            PlanetRandomizer.instance.Update();
        }
    }
}
