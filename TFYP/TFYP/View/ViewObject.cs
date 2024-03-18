using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TFYP.View
{
    internal class ViewObject
    {
        public float scale {  get; set; }
        public int x { get; set; }
        public int y { get; set; }
        public string name { get; set; }

        public ViewObject(string _name, int _x, int _y, float _scale = 1f)
        {
            this.name = _name;
            this.scale = _scale;
            this.x = _x;
            this.y = _y;
        }

        public ViewObject(string _name) : this(_name, 0, 0) { }
    }
}
