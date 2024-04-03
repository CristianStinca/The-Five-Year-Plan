using Microsoft.Xna.Framework;
using TFYP.Model.Common;

namespace TFYP.Model.Facilities
{
    public class School : Education
    {
        public School(Vector2 coordinate, int constructionCost, int maintenanceCost)
            : base(coordinate, EBuildable.School, constructionCost, maintenanceCost,
                   capacity: 5, graduationTime: 12, educationLevel: EducationLevel.School)
        {
        }

    }
}
