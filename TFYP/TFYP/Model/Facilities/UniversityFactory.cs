using Microsoft.Xna.Framework;
using System;
using TFYP.Model.Common;

namespace TFYP.Model.Facilities
{
    public class UniversityFactory
    {
        private GameModel gameModel;

        public UniversityFactory(GameModel gm)
        {
            this.gameModel = gm;
        }

        public University CreateFacility(Vector2 coordinate)
        {
            int constructionCost = Constants.UniversityBuildCost;
            int maintenanceCost = Constants.UniversityMaintenanceFee;

            return new University(coordinate, constructionCost, maintenanceCost);
        }
    }
}
