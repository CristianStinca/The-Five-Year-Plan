using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TFYP.Model.Facilities;
using TFYP.Model.Common;
using TFYP.Model.City;
using System.Security.Policy;
using Microsoft.Xna.Framework;

namespace TFYP.Model.Zones
{
    public class IndustrialZone : Zone 
    {
        public IndustrialZone(EBuildable type, List<Vector2> coor,int influenceRadius, int timeToBuild, int capacity, int maintenanceCost, int buildCost, DateTime dayOfBuildStart)
        : base(type, coor, influenceRadius, timeToBuild, capacity, maintenanceCost, buildCost, dayOfBuildStart)
        {

        }

        public new EBuildable type 
        {
            get { return EBuildable.Industrial; }
        }

    }
}
