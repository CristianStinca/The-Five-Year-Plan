using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TFYP.Model.Facilities;
using TFYP.Model.Common;
using TFYP.Model.City;

namespace TFYP.Model.Zones
{
    public class ServiceZone : Zone  // TO DO: implement logic around free work places in service zone!!!
    {
        public ServiceZone(EBuildable type, float effectRadius, double timeToBuild, int capacity, int maintenanceCost, int buildCost)
        : base(type, effectRadius, timeToBuild, capacity, maintenanceCost, buildCost)
        {
        
        }

        public new EBuildable type // new needed to "override it", alternative way, didn't want to use virtual  
        {
            get { return EBuildable.Service; }
        }

        // Adjusted constants for service zone

        public new int Capacity => Constants.ServiceZoneCapacity;
        public new int MaintenanceCost => Constants.ServiceZoneMaintenanceCost;
        public new int BuildCost => Constants.ServiceZoneBuildCost;
        public new float EffectRadius => Constants.ServiceEffectRadius;





    }
}
