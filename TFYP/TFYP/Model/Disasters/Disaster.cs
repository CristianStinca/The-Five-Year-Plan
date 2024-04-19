using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TFYP.Model;
using TFYP.Model.Facilities;
using TFYP.Model.Common;
using TFYP.Model.City;
using TFYP.Model.Zones;
using System.Security.Policy;
using Microsoft.Xna.Framework;

namespace TFYP.Model.Disasters 
{
    public class Disaster
    {
        public string Name { get; set; }
        public float EffectRadius { get; private set; }
        public Vector2 Location { get; set; }
        public DisasterType Type { get; set; }

        public Disaster(string name, float effectRadius, DisasterType type, Vector2 location)
        {
            Name = name;
            EffectRadius = effectRadius;
            Type = type;
            Location = location;
        }

        public void ApplyEffects(GameModel gameModel)
        {
            foreach(var zone in gameModel.Zones)
            {
                // Check if the zone is within the effect radius of the disaster
                if (IsWithinEffectRadius(zone))
                {
                    switch (Type)
                    {
                        case DisasterType.Fire:
                            DamageBuildings(zone, 50); // 50% damage to buildings/zones
                            AffectPopulation(zone, -10); // reduce population happiness by 10
                            break;
                        case DisasterType.Flood:
                            DamageBuildings(zone, 30); // 30% damage to buildings/zones
                            AffectPopulation(zone, -5); // reduce population happiness by 5
                            break;
                        case DisasterType.Earthquake:
                            DamageBuildings(zone, 70); // 70% damage to buildings/zones
                            AffectPopulation(zone, -15); // reduce population happiness by 15
                            break;
                        case DisasterType.GodzillaAttack:
                            DestroyBuildings(zone); // destroy buildings/zones
                            AffectPopulation(zone, -20); // reduce population happiness by 20
                            break;
                    }
                }
            }
            //foreach (var citizen in gameModel.Citizens)
            //{
            //    if (IsWithinDisasterRadius(citizen.Location))
            //    {
            //        // Decrease health/satisfaction
            //    }
            //}

            // Affect the city's economy
            //gameModel.Budget.AdjustForDisaster(recoveryCosts, lostIncome);
        }

        private bool IsWithinEffectRadius(Zone zone)
        {
            float distance = Vector2.Distance(this.Location, zone.Coor);
            return distance <= this.EffectRadius;
        }

        private void DamageBuildings(Zone zone, float damagePercent)
        {
            //foreach (var building in zone.Buildings)
            //{
            //    building.Integrity -= building.Integrity * damagePercent;
            //}
            zone.SetHealth(zone.Health * (100 - damagePercent));
        }

        private void DestroyBuildings(Zone zone)
        {
            //foreach (var building in zone.Buildings)
            //{
            //    building.IsDestroyed = true;
            //}
            zone.SetHealth(0); // HP is 0 so zone is destroyed
        }

        private void AffectPopulation(Zone zone, int happinessChange)
        {
            //foreach (var citizen in zone.Citizens)
            //{
            //    citizen.Happiness += happinessChange;
            //}
        }

    }

    // Add/Change types
    public enum DisasterType
    {
        Fire,
        Flood,
        Earthquake,
        GodzillaAttack 
    }


    /*TO DO: we need to add this method in game model or controller to trigger disasters at random or under certain conditions
     
     
     public void TriggerRandomDisaster(GameModel gameModel)
    {
        // Example: Randomly decide to trigger a disaster
        var random = new Random();
        if (random.NextDouble() < 0.1) // 10% chance to trigger a disaster
        {
            // Randomly generate disaster parameters - TYPE AND LOCATION
            var disasterType = ..
            var location = ...
            var EfectedRadius = ...

            var disaster = new Disaster("Generic Disaster", "A disaster has occurred!", location, radius, disasterType);

            // THIS FUNCTION SHOULD BE IMPLEMENTED
            disaster.ApplyEffects(gameModel);

            Console.WriteLine($"A {disasterType} disaster occurred at ({location.X}, {location.Y}) with radius {EffectRadius}.");
        }
    }
     
    
     */


}