using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TFYP.Model.Facilities;
using TFYP.Model.Common;
using TFYP.Model.City;
using Microsoft.Xna.Framework;
using ProtoBuf;

namespace TFYP.Model.Zones
{
    [ProtoContract]
    [Serializable]
    public class ServiceZone : Zone 
    {
        public ServiceZone() { }
        public ServiceZone(EBuildable type, List<Vector2> coor, int influenceRadius, int timeToBuild, int capacity, int maintenanceCost, int buildCost, DateTime dayOfCreation)
        : base(type, coor, influenceRadius, timeToBuild, capacity, maintenanceCost, buildCost, dayOfCreation)
        {
        
        }

        public new EBuildable type // new needed to "override it", alternative way, didn't want to use virtual  
        {
            get { return EBuildable.Service; }
        }

    }
}
