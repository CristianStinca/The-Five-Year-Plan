using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using TFYP.Model.Common;

namespace TFYP.Model.Facilities
{
    public class SchoolFactory : FacilityFactory
    {
        public SchoolFactory(GameModel gm) :base(gm) { }

        public override Facility CreateFacility(List<Vector2> coordinate)
        {
            return new School(coordinate);
        }
    }
}
