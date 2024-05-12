using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace TFYP.Model.Facilities
{
    public abstract class FacilityFactory
    {
        public GameModel gameModel;

        protected FacilityFactory(GameModel gameModel)
        {
            this.gameModel = gameModel;
        }

        public abstract Facility CreateFacility(List<Vector2> coordinate);
    }
}
