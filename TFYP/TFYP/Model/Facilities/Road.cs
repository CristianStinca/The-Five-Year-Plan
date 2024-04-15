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
    public class Road : Facility
    {
        public bool IsConnected {  get; private set; }
        public Road(Vector2 _coor, EBuildable _type) : base(_coor, _type)
        {
            IsConnected = false;
            MaintenanceCost = Constants.RoadMaintenanceFee;
            ConstructionCost = Constants.RoadBuildCost;
            // ConstructionTime = Constants.; // Must be added in Constants.cs
        }


    }
}
