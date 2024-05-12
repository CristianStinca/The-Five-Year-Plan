using Microsoft.Xna.Framework;
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
using ProtoBuf;

namespace TFYP.Model.Common
{
    [ProtoContract]
    [Serializable]
    public class Buildable
    {
        [ProtoMember(1)]
        public List<Vector2> Coor { get; set; }
        [ProtoMember(2)]
        public EBuildable Type { get; set; }
        [ProtoMember(3)]
        public int ConstructionCost { get; set; }
        [ProtoMember(4)]
        public int InfluenceRadius { get; set; }
        [ProtoMember(5)]
        public int Capacity { get; set; }
        [ProtoMember(6)]
        public List<Citizen> citizens;
        [ProtoMember(7)]
        public int MaintenanceCost { get; set; }

        public Buildable() {
            Coor = new List<Vector2>();
            citizens = new List<Citizen>();
        }
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
