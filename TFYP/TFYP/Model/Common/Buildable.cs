﻿using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TFYP.Model.Facilities;
using TFYP.Model.City;
using TFYP.Model.Zones;

namespace TFYP.Model.Common
{
    public class Buildable
    {

        public List<Vector2> Coor { get; set; }
        public EBuildable Type { get; set; }
        public int ConstructionCost { get; set; }
        public int InfluenceRadius { get; set; }
        public int Capacity { get; set; }
        
        public List<Citizen> citizens;
        public int MaintenanceCost { get; set; }


        // Main constructor
        public Buildable(List<Vector2> _coor, EBuildable _type, int constructionCost = 0, int maintenanceCost = 0, int influenceRadius = 0, int capacity = 0, int timeToBuild = 0)
        {
            Coor = _coor;
            Type = _type;
            ConstructionCost = constructionCost;
            InfluenceRadius = influenceRadius;
            MaintenanceCost = maintenanceCost;
            Capacity = capacity;
            citizens = new List<Citizen>();

        }

        public virtual void startBuilding(DateTime buildingStartDate) { }

        public virtual void stopBuilding() { }

        public virtual bool checkToBuild() { return true; }

        public virtual void AddOutgoingRoad(Road r)
        {

        }
        public virtual void AddConnectedZone(Zone z) { }
        public virtual void ClearConnectedZone() { }
    }

}
