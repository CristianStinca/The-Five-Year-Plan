using Microsoft.Xna.Framework;
using System;
using TFYP.Model.Common;

namespace TFYP.Model.Facilities
{
    public abstract class Education : Facility
    {
        public int GraduationTime { get; set; }
        public EducationLevel EducationLevel { get; set; }

        public Education(Vector2 _coor, EBuildable _type) :base(_coor, _type) { }

    }
}
