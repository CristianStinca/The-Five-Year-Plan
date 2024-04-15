using Microsoft.Xna.Framework;
using System;
using TFYP.Model.Common;

namespace TFYP.Model.Facilities
{
    public class UniversityFactory : FacilityFactory
    {
        public UniversityFactory(GameModel gm) :base(gm) { }
        public override Facility CreateFacility(Vector2 coordinate)
        {
            return new University(coordinate);
        }
    }
}
