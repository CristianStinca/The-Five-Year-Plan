using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TFYP.Model.Facilities;
using TFYP.Model.Common;
using TFYP.Model.Zones;

namespace TFYP.Model.City
{
    public class Statistics
    {
        public int Capacity { get; private set; }
        private double satisfaction;
        public Budget Budget { get; private set; }
        public int IndustrialZoneCount { get; private set; }
        public int ServiceZoneCount { get; private set; }
        public double BudgetEffect { get; }
        public double TaxEffect { get => 1 / (Budget.TaxRate > 0 ? Budget.TaxRate : 1); }

        public Statistics(Budget budget)
        {
            satisfaction = 0;
            IndustrialZoneCount = 0;
            ServiceZoneCount = 0;
            Budget = budget;
        }
        public double ZoneBalance()
        {
            int difference = Math.Abs(ServiceZoneCount - IndustrialZoneCount);
            if(difference == 0)
            {
                return 1.0;
            }
            else
            {
                return 1.0 / difference;
            }
        }
        public void UpdateZoneCount(CityRegistry cityRegistry)
        {
            ServiceZoneCount = cityRegistry.Zones.OfType<IndustrialZone>().Count();
            IndustrialZoneCount = cityRegistry.Zones.OfType<ServiceZone>().Count();
        }
        
        public double Satisfaction
        {
            get => Math.Clamp(satisfaction, 0.0, 100.0); // ensure satisfaction is within 0-100
            private set => satisfaction = value;
        }
        public void CitySatisfaction(GameModel gm)
        {
            double totalSatisfactionInZone = gm.CityRegistry.Zones.Sum(zone => zone.GetOverallSatisfaction());
            int zoneCount = (gm.CityRegistry.Zones.Count());
            satisfaction = zoneCount == 0 ? 60 + (CalculateCitySatisfaction()) : (double)totalSatisfactionInZone / zoneCount;
        }
        private double CalculateCitySatisfaction()
        {
            var currentSatisfaction = TaxEffect + GetZoneBalance() + BudgetEffect;

            return currentSatisfaction;
        }

        public int GetPopulationCount(CityRegistry cityRegistry)
        {
            return cityRegistry.GetAllCitizens().Count();
        }

        private double GetZoneBalance()
        {
            int diff = Math.Abs(IndustrialZoneCount - ServiceZoneCount);
            return 1.0 / (diff == 0 ? 1.0 : diff);
        }

    }   
}
