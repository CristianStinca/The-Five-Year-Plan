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

namespace TFYP.Model.City
{
    public class CityRegistry
    {
        
        public Statistics Statistics { get; private set; }
        public List<Zone> Zones { get; private set; }
        public List<Facility> Facilities { get; private set; }
        public int SchoolCount { get; private set; }
        public int UniversityCount { get; private set; }
        public int PoliceCount { get; private set; }
        public int StadiumCount { get; private set; }
        public int RoadCount { get; private set; }
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

        
        // citizens
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
        

        // budget
        public void SetBalance(double amount, DateTime time) // adds value (if negative, subtracts) from the balance
        {
            Statistics.Budget.UpdateBalance(amount, time);
        }



        //These 2 methods are to check for conditions, if new citizens are eligible to move in city

        public bool GetFreeWorkplacesNearResidentialZones(GameModel gm)
        {
            int workplaceSearchRadius = (int)(gm.MaxDistance/2);

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


        // Method to check the absence of industrial buildings near residential zones
        public bool NoIndustriesNearResidentialZones(GameModel gm)
        {
            int industryProximityRadius = (int)(gm.MaxDistance / 5); 


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
