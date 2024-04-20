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
    public class IndustrialZoneFactory : ZoneFactory
    {
        public IndustrialZoneFactory(GameModel _gameModel) : base(_gameModel)
        {
        }

        public override Zone CreateZone(int _x, int _y)
        {
            return new IndustrialZone(
                EBuildable.Industrial,
                new List<Vector2> { new Vector2(_x, _y) },
                Constants.IndustrialEffectRadius,
                10,
                Constants.IndustrialZoneCapacity, 
                Constants.IndustrialZoneMaintenanceCost, 
                Constants.IndustrialZoneBuildCost  
            );
        }
    }
}