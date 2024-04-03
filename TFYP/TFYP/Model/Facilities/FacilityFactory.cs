using Microsoft.Xna.Framework;

namespace TFYP.Model.Facilities
{
    public abstract class FacilityFactory
    {
        protected GameModel gameModel;

        protected FacilityFactory(GameModel gameModel)
        {
            this.gameModel = gameModel;
        }

        public abstract Facility CreateFacility(Vector2 coordinate);
    }
}
