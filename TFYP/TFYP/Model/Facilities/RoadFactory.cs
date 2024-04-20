using Microsoft.Xna.Framework;
using System.Collections.Generic;
using TFYP.Model.Common;
using TFYP.Model.Facilities;

namespace TFYP.Model.Factories
{
    public class RoadFactory : FacilityFactory
    {
        public RoadFactory(GameModel gameModel) : base(gameModel)
        {
        }

        public override Facility CreateFacility(List<Vector2> coordinate)
        {
            return new Road(coordinate, EBuildable.Road);
        }
    }
}
