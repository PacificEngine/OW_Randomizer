using PacificEngine.OW_CommonResources.Game.Config;
using PacificEngine.OW_CommonResources.Game.Resource;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PacificEngine.OW_Randomizer
{
    public class PlanetRandomizer : AbstractRandomizer
    {
        public static PlanetRandomizer instance { get; } = new PlanetRandomizer();

        public new void updateSeed(int seed, RandomizerSeeds.Type? type)
        {
            base.updateSeed(seed, type);
        }

        protected override void defaultValues()
        {
        }

        protected override void randomizeValues(int cycles)
        {
            
        }
    }
}