using OWML.Utils;
using PacificEngine.OW_CommonResources.Game;
using PacificEngine.OW_CommonResources.Game.Config;
using PacificEngine.OW_CommonResources.Game.Resource;
using PacificEngine.OW_CommonResources.Game.State;
using PacificEngine.OW_CommonResources.Geometry;
using PacificEngine.OW_CommonResources.Geometry.Orbits;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace PacificEngine.OW_Randomizer
{
    public class PlanetRandomizer : AbstractRandomizer
    {
        public static PlanetRandomizer instance { get; } = new PlanetRandomizer();

        public new void updateSeed(int seed, RandomizerSeeds.Type type)
        {
            base.updateSeed(seed, type);
        }

        public override void Awake()
        {
            base.Awake();

            Tracker.getTracked(Position.HeavenlyBodies.Player).onAstroUpdateEvent += onParentUpdate;
            Tracker.getTracked(Position.HeavenlyBodies.Ship).onAstroUpdateEvent += onParentUpdate;
            Tracker.getTracked(Position.HeavenlyBodies.Probe).onAstroUpdateEvent += onParentUpdate;
            Tracker.getTracked(Position.HeavenlyBodies.ModelShip).onAstroUpdateEvent += onParentUpdate;
            Tracker.getTracked(Position.HeavenlyBodies.NomaiEmberTwinShuttle).onAstroUpdateEvent += onParentUpdate;
            Tracker.getTracked(Position.HeavenlyBodies.NomaiBrittleHollowShuttle).onAstroUpdateEvent += onParentUpdate;
        }

        public override void Update()
        {
            base.Update();
        }

        protected override void defaultValues()
        {
            Planet.mapping = Planet.defaultMapping;
        }

        protected override void randomizeValues()
        {
            var defaultMapping = Planet.defaultMapping;
            var originalMapping = Planet.mapping;
            var mapping = new Dictionary<Position.HeavenlyBodies, Planet.Plantoid>();
            foreach (var planet in originalMapping)
            {
                var defaultValue = defaultMapping.ContainsKey(planet.Key) ? defaultMapping[planet.Key] : null;
                var newPlanet = randomPlantoid(originalMapping, mapping, planet.Key, planet.Value, defaultValue);
                mapping.Add(planet.Key, newPlanet);
            }
            Planet.mapping = mapping;
        }

        private Planet.Plantoid randomPlantoid(Dictionary<Position.HeavenlyBodies, Planet.Plantoid> originalMapping, Dictionary<Position.HeavenlyBodies, Planet.Plantoid> newMapping, Position.HeavenlyBodies body, Planet.Plantoid currentValue, Planet.Plantoid defaultValue)
        {
            Planet.Plantoid parent = originalMapping.ContainsKey(currentValue.state.parent) ? originalMapping[currentValue.state.parent] : null;
            switch (body)
            {
                case Position.HeavenlyBodies.GiantsDeep:
                    // Giants Deep Island's majorly glitch out if it isn't Facing Up with no rotational velocity
                    return new Planet.Plantoid(currentValue.size, currentValue.gravity, currentValue.state?.orbit?.rotation ?? currentValue.state.relative.rotation, currentValue.state?.orbit?.angularVelocity.magnitude ?? currentValue.state.relative.angularVelocity.magnitude, currentValue.state.parent, randomKepler(parent, currentValue));
                case Position.HeavenlyBodies.SunStation:
                case Position.HeavenlyBodies.HourglassTwins:
                case Position.HeavenlyBodies.Attlerock:
                case Position.HeavenlyBodies.Interloper:
                    // Because of AlignWithTargetBody
                    return new Planet.Plantoid(currentValue.size, currentValue.gravity, currentValue.state?.orbit?.rotation ?? currentValue.state.relative.rotation, 0f, currentValue.state.parent, randomKepler(parent, currentValue));
                case Position.HeavenlyBodies.ProbeCannon:
                    // It unspawns if too far from GiantsDeep
                    return new Planet.Plantoid(currentValue.size, currentValue.gravity, currentValue.state?.orbit?.rotation ?? currentValue.state.relative.rotation, 0f, currentValue.state.parent, randomKepler(parent, 0.00001f, 0.85f, parent.size.size + currentValue.size.size, 2000f));
                case Position.HeavenlyBodies.TimberHearth:
                case Position.HeavenlyBodies.TimberHearthProbe:
                case Position.HeavenlyBodies.BrittleHollow:
                case Position.HeavenlyBodies.HollowLantern:
                case Position.HeavenlyBodies.DarkBramble:
                case Position.HeavenlyBodies.BackerSatilite:
                    return new Planet.Plantoid(currentValue.size, currentValue.gravity, randomQuaternion(), (float)seeds.NextRange(-0.2, 0.2), currentValue.state.parent, randomKepler(parent, currentValue));
                case Position.HeavenlyBodies.WhiteHole:
                    // White Hole do not obey gravity
                    return new Planet.Plantoid(currentValue.size, currentValue.gravity, randomQuaternion(), currentValue.state.relative.angularVelocity.magnitude, currentValue.state.parent, randomPosition(parent, currentValue), currentValue.state.relative.velocity);
                case Position.HeavenlyBodies.WhiteHoleStation:
                    // White Hole Station break when not near the white hole
                    Planet.Plantoid whiteHole = newMapping[Position.HeavenlyBodies.WhiteHole];
                    return new Planet.Plantoid(currentValue.size, currentValue.gravity, randomQuaternion(), currentValue.state.relative.angularVelocity.magnitude, currentValue.state.parent, randomPosition(whiteHole, currentValue) + whiteHole.state.relative.position, currentValue.state.relative.velocity);
                case Position.HeavenlyBodies.Sun:
                case Position.HeavenlyBodies.AshTwin:
                case Position.HeavenlyBodies.EmberTwin:
                case Position.HeavenlyBodies.MapSatilite:
                    return defaultValue;
                case Position.HeavenlyBodies.Stranger:
                case Position.HeavenlyBodies.DreamWorld:
                case Position.HeavenlyBodies.QuantumMoon:
                default:
                    return currentValue;
            }
        }

        private Quaternion randomQuaternion()
        {
            return Quaternion.Euler((float)seeds.NextRange(0.0, 360.0), (float)seeds.NextRange(0.0, 360.0), (float)seeds.NextRange(0.0, 360.0));
        }

        private KeplerCoordinates randomKepler(Planet.Plantoid parent, Planet.Plantoid body)
        {
            var minDistance = parent.size.size + body.size.size;
            var maxDistance = parent.size.influence;
            return randomKepler(parent, 0.00001f, 0.85f, minDistance, maxDistance);
        }

        private KeplerCoordinates randomKepler(Planet.Plantoid parent, float minEccentricity, float maxEccentricity, float minOrbitalDistance, float maxOrbitalDistance)
        {
            KeplerCoordinates kepler;
            do
            {
                kepler = KeplerCoordinates.fromTrueAnomaly((float)seeds.NextRange(minEccentricity, maxEccentricity), (float)seeds.NextRange(minOrbitalDistance, maxOrbitalDistance), (float)seeds.NextRange(0.0, 180.0), (float)seeds.NextRange(0.0, 360.0), (float)seeds.NextRange(0.0, 360.0), (float)seeds.NextRange(0.0, 360.0));
            } while (maxOrbitalDistance < kepler.apogee || kepler.perigee < minOrbitalDistance);

            return kepler;
        }

        private Vector3 randomPosition(Planet.Plantoid parent, Planet.Plantoid body)
        {
            var minDistance = parent.size.size + body.size.size;
            var maxDistance = parent.size.influence;
            return randomPosition(minDistance, maxDistance);
        }

        private Vector3 randomPosition(float minOrbitalDistance, float maxOrbitalDistance)
        {
            Vector3 position;
            do
            {
                position = new Vector3((float)seeds.NextRange(-maxOrbitalDistance, maxOrbitalDistance), (float)seeds.NextRange(-maxOrbitalDistance, maxOrbitalDistance), (float)seeds.NextRange(-maxOrbitalDistance, maxOrbitalDistance));
            } while ((maxOrbitalDistance * maxOrbitalDistance) < position.sqrMagnitude || position.sqrMagnitude < (minOrbitalDistance * minOrbitalDistance));

            return position;
        }

        private void onParentUpdate(Position.HeavenlyBodies body, Position.HeavenlyBodies oldParent, Position.HeavenlyBodies newParent)
        {
            if (body == Position.HeavenlyBodies.None || oldParent == Position.HeavenlyBodies.None || newParent == Position.HeavenlyBodies.None)
            {
                return;
            }

            if (type == RandomizerSeeds.Type.FullUse || type == RandomizerSeeds.Type.SeedlessFullUse)
            {
                if (oldParent == Position.HeavenlyBodies.Sun)
                {
                    randomizeValues();
                }
            }
            else if (type == RandomizerSeeds.Type.Use || type == RandomizerSeeds.Type.SeedlessUse
                || type == RandomizerSeeds.Type.MinuteUse || type == RandomizerSeeds.Type.SeedlessMinuteUse)
            {
                if (oldParent != Position.HeavenlyBodies.Sun
                    && oldParent != Position.HeavenlyBodies.HourglassTwins)
                {
                    if (oldParent == Position.HeavenlyBodies.AshTwin
                        || oldParent == Position.HeavenlyBodies.EmberTwin)
                    {
                        oldParent = Position.HeavenlyBodies.HourglassTwins;
                    }
                    var defaultMapping = Planet.defaultMapping;
                    var originalMapping = Planet.mapping;
                    var mapping = originalMapping;

                    if (originalMapping.ContainsKey(oldParent))
                    {
                        var defaultValue = defaultMapping.ContainsKey(oldParent) ? defaultMapping[oldParent] : null;
                        mapping[oldParent] = randomPlantoid(originalMapping, mapping, oldParent, originalMapping[oldParent], defaultValue);
                        if (oldParent == Position.HeavenlyBodies.WhiteHole)
                        {
                            defaultValue = defaultMapping.ContainsKey(Position.HeavenlyBodies.WhiteHoleStation) ? defaultMapping[Position.HeavenlyBodies.WhiteHoleStation] : null;
                            mapping[Position.HeavenlyBodies.WhiteHoleStation] = randomPlantoid(originalMapping, mapping, Position.HeavenlyBodies.WhiteHoleStation, originalMapping[Position.HeavenlyBodies.WhiteHoleStation], defaultValue);
                        }

                        Planet.mapping = mapping;
                    }
                }
            }
        }
    }
}