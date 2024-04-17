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
    public class ResidentialZone : Zone
    {
        public ResidentialZone(EBuildable type, Vector2 coor, int influenceRadius, int timeToBuild, int capacity, int maintenanceCost, int buildCost)
        : base(type, coor, influenceRadius, timeToBuild, capacity, maintenanceCost, buildCost)
        {

        }

        public new EBuildable type   
        {
            get { return EBuildable.Residential; }
        }

        public override List<Citizen> GetCitizens()
        {
            return base.GetCitizens();
        }
    }
}
