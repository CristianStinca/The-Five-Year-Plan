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
    public class ServiceZoneFactory : ZoneFactory
    {
        public ServiceZoneFactory(GameModel _gameModel) : base(_gameModel)
        {
        }

        public override Zone CreateZone(int _x, int _y)
        {
            return new ServiceZone(
                EBuildable.Service, 
                Constants.ServiceEffectRadius, 
                10.0, 
                Constants.ServiceZoneCapacity, 
                Constants.ServiceZoneMaintenanceCost, 
                Constants.ServiceZoneBuildCost 
            );
        }
    }
}