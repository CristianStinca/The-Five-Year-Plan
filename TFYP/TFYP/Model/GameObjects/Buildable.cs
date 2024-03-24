using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TFYP.Model.GameObjects
{
    internal class Buildable
    {
        public Vector2 coor {  get; }
        public EBuildable type { get; }

        public Buildable(Vector2 _coor, EBuildable _type)
        {
            this.coor = _coor;
            this.type = _type;
        }

        public Buildable() : this(new Vector2(0, 0), EBuildable.None) { }
        public Buildable(Vector2 _coor) : this(_coor, EBuildable.None) { }
        public Buildable(EBuildable _type) : this(new Vector2(0, 0), _type) { }
    }
}
