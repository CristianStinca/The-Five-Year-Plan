using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TFYP.Model.Common;
using TFYP.Model.City;
using TFYP.Model.Zones;
using ProtoBuf;

namespace TFYP.Model.Facilities
{
    [ProtoContract]
    [Serializable]
    public class Facility : Buildable
    {
        [ProtoMember(1)]
        public int CurrentCapacity { get; set; }
        public Facility() { }
        public Facility(List<Vector2> _coor, EBuildable _type) : base(_coor, _type)
        {
        }

    }
}
