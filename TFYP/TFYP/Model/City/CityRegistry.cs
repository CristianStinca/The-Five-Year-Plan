using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TFYP.Model.Facilities;
using TFYP.Model.Zones;
using TFYP.Model.Common;
using Microsoft.Xna.Framework;
using ProtoBuf;

namespace TFYP.Model.City
{
    [ProtoContract]
    [Serializable]
    public class CityRegistry
    {
        [ProtoMember(1)]
        public Statistics Statistics { get;  set; }
        [ProtoMember(2)]
        public List<Zone> Zones { get;  set; }
        [ProtoMember(3)]
        public List<Facility> Facilities { get;  set; }
        [ProtoMember(4)]
        public int SchoolCount { get;  set; }
        [ProtoMember(5)]
        public int UniversityCount { get;  set; }
        [ProtoMember(6)]
        public int PoliceCount { get; set; }
        [ProtoMember(7)]
        public int StadiumCount { get; set; }
        [ProtoMember(8)]
        public int RoadCount { get;  set; }

        public CityRegistry(Statistics statistics)
        {
            Statistics = statistics;
            Zones = new List<Zone>();
            Facilities = new List<Facility>();
            SchoolCount = 0;
            UniversityCount = 0;
            PoliceCount = 0;
            StadiumCount = 0;
            RoadCount = 0;
        }
        public void IncPoliceCount()
        {
            PoliceCount += 1;
        }
        public void DecPoliceCount()
        {
            PoliceCount -= 1;
        }

        public void IncStadiumCount()
        {
            StadiumCount += 1;
        }
        public void DecStadiumCount()
        {
            StadiumCount -= 1;
        }

        public void IncSchoolCount()
        {
            SchoolCount += 1;
        }
        public void DecSchoolCount()
        {
            SchoolCount -= 1;
        }

        public void IncUniversityCount()
        {
            UniversityCount += 1;
        }
        public void DecUniversityCount()
        {
            UniversityCount -= 1;
        }
        public void IncRoadCount()
        {
            RoadCount += 1;
        }
        public void DecRoadCount()
        {
            RoadCount -= 1;
        }
        // zones
        public void AddZone(Zone zone)
        {
            if (zone == null) throw new System.ArgumentNullException(nameof(zone));
            Zones.Add(zone);
        }
        public void RemoveZone(Zone zone)
        {
            bool removed = Zones.Remove(zone);
        }
        public IEnumerable<Zone> GetAllZones()
        {
            return Zones;
        }

        // facilities
        public void AddFacility(Facility facility)
        {
            if (facility == null) throw new ArgumentNullException(nameof(facility));
            Facilities.Add(facility);
        }
        public void RemoveFacility(Facility facility)
        {
            Facilities.Remove(facility);
        }


        /// <summary>
        /// Retrieves all active citizens residing in residential zones.
        /// </summary>
        /// <returns>A list containing all active citizens.</returns>

        public List<Citizen> GetAllCitizens()
        {
            List<Citizen> allCitizens = new List<Citizen>();
            

            foreach (var residentialZone in Zones.Where(z => z.Type == EBuildable.Residential))
            {
                foreach (Citizen c in residentialZone.citizens)
                {
                    if (c.IsActive)
                    {
                        allCitizens.Add(c);
                    }
                }
            }

            return allCitizens;
        }

        /// <summary>
        /// Sets the balance of the city's budget to the specified amount at the given time.
        /// </summary>
        /// <param name="amount">The new balance amount.</param>
        /// <param name="time">The time at which the balance is set.</param>

        public void SetBalance(double amount, DateTime time) 
        {
            Statistics.Budget.UpdateBalance(amount, time);
        }



        //Next two methods are to check for conditions, if new citizens are eligible to move in city

        /// <summary>
        /// Checks if there are free workplaces near residential zones within a specified search radius.
        /// </summary>
        /// <param name="gm">The GameModel instance.</param>
        /// <returns>True if there are free workplaces near residential zones within the search radius; otherwise, false.</returns>


        public bool GetFreeWorkplacesNearResidentialZones(GameModel gm)
        {
            int workplaceSearchRadius = (int)(gm.MaxDistance/1.2);

            foreach (var residentialZone in Zones.Where(z => z.Type == EBuildable.Residential))
            {
                foreach (var workZone in residentialZone.conncetedZone.Where(z => z.Type == EBuildable.Industrial || z.Type == EBuildable.Service))
                {

                    if (gm.CalculateDistanceBetweenTwo(residentialZone, workZone) <= workplaceSearchRadius && workZone.HasFreeCapacity() && residentialZone.HasFreeCapacity())
                    {
                        return true;
                    }

                }
            }
            return false;
        }



        /// <summary>
        /// Checks whether there are industrial buildings near residential zones within a certain proximity radius.
        /// </summary>
        /// <param name="gm">The GameModel instance.</param>
        /// <returns>True if there are no industrial buildings near residential zones within the specified proximity radius; otherwise, false.</returns>
        public bool NoIndustriesNearResidentialZones(GameModel gm)
        {
            int industryProximityRadius = (int)(gm.MaxDistance / 20); 


            foreach (var residentialZone in Zones.Where(z => z.Type == EBuildable.Residential))
            {
                foreach (var industrial in residentialZone.conncetedZone.Where(z => z.Type == EBuildable.Industrial))
                {

                    if (gm.CalculateDistanceBetweenTwo(residentialZone, industrial) <= industryProximityRadius)
                    {
                        return false;
                    }

                }
            }
            return true;
            
        }
    }
}
