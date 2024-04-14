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
            return new University(coordinate);
        }
    }
}
