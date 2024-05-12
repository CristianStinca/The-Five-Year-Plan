using Microsoft.Xna.Framework;
using ProtoBuf;
using System;
using System.Collections.Generic;
using TFYP.Model.Common;

namespace TFYP.Model.Facilities
{
    [ProtoContract]
    [Serializable]
    public abstract class Education : Facility
    {
        [ProtoMember(1)]
        public int GraduationTime { get; set; }
        [ProtoMember(2)]
        public EducationLevel EducationLevel { get; set; }
        public Education() { }
        public Education(List<Vector2> _coor, EBuildable _type) :base(_coor, _type) { }

    }
}
