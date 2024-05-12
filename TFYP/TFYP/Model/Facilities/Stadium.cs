using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TFYP.Model.Common;
using TFYP.Model.City;
using TFYP.Model.Zones;
using ProtoBuf;

namespace TFYP.Model.Facilities
{
    [ProtoContract]
    [Serializable]
    public class Stadium : Facility
    {
        [ProtoMember(1)]
        public int HappinessBoost { get;  set; } = 5; // this will be changed later
        public Stadium() { }
        public Stadium(List<Vector2> _coor, EBuildable _type) : base(_coor, _type)
        {
            //Capacity = Constants.;
            MaintenanceCost = Constants.StadiumMaintenanceFee;
            ConstructionCost = Constants.StadiumBuildCost;
            InfluenceRadius = Constants.StadiumEffectRadius;
        }
        
    }
}
