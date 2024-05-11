using Microsoft.Xna.Framework;
using System.Collections.Generic;
using TFYP.Model.Common;

namespace TFYP.Model.Facilities
{
    public class School : Education
    {
        public School(List<Vector2> _coor) :base(_coor, EBuildable.School)
        {
            Capacity = Constants.ServiceZoneCapacity;
            MaintenanceCost = Constants.SchoolMaintenanceFee;
            ConstructionCost = Constants.SchoolBuildCost;
            GraduationTime = Constants.SchoolGraduationTime;
        }

    }
}
