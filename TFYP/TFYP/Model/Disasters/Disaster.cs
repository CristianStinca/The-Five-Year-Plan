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

namespace TFYP.Model.Disasters 
{
    public class Disaster
    {
        public string Name { get; set; }
        public float EffectRadius { get; private set; }
        //need to add LOCATION
        public DisasterType Type { get; set; }

        public Disaster(string name, float effectRadius, DisasterType type)
        {
            Name = name;
            EffectRadius = effectRadius;
            Type = type;
        }

        public void ApplyEffects(GameModel gameModel)
        {
            // Implement the logic to apply disaster effects based on the DisasterType and other properties
            // damaging buildings, affecting population...
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