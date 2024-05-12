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
using ProtoBuf;

namespace TFYP.Model.Zones
{
    [ProtoContract]
    [Serializable]
    public class IndustrialZone : Zone 
    {
        public IndustrialZone() { }
        public IndustrialZone(EBuildable type, List<Vector2> coor,int influenceRadius, int timeToBuild, int capacity, int maintenanceCost, int buildCost, DateTime dayOfCreation)
        : base(type, coor, influenceRadius, timeToBuild, capacity, maintenanceCost, buildCost, dayOfCreation)
        {

        }

        public new EBuildable type 
        {
            get { return EBuildable.Industrial; }
        }

    }
}
