using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TFYP.Model.GameObjects.Facilities
{
    internal abstract class Facility : Buildable
    {
        public Facility(Vector2 _coor, EBuildable _type) : base(_coor, _type)
        {
        }
    }
}
