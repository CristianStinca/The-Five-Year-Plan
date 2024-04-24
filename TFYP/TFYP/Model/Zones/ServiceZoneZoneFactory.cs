using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TFYP.Model.Facilities;
using TFYP.Model.Common;
using TFYP.Model.City;
using System.Security.Policy;
using Microsoft.Xna.Framework;

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
                new List<Vector2> { new Vector2(_x, _y) },
                Constants.ServiceEffectRadius, 
                10, 
                Constants.ServiceZoneCapacity, 
                Constants.ServiceZoneMaintenanceCost, 
                Constants.ServiceZoneBuildCost,
                DateTime.Now
            );
        }
    }
}