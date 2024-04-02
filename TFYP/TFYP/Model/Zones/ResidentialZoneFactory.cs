using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TFYP.Model.Facilities;
using TFYP.Model.Common;
using TFYP.Model.City;
using System.Security.Policy;

namespace TFYP.Model.Zones
{
    public class ResidentialZoneFactory : ZoneFactory
    {
        public ResidentialZoneFactory(GameModel _gameModel) : base(_gameModel)
        {
        }

        public override Zone CreateZone(int _x, int _y)
        {
            return new ResidentialZone(
                EBuildable.Residential, 
                Constants.ResidentialEffectRadius,
                10.0, // example value for `timeToBuild`, will change later 
                Constants.ResidentialZoneCapacity,
                Constants.ResidentialZoneMaintenanceCost,
                Constants.ResidentialZoneBuildCost
            );
        }
    }

}