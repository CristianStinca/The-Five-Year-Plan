using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TFYP.Model.Facilities;
using TFYP.Model.Common;
using TFYP.Model.City; 

namespace TFYP.Model.Zones
{
    public abstract class ZoneFactory
    {
        public GameModel _gameModel;

        public ZoneFactory(GameModel _gameModel)
        {
            this._gameModel = _gameModel;
        }

        public abstract Zone CreateZone(int _x, int _y);
    }
}