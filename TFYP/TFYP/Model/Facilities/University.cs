using Microsoft.Xna.Framework;
using System.Collections.Generic;
using TFYP.Model.Common;

namespace TFYP.Model.Facilities
{
    public class University : Education
    {
        public University(List<Vector2> _coor) : base(_coor, EBuildable.University)
        {
            Capacity = Constants.ServiceZoneCapacity;
            MaintenanceCost = Constants.UniversityMaintenanceFee;
            ConstructionCost = Constants.UniversityBuildCost;
            GraduationTime = Constants.UniversityGraduationTime;
            TimeToBuild = Constants.UniversityConstructionTime; 
        }
    }
}
