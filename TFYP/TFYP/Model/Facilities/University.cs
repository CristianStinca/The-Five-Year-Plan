using Microsoft.Xna.Framework;
using TFYP.Model.Common;

namespace TFYP.Model.Facilities
{
    public class University : Education
    {
        public University(Vector2 _coor) : base(_coor, EBuildable.University)
        {
            MaxCapacity = Constants.ServiceZoneCapacity;
            MaintenanceCost = Constants.UniversityMaintenanceFee;
            ConstructionCost = Constants.UniversityBuildCost;
            GraduationTime = Constants.UniversityGraduationTime;
            // ConstructionTime = Constants.; // Must be added in Constants.cs
        }
        

    }
}
