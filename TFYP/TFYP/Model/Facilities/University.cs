using Microsoft.Xna.Framework;
using TFYP.Model.Common;

namespace TFYP.Model.Facilities
{
    public class University : Education
    {
        public University(Vector2 coordinate, int constructionCost, int maintenanceCost)
            : base(coordinate, EBuildable.University, constructionCost, maintenanceCost,
                   capacity: 5, graduationTime: 12, educationLevel: EducationLevel.University)
        {
        }

    }
}
