using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TFYP.Model.GameObjects
{
    internal class Buildable
    {
        public Coordinate coor {  get; }
        public Dimension dimension { get; }
        public EBuildable type { get; }

        public Buildable(Coordinate _coor, Dimension _dimension, EBuildable _type)
        {
            this.coor = _coor;
            this.dimension = _dimension;
            this.type = _type;
        }

        public Buildable() : this(new Coordinate(0, 0), Dimension.DEFAULT, EBuildable.None) { }
        public Buildable(Coordinate _coor) : this(_coor, Dimension.DEFAULT, EBuildable.None) { }
        public Buildable(Dimension _dimension) : this(new Coordinate(0, 0), _dimension, EBuildable.None) { }
        public Buildable(EBuildable _type) : this(new Coordinate(0, 0), Dimension.DEFAULT, _type) { }
        public Buildable(Coordinate _coor, Dimension _dimension) : this(_coor, _dimension, EBuildable.None) { }
        public Buildable(Coordinate _coor, EBuildable _type) : this(_coor, Dimension.DEFAULT, _type) { }
        public Buildable(Dimension _dimension, EBuildable _type) : this(new Coordinate(0, 0), _dimension, _type) { }
    }
}
