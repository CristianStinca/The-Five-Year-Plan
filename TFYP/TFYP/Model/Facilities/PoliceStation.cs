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

namespace TFYP.Model.Facilities
{
    public class PoliceStation : Facility
    {
        public int SafetyIncrease { get; private set; } = 10; // safety impact

        public PoliceStation(List<Vector2> _coor, EBuildable _type) : base(_coor, _type)
        {
            Capacity = Constants.ServiceZoneCapacity;
            MaintenanceCost = Constants.PoliceStationMaintenanceFee;
            ConstructionCost = Constants.PoliceStationBuildCost;
            InfluenceRadius = Constants.PoliceStationEffectRadius;
        }
        

    }
}
