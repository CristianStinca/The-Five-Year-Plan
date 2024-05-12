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
    public class PoliceStation : Facility
    {
        [ProtoMember(1)]
        public int SafetyIncrease { get; set; } = 10; // safety impact
        public PoliceStation() { }

        public PoliceStation(List<Vector2> _coor, EBuildable _type) : base(_coor, _type)
        {
            Capacity = Constants.ServiceZoneCapacity;
            MaintenanceCost = Constants.PoliceStationMaintenanceFee;
            ConstructionCost = Constants.PoliceStationBuildCost;
            InfluenceRadius = Constants.PoliceStationEffectRadius;
        }
        

    }
}
