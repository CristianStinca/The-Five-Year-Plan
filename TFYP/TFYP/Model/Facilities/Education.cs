using Microsoft.Xna.Framework;
using System;
using TFYP.Model.Common;

namespace TFYP.Model.Facilities
{
    public abstract class Education : Facility
    {
        public int GraduationTime { get; protected set; }
        public EducationLevel EducationLevel { get; protected set; }

        protected Education(Vector2 coordinate, EBuildable type, int constructionCost, int maintenanceCost,
                            int capacity, int graduationTime, EducationLevel educationLevel)
            : base(coordinate, type, constructionCost, maintenanceCost, capacity, TimeSpan.FromDays(graduationTime * 365))
        {
            GraduationTime = graduationTime;
            EducationLevel = educationLevel;
        }
    }
}
