using Microsoft.Xna.Framework;
using ProtoBuf;
using System;
using System.Collections.Generic;
using TFYP.Model.Common;

namespace TFYP.Model.Facilities
{
    [ProtoContract]
    [Serializable]
    public class University : Education
    {
        public University() { }
        public University(List<Vector2> _coor) : base(_coor, EBuildable.University)
        {
            Capacity = Constants.ServiceZoneCapacity;
            MaintenanceCost = Constants.UniversityMaintenanceFee;
            ConstructionCost = Constants.UniversityBuildCost;
            GraduationTime = Constants.UniversityGraduationTime;
        }
    }
}
