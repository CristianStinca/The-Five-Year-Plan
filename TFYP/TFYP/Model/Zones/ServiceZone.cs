using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TFYP.Model.Facilities;
using TFYP.Model.Common;
using TFYP.Model.City;
using Microsoft.Xna.Framework;

namespace TFYP.Model.Zones
{
    public class ServiceZone : Zone  // TO DO: implement logic around free work places in service zone!!!
    {
        public ServiceZone(EBuildable type, Vector2 coor, int influenceRadius, int timeToBuild, int capacity, int maintenanceCost, int buildCost)
        : base(type, coor, influenceRadius, timeToBuild, capacity, maintenanceCost, buildCost)
        {
        
        }

        public new EBuildable type // new needed to "override it", alternative way, didn't want to use virtual  
        {
            get { return EBuildable.Service; }
        }

    }
}
