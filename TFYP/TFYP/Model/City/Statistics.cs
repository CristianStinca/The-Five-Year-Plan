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
        public int Population { get; set; }

        public int Capacity { get; private set; }
        private int satisfaction;
        public Budget Budget { get; private set; }
        public int IndustrialZoneCount { get; private set; }
        public int ServiceZoneCount { get; private set; }

        // Volatile is used for concurrency
        
        public Statistics(Budget budget)
        {
            Budget = budget;
            IndustrialZoneCount = 0;
            ServiceZoneCount = 0;
            satisfaction = 50; // I think we ust start with avg value
        }
        public void UpdateFinancialStats() { }
        public void UpdateZoneCount(CityRegistry cityRegistry)
        {
            ServiceZoneCount = cityRegistry.Zones.OfType<IndustrialZone>().Count();
            IndustrialZoneCount = cityRegistry.Zones.OfType<ServiceZone>().Count();
        }
        public void IncreaseZoneCOunt() { }
        public int Satisfaction
        {
            get => Math.Clamp(satisfaction, 0, 100); // Ensure satisfaction is within 0-100
            set => satisfaction = value;
        }
        public void UpdateCityStatistics(CityRegistry cityRegistry)
        {
            //UpdateZoneCount(cityRegistry);
            //Population = cityRegistry.GetAllCitizens().Count;
            //CitySatisfaction = CalculateCitySatisfaction(cityRegistry);
        }

        private double CalculateCitySatisfaction(CityRegistry cityRegistry)
        {
            var currentSatisfaction = GetTaxEffect()
                                    + GetZoneBalance()
                                    + GetBudgetEffect(DateTime.Now);

            var zones = cityRegistry.GetAllZones();
            if (zones.Any())
            {
                //currentSatisfaction += zones.Average(z => z.GetZoneSatisfaction());
                // TODO satisfaction logic from zone
                currentSatisfaction += 0;
            }
            return currentSatisfaction;
        }
        private double GetTaxEffect()
        {
            return 1 / (Budget.TaxRate > 0 ? Budget.TaxRate : 1);
        }

        private double GetZoneBalance()
        {
            int diff = Math.Abs(IndustrialZoneCount - ServiceZoneCount);
            return 1.0 / (diff == 0 ? 1.0 : diff);
        }

        private double GetBudgetEffect(DateTime now)
        {
            return 0; // change
        }
        private void UpdateNrZones(CityRegistry cityRegistry)
        {
            //ServiceZoneCount = cityRegistry.GetAllZones().OfType<CommercialZone>().Count();
            //IndutstrialZoneCount = cityRegistry.GetAllZones().OfType<IndustrialZone>().Count();
        }
    }   
}
