using Microsoft.Xna.Framework;
using TFYP.Model.Common;
using TFYP.Model.Facilities;

namespace TFYP.Model.Factories
{
    public class StadiumFactory : FacilityFactory
    {
        public StadiumFactory(GameModel gameModel) : base(gameModel)
        {
        }

        public override Facility CreateFacility(Vector2 coordinate)
        {
            return new Stadium(coordinate, EBuildable.Stadium);
        }
    }
}
