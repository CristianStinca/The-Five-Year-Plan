using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TFYP.Model.GameObjects.Facilities
{
    internal abstract class Facility : Buildable
    {
        public Facility(Coordinate _coor, Dimension _dimension, EBuildable _type) : base(_coor, _dimension, _type) {}
    }
}
