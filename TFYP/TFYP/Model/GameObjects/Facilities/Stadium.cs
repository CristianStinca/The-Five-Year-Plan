using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TFYP.Model.GameObjects.Facilities
{
    internal class Stadium : Facility
    {
        public Stadium(Coordinate _coor, Dimension _dimension, EBuildable _type) : base(_coor, _dimension, _type) {
            Console.WriteLine("Stadium built");
        }

        
    }
}
