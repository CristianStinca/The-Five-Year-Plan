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
        public List<Citizen> Citizens { get; private set; }
        public List<Facility> Facilities { get; private set; }
        
        public CityRegistry(Statistics statistics)
        {
            Statistics = statistics;
            Zones = new List<Zone>();
            Citizens = new List<Citizen>();
            Facilities = new List<Facility>();
        }

        // zones
        public void AddZone(Zone zone)
        {
            if (zone == null) throw new System.ArgumentNullException(nameof(zone));
            Zones.Add(zone);
            Statistics.UpdateZoneCount(this);
        }
        public void RemoveZone(Zone zone)
        {
            bool removed = Zones.Remove(zone);
            if (removed) Statistics.UpdateZoneCount(this);
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
            foreach (var zone in Zones)
            {
                if (zone is ResidentialZone residentialZone) // Checking if the zone is a ResidentialZone
                {
                    // Filter to include only active citizens
                    allCitizens.AddRange(residentialZone.GetCitizens().Where(citizen => citizen.IsActive));
                }
            }
            return allCitizens;
        }


        // budget
        public void SetBalance(double amount, DateTime time) // adds value (if negative, subtracts) from the balance
        {
            Statistics.Budget.UpdateBalance(amount, time);
        }

        public void ChangeTax(double newRate)
        {
            Statistics.Budget.UpdateTax(newRate);
        }


        //These 2 methods are to check for conditions, if new citizens are eligible to move in city

        public bool GetFreeWorkplacesNearResidentialZones()
        {
            int workplaceSearchRadius = 5;
            foreach (var residentialZone in Zones.Where(z => z.Type == EBuildable.Residential))
            {
                foreach (var residentialPos in residentialZone.Coor)
                {
                    foreach (var workZone in Zones.Where(z => z.Type == EBuildable.Industrial || z.Type == EBuildable.Service))
                    {
                        foreach (var workPos in workZone.Coor)
                        {
                            double distance = Vector2.Distance(residentialPos, workPos);
                            if (distance <= workplaceSearchRadius && workZone.HasFreeCapacity())
                            {
                                return true;
                            }
                        }
                    }
                }
            }
            return false;
        }


        // Method to check the absence of industrial buildings near residential zones
        public bool NoIndustriesNearResidentialZones()
        {
            int industryProximityRadius = 5;//ეს შეიძლება შესაცვლელი გახდეს
            foreach (var residentialZone in Zones.Where(z => z.Type == EBuildable.Residential))
            {
                foreach (var residentialPos in residentialZone.Coor)
                {
                    foreach (var industrialZone in Zones.Where(z => z.Type == EBuildable.Industrial))
                    {
                        foreach (var industrialPos in industrialZone.Coor)
                        {
                            double distance = Vector2.Distance(residentialPos, industrialPos);
                            if (distance <= industryProximityRadius)
                            {
                                return false;
                            }
                        }
                    }
                }
            }
            return true;
        }



    }
}
