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
        private static int _totalCycles = 0;
        private static float _lastUpdate = 0f;
        private static bool _isSet = false;
        protected RandomizerSeeds seeds;
        public RandomizerSeeds.Type? type { get { return seeds?.type; } }

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
                _totalCycles++;
                _lastUpdate = Time.time;
                if (type == RandomizerSeeds.Type.Minute || type == RandomizerSeeds.Type.SeedlessMinute)
                {
                    _isSet = false;
                }
            }

            updateValues(0);
        }

        public virtual void Destroy()
        {
        }

        public virtual void Reset()
        {
            seeds.reset();
            _isSet = false;
            _totalCycles = 0;
            _lastUpdate = Time.time;

            updateValues(0);
        }

        protected void updateSeed(int seed, RandomizerSeeds.Type? type)
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
            if (type == RandomizerSeeds.Type.Minute || type == RandomizerSeeds.Type.SeedlessMinute)
            {
                updateValues(_totalCycles);
            }
            updateValues(0);
        }

        private void updateValues(int cycles)
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

        protected abstract void defaultValues();

        protected abstract void randomizeValues(int cycles);
    }
}
