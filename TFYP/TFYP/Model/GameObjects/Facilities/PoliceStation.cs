using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TFYP.Model.GameObjects.Facilities
{
    internal class PoliceStation : Facility
    {
        public int InfluenceRadius { get; private set; } = 2; // influence area for safety
        public int SafetyIncrease { get; private set; } = 10; // safety impact

        public PoliceStation(Vector2 _coor, EBuildable _type) : base(_coor, _type)
        {
        }
        public PoliceStation(Vector2 coor, int constructionCost, int capacity, TimeSpan constructionTime)
            : base(coor, EBuildable.PoliceStation, constructionCost, capacity, constructionTime)
        {
        }

        public void RespondToIncident() { }

        public void IncreaseSafety() { }

    }
}
