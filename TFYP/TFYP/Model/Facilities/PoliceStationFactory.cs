using Microsoft.Xna.Framework;
using TFYP.Model.Common;
using TFYP.Model.Facilities;

namespace TFYP.Model.Factories
{
    public class PoliceStationFactory : FacilityFactory
    {
        public PoliceStationFactory(GameModel gameModel) : base(gameModel)
        {
        }

        public override Facility CreateFacility(Vector2 coordinate)
        {
            return new PoliceStation(coordinate, EBuildable.PoliceStation);
        }
    }
}
