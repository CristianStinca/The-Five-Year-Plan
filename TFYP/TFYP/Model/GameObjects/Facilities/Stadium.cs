using Microsoft.Xna.Framework;
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
        public int InfluenceRadius { get; private set; } = 2;
        public int HappinessBoost { get; private set; } = 5; // this will be changed later
        public Stadium(Vector2 _coor, EBuildable _type) : base(_coor, _type)
        {
        }
        public Stadium(Vector2 coor, int constructionCost, int capacity, TimeSpan constructionTime)
            : base(coor, EBuildable.Stadium, constructionCost, capacity, constructionTime)
        {
        }
    }
}
