using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TFYP.Model.Facilities;
using TFYP.Model.City;
using TFYP.Model.Zones;

namespace TFYP.Model.Common
{
    internal class Buildable
    {
        public Vector2 coor {  get; }
        public EBuildable type { get; }

        public int ConstructionCost { get; set; }
        public int MaintenanceCost { get; set; }
        public int InfluenceRadius { get; set; }


        // Main constructor
        public Buildable(Vector2 _coor, EBuildable _type, int constructionCost = 0, int maintenanceCost = 0, int influenceRadius = 0)
        {
            this.coor = _coor;
            this.type = _type;
            ConstructionCost = constructionCost;
            MaintenanceCost = maintenanceCost;
            InfluenceRadius = influenceRadius;
        }

        // Convenience constructors
        public Buildable() : this(new Vector2(0, 0), EBuildable.None) { }
        public Buildable(Vector2 coor) : this(coor, EBuildable.None) { }
        public Buildable(EBuildable type) : this(new Vector2(0, 0), type) { }

    }

}
