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
    public class ResidentialZone : Zone
    {
        public ResidentialZone(EBuildable type, float effectRadius, double timeToBuild, int capacity, int maintenanceCost, int buildCost)
        : base(type, effectRadius, timeToBuild, capacity, maintenanceCost, buildCost)
        {

        }

        public new EBuildable type   
        {
            get { return EBuildable.Residential; }
        }

        public new int Capacity => Constants.ResidentialZoneCapacity;
        public new int MaintenanceCost => Constants.ResidentialZoneMaintenanceCost;
        public new int BuildCost => Constants.ResidentialZoneBuildCost;
        public new float EffectRadius => Constants.ResidentialEffectRadius;
    }
}
