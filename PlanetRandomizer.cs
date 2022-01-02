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
        private static bool hasUpdate = false;
        public static PlanetRandomizer instance { get; } = new PlanetRandomizer();

        public new void updateSeed(int seed, RandomizerSeeds.Type type)
        {
            base.updateSeed(seed, type);
        }

        public override void Start()
        {
            Planet.enabledManagement = true;
        }

        public override void Awake()
        {
            hasUpdate = false;
            Tracker.getTracked(HeavenlyBodies.Player).onAstroUpdateEvent += onParentUpdate;
            Tracker.getTracked(HeavenlyBodies.Ship).onAstroUpdateEvent += onParentUpdate;
            Tracker.getTracked(HeavenlyBodies.Probe).onAstroUpdateEvent += onParentUpdate;
            Tracker.getTracked(HeavenlyBodies.ModelShip).onAstroUpdateEvent += onParentUpdate;
            Tracker.getTracked(HeavenlyBodies.NomaiEmberTwinShuttle).onAstroUpdateEvent += onParentUpdate;
            Tracker.getTracked(HeavenlyBodies.NomaiBrittleHollowShuttle).onAstroUpdateEvent += onParentUpdate;
        }

        public override void Update()
        {
            base.Update();
        }

        public override void FixedUpdate()
        {
            if (!hasUpdate && GameTimer.FramesSinceAwake > 1)
            {
                Reset();
                hasUpdate = true;
            }
        }

        protected override void defaultValues()
        {
            Planet.mapping = Planet.defaultMapping;
        }

        protected override void randomizeValues()
        {
            var defaultMapping = Planet.defaultMapping;
            var originalMapping = Planet.mapping;
            var mapping = new Dictionary<HeavenlyBody, Planet.Plantoid>();
            foreach (var planet in defaultMapping)
            {
                var currentValue = originalMapping.ContainsKey(planet.Key) ? originalMapping[planet.Key] : null;
                var newPlanet = randomPlantoid(defaultMapping, originalMapping, mapping, planet.Key, currentValue, planet.Value);
                mapping.Add(planet.Key, newPlanet);
            }
            Planet.mapping = mapping;
        }

        private Planet.Plantoid randomPlantoid(Dictionary<HeavenlyBody, Planet.Plantoid> defaultMapping, Dictionary<HeavenlyBody, Planet.Plantoid> originalMapping, Dictionary<HeavenlyBody, Planet.Plantoid> newMapping, HeavenlyBody body, Planet.Plantoid currentValue, Planet.Plantoid defaultValue)
        {
            if (currentValue == null)
            {
                return null;
            }

            Planet.Plantoid parent = originalMapping.ContainsKey(currentValue.state.parent) ? originalMapping[currentValue.state.parent] : null;
            if (parent == null)
            {
                parent = defaultMapping.ContainsKey(currentValue.state.parent) ? defaultMapping[currentValue.state.parent] : null;
            }

            if (body == HeavenlyBodies.GiantsDeep)
            {
                // Giants Deep Island's majorly glitch out if it isn't Facing Up with no rotational velocity
                return new Planet.Plantoid(currentValue.size, currentValue.gravity, currentValue.state?.orbit?.rotation ?? currentValue.state.relative.rotation, currentValue.state?.orbit?.angularVelocity.magnitude ?? currentValue.state.relative.angularVelocity.magnitude, currentValue.state.parent, randomKepler(parent, body, currentValue));
            }
            else if (body == HeavenlyBodies.SunStation
                || body == HeavenlyBodies.HourglassTwins
                || body == HeavenlyBodies.Attlerock
                || body == HeavenlyBodies.Interloper
                || body == HeavenlyBodies.TimberHearthProbe)
            {
                // Because of AlignWithTargetBody
                return new Planet.Plantoid(currentValue.size, currentValue.gravity, currentValue.state?.orbit?.rotation ?? currentValue.state.relative.rotation, 0f, currentValue.state.parent, randomKepler(parent, body, currentValue));
            }
            else if (body == HeavenlyBodies.ProbeCannon)
            {
                // It unspawns if too far from GiantsDeep
                return new Planet.Plantoid(currentValue.size, currentValue.gravity, currentValue.state?.orbit?.rotation ?? currentValue.state.relative.rotation, 0f, currentValue.state.parent, randomKepler(parent, body, 0.00001f, 0.85f, parent.size.size + currentValue.size.size, 2000f));
            }
            else if (body == HeavenlyBodies.TimberHearth
                || body == HeavenlyBodies.BrittleHollow
                || body == HeavenlyBodies.HollowLantern
                || body == HeavenlyBodies.DarkBramble
                || body == HeavenlyBodies.SatiliteBacker)
            {
                return new Planet.Plantoid(currentValue.size, currentValue.gravity, randomQuaternion(), (float)seeds.NextRange(-0.2, 0.2), currentValue.state.parent, randomKepler(parent, body, currentValue));
            }
            else if (body == HeavenlyBodies.WhiteHole)
            {
                // White Hole do not obey gravity
                parent = originalMapping.ContainsKey(HeavenlyBodies.Sun) ? originalMapping[HeavenlyBodies.Sun] : null;
                return new Planet.Plantoid(currentValue.size, currentValue.gravity, randomQuaternion(), currentValue.state.relative.angularVelocity.magnitude, currentValue.state.parent, randomPosition(parent, body, currentValue), currentValue.state.relative.velocity);
            }
            else if (body == HeavenlyBodies.WhiteHoleStation)
            {
                // White Hole Station break when not near the white hole
                Planet.Plantoid whiteHole = newMapping[HeavenlyBodies.WhiteHole];
                return new Planet.Plantoid(currentValue.size, currentValue.gravity, randomQuaternion(), currentValue.state.relative.angularVelocity.magnitude, currentValue.state.parent, randomPosition(whiteHole, body, currentValue) + whiteHole.state.relative.position, currentValue.state.relative.velocity);
            }
            else if (body == HeavenlyBodies.Sun
                || body == HeavenlyBodies.AshTwin
                || body == HeavenlyBodies.EmberTwin
                || body == HeavenlyBodies.SatiliteMapping)
            {
                return defaultValue;
            }
            else if (body == HeavenlyBodies.Stranger
                || body == HeavenlyBodies.DreamWorld
                || body == HeavenlyBodies.QuantumMoon
                || body == HeavenlyBodies.EyeOfTheUniverse
                || body == HeavenlyBodies.EyeOfTheUniverse_Vessel)
            {
                return currentValue;
            }
            else if (parent != null)
            {
                return new Planet.Plantoid(currentValue.size, currentValue.gravity, randomQuaternion(), (float)seeds.NextRange(-0.2, 0.2), currentValue.state.parent, randomKepler(parent, body, currentValue));
            }
            else
            {
                return currentValue;
            }
        }

        private Quaternion randomQuaternion()
        {
            return Quaternion.Euler((float)seeds.NextRange(0.0, 360.0), (float)seeds.NextRange(0.0, 360.0), (float)seeds.NextRange(0.0, 360.0));
        }

        private KeplerCoordinates randomKepler(Planet.Plantoid parent, HeavenlyBody body, Planet.Plantoid oldState)
        {
            var minDistance = parent.size.size + oldState.size.size;
            var maxDistance = parent.size.influence;
            return randomKepler(parent, body, 0.00001f, 0.85f, minDistance, maxDistance);
        }

        private KeplerCoordinates randomKepler(Planet.Plantoid parent, HeavenlyBody body, float minEccentricity, float maxEccentricity, float minOrbitalDistance, float maxOrbitalDistance)
        {
            KeplerCoordinates kepler;
            var maxFocus = (maxOrbitalDistance - minOrbitalDistance) / 2f;
            var maxRadius = minOrbitalDistance + maxFocus;
            var newMaxEccentricity = Ellipse.getEccentricity(maxRadius, maxFocus);
            if (newMaxEccentricity < maxEccentricity)
            {
                maxEccentricity = newMaxEccentricity;
            }

            if (minEccentricity > maxEccentricity)
            {
                var minRadius = minOrbitalDistance / (1 - minEccentricity);
                maxRadius = maxOrbitalDistance / (1 + minEccentricity);
                if (minRadius > maxRadius)
                {
                    return KeplerCoordinates.fromTrueAnomaly(minEccentricity, minRadius, (float)seeds.NextRange(-90.0, 90.0), (float)seeds.NextRange(0.0, 360.0), (float)seeds.NextRange(0.0, 360.0), (float)seeds.NextRange(0.0, 360.0));
                }
                return KeplerCoordinates.fromTrueAnomaly(minEccentricity, (float)seeds.NextRange(minRadius, maxRadius), (float)seeds.NextRange(-90.0, 90.0), (float)seeds.NextRange(0.0, 360.0), (float)seeds.NextRange(0.0, 360.0), (float)seeds.NextRange(0.0, 360.0));
            }
            else if (minOrbitalDistance > maxOrbitalDistance)
            {
                var minRadius = minOrbitalDistance / (1 - minEccentricity);
                return KeplerCoordinates.fromTrueAnomaly(minEccentricity, minRadius, (float)seeds.NextRange(-90.0, 90.0), (float)seeds.NextRange(0.0, 360.0), (float)seeds.NextRange(0.0, 360.0), (float)seeds.NextRange(0.0, 360.0));
            }
            else
            {
                var eccentricity = (float)seeds.NextRange(minEccentricity, maxEccentricity);
                var minRadius = minOrbitalDistance / (1 - eccentricity);
                maxRadius = maxOrbitalDistance / (1 + minEccentricity);

                if (minRadius > maxRadius)
                {
                    return KeplerCoordinates.fromTrueAnomaly(eccentricity, minRadius, (float)seeds.NextRange(-90.0, 90.0), (float)seeds.NextRange(0.0, 360.0), (float)seeds.NextRange(0.0, 360.0), (float)seeds.NextRange(0.0, 360.0));
                }
                return KeplerCoordinates.fromTrueAnomaly(eccentricity, (float)seeds.NextRange(minRadius, maxRadius), (float)seeds.NextRange(-90.0, 90.0), (float)seeds.NextRange(0.0, 360.0), (float)seeds.NextRange(0.0, 360.0), (float)seeds.NextRange(0.0, 360.0));
            }
        }

        private Vector3 randomPosition(Planet.Plantoid parent, HeavenlyBody body, Planet.Plantoid oldState)
        {
            var minDistance = parent.size.size + oldState.size.size;
            var maxDistance = parent.size.influence;
            return randomPosition(body, minDistance, maxDistance);
        }

        private Vector3 randomPosition(HeavenlyBody body, float minOrbitalDistance, float maxOrbitalDistance)
        {
            if (minOrbitalDistance > maxOrbitalDistance)
            {
                return new Vector3((float)seeds.NextRange(-1, 1), (float)seeds.NextRange(-1, 1), (float)seeds.NextRange(-1, 1)).normalized * minOrbitalDistance;
            }
            else
            {
                return new Vector3((float)seeds.NextRange(-1, 1), (float)seeds.NextRange(-1, 1), (float)seeds.NextRange(-1, 1)).normalized * (float)seeds.NextRange(minOrbitalDistance, maxOrbitalDistance);
            }
        }

        private void onParentUpdate(HeavenlyBody body, HeavenlyBody oldParent, HeavenlyBody newParent)
        {
            if (body == HeavenlyBodies.None || oldParent == HeavenlyBodies.None || newParent == HeavenlyBodies.None)
            {
                return;
            }

            if (type == RandomizerSeeds.Type.FullUse || type == RandomizerSeeds.Type.SeedlessFullUse)
            {
                if (oldParent == HeavenlyBodies.Sun)
                {
                    randomizeValues();
                }
            }
            else if (type == RandomizerSeeds.Type.Use || type == RandomizerSeeds.Type.SeedlessUse
                || type == RandomizerSeeds.Type.MinuteUse || type == RandomizerSeeds.Type.SeedlessMinuteUse)
            {
                if (oldParent != HeavenlyBodies.Sun
                    && oldParent != HeavenlyBodies.HourglassTwins)
                {
                    var toUpdate = oldParent;
                    if (toUpdate == HeavenlyBodies.AshTwin
                        || toUpdate == HeavenlyBodies.EmberTwin)
                    {
                        toUpdate = HeavenlyBodies.HourglassTwins;
                    }
                    var defaultMapping = Planet.defaultMapping;
                    var originalMapping = Planet.mapping;
                    var mapping = originalMapping;

                    if (originalMapping.ContainsKey(toUpdate))
                    {
                        var defaultValue = defaultMapping.ContainsKey(toUpdate) ? defaultMapping[toUpdate] : null;
                        mapping[toUpdate] = randomPlantoid(defaultMapping, originalMapping, mapping, toUpdate, originalMapping[toUpdate], defaultValue);
                        if (toUpdate == HeavenlyBodies.WhiteHole)
                        {
                            toUpdate = HeavenlyBodies.WhiteHoleStation;
                            defaultValue = defaultMapping.ContainsKey(toUpdate) ? defaultMapping[toUpdate] : null;
                            mapping[toUpdate] = randomPlantoid(defaultMapping, originalMapping, mapping, toUpdate, originalMapping[toUpdate], defaultValue);
                        }

                        Planet.mapping = mapping;
                    }
                }
            }
        }
    }
}