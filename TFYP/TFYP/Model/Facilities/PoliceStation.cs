using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TFYP.Model.Common;
using TFYP.Model.City;
using TFYP.Model.Zones;

namespace TFYP.Model.Facilities
{
    public class PoliceStation : Facility
    {
        public int SafetyIncrease { get; private set; } = 10; // safety impact

        public PoliceStation(List<Vector2> _coor, EBuildable _type) : base(_coor, _type)
        {
            Capacity = Constants.ServiceZoneCapacity;
            MaintenanceCost = Constants.PoliceStationMaintenanceFee;
            ConstructionCost = Constants.PoliceStationBuildCost;
            InfluenceRadius = Constants.PoliceStationEffectRadius;
            TimeToBuild = Constants.PoliceConstructionTime;
        }
        

        public void RespondToIncident() 
        {
            //if (IsWithinInfluenceRadius(zone))
            //{
            //    // Implementation for responding to an incident
            //}
        }

        // Increase safety in the surrounding area
        public void IncreaseSafety(Zone zone)
        {
            //if (IsWithinInfluenceRadius(zone))
            //{
            //    zone.UpdateSafety(SafetyIncrease);
            //}
        }

        // Decrease safety when a police station is removed or not operational
        public void DecreaseSafety(Zone zone)
        {
            //if (IsWithinInfluenceRadius(zone))
            //{
            //    zone.UpdateSafety(-SafetyIncrease);
            //}
        }

        // Check if a zone is within the influence radius of the police station
        //private bool IsWithinInfluenceRadius(Zone zone)
        //{
        //    return PathFinder.ManhattanDistance(this.Coordinate, zone.Coordinate) <= InfluenceRadius;
        //}
    }
}
