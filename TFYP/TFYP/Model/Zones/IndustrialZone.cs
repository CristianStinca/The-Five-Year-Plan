using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TFYP.Model.Facilities;
using TFYP.Model.Common;
using TFYP.Model.City;
using System.Security.Policy;

namespace TFYP.Model.Zones
{
    public class IndustrialZone : Zone 
    {
        public IndustrialZone(EBuildable type, float effectRadius, double timeToBuild, int capacity, int maintenanceCost, int buildCost)
        : base(type, effectRadius, timeToBuild, capacity, maintenanceCost, buildCost)
        {

        }

        public new EBuildable type 
        {
            get { return EBuildable.Industrial; }
        }


        public new int Capacity => Constants.IndustrialZoneCapacity;
        public new int MaintenanceCost => Constants.IndustrialZoneMaintenanceCost;
        public new int BuildCost => Constants.IndustrialZoneBuildCost;
        public new float EffectRadius => Constants.IndustrialEffectRadius;

        //TO DO - pollution effect



    }
}
