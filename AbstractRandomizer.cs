using PacificEngine.OW_CommonResources.Game.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace PacificEngine.OW_Randomizer
{
    public abstract class AbstractRandomizer
    {
        protected static int minuteCycles { get; private set; } = 0;
        protected static int useCycles { get; set; } = 0;
        protected static int fullUseCycles { get; set; } = 0;
        private static float _lastUpdate = 0f;
        private static bool _isSet = false;
        protected RandomizerSeeds seeds = new RandomizerSeeds(0, RandomizerSeeds.Type.None, () => minuteCycles, () => useCycles, () => fullUseCycles);
        public RandomizerSeeds.Type type { get { return seeds == null ? RandomizerSeeds.Type.None : seeds.type; } }

        public virtual void Start()
        {
        }

        public virtual void Awake()
        {
            Reset();
        }

        public virtual void Update()
        {
            if (Time.time - _lastUpdate > 60f)
            {
                minuteCycles++;
                if (type == RandomizerSeeds.Type.Minute || type == RandomizerSeeds.Type.SeedlessMinute
                    || type == RandomizerSeeds.Type.MinuteUse || type == RandomizerSeeds.Type.SeedlessMinuteUse)
                {
                    _isSet = false;
                }
            }

            updateValues();
        }

        public virtual void Destroy()
        {
        }

        public virtual void Reset()
        {
            seeds.reset();
            _isSet = false;
            minuteCycles = 0;
            useCycles = 0;
            fullUseCycles = 0;
            _lastUpdate = Time.time;

            updateValues();
        }

        protected void updateSeed(int seed, RandomizerSeeds.Type type)
        {
            seeds.reset(seed, type);

            _isSet = false;
            updateValues();
        }

        private void updateValues()
        {
            if (!_isSet)
            {
                _isSet = true;
                if (type == RandomizerSeeds.Type.None)
                {
                    defaultValues();
                }
                else
                {
                    randomizeValues();
                }
            }
        }

        protected abstract void defaultValues();

        protected abstract void randomizeValues();
    }
}
