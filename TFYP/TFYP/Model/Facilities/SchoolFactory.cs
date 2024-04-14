using Microsoft.Xna.Framework;
using System;
using TFYP.Model.Common;

namespace TFYP.Model.Facilities
{
    public class SchoolFactory
    {
        private GameModel gameModel;

        public SchoolFactory(GameModel gm)
        {
            this.gameModel = gm;
        }

        public School CreateFacility(Vector2 coordinate)
        {
            return new School(coordinate);
        }
    }
}
