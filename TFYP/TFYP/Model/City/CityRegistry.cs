using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TFYP.Model.Facilities;
using TFYP.Model.Zones;
using TFYP.Model.Common;

namespace TFYP.Model.City
{
    public class CityRegistry
    {
        public Statistics Statistics { get; private set; }
        public List<Zone> Zones { get; private set; }
        public List<Facility> Facilities { get; private set; }
        
        public CityRegistry(Statistics statistics)
        {
            Statistics = statistics;
            Zones = new List<Zone>();
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
                    allCitizens.AddRange(residentialZone.GetCitizens());
                }
            }
            return allCitizens;
        }

        // budget
        public void SetBalance(double amount) // adds value (if negative, subtracts) from the balance
        {
            Statistics.Budget.UpdateBalance(amount);
        }

        public void ChangeTaxRate(double newRate)
        {
            Statistics.Budget.SetTaxRate(newRate);
        }


    }
}
