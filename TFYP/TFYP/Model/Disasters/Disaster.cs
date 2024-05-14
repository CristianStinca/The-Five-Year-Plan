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
using System.Diagnostics;
using ProtoBuf;

namespace TFYP.Model.Disasters 
{
    [ProtoContract]
    [Serializable]
    public class Disaster
    {
        [ProtoMember(1)]
        public string Name { get; set; }
        [ProtoMember(2)]
        public float EffectRadius { get;  set; }
        [ProtoMember(3)]
        public Vector2 Location { get; set; }
        [ProtoMember(4)]
        public DisasterType Type { get; set; }
        [ProtoMember(5)]
        public List<Zone> affectedZones = new List<Zone>();

        [ProtoMember(6)]
        public bool isActive { get;  set; }
        public Disaster() { }
        public Disaster( float effectRadius, Vector2 location)
        {
            EffectRadius = effectRadius;
            Type = this.SelectType();
            Location = location;
            isActive = true;
        }

        /// <summary>
        /// Applies the effects of the disaster to the zones within its effect radius in the game model.
        /// </summary>
        /// <param name="gameModel">The game model containing the zones.</param>
        public async void ApplyEffects(GameModel gameModel)
        {
            foreach(var zone in gameModel.CityRegistry.Zones)
            {
                if (IsWithinEffectRadius(zone, gameModel))
                {
                    affectedZones.Add(zone);
                    switch (Type)
                    {
                        case DisasterType.Fire:
                            DamageBuildings(zone, 50); // 50% damage to buildings/zones
                            break;
                        case DisasterType.Flood:
                            DamageBuildings(zone, 30); // 30% damage to buildings/zones
                            break;
                        case DisasterType.Earthquake:
                            DamageBuildings(zone, 70); // 70% damage to buildings/zones
                            break;
                        case DisasterType.GodzillaAttack:
                            DestroyBuildings(zone); // destroy buildings/zones
                            break;
                    }
                }
                
            }
            await Task.Delay(11000);  
            isActive = false;
        }

        /// <summary>
        /// Checks if the zone is within the effect radius of the disaster.
        /// </summary>
        /// <param name="zone">The zone to check.</param>
        /// <param name="gameModel">The game model containing distance calculation.</param>
        /// <returns>True if the zone is within the effect radius, otherwise false.</returns>

        private bool IsWithinEffectRadius(Zone zone, GameModel gameModel)
        {
            foreach (var i in zone.Coor) {
                float distance = gameModel.Distance(this.Location, i);
                if (distance <= this.EffectRadius)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Randomly selects a type of disaster.
        /// </summary>
        /// <returns>The selected type of disaster.</returns>
        private DisasterType SelectType() {
            Random random = new Random();
            int selection = random.Next(4);
            return (DisasterType)selection;   
        }

        /// <summary>
        /// Inflicts damage on buildings within the specified zone.
        /// </summary>
        /// <param name="zone">The zone where buildings are to be damaged.</param>
        /// <param name="damage">The amount of damage to inflict on the buildings.</param>

        private void DamageBuildings(Zone zone, float damage)
        {

            GameModel instance = GameModel.GetInstance();
            Zone z = (Zone)instance.map[(int)zone.Coor[0].X, (int)zone.Coor[0].Y];
            if (z.Health < damage)
            {
                z.SetHealth(0);
            }
            else
            {
                z.SetHealth(z.Health - damage);
            }
        }

        private void DestroyBuildings(Zone zone)
        {
            zone.SetHealth(0); 
        }


    }

    public enum DisasterType
    {
        Fire=0,
        Flood=1,
        Earthquake=2,
        GodzillaAttack=3 
    }




}