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
        public double Satisfaction { get; private set; }
        public Budget Budget { get; private set; }
        public int IndustrialZoneCount { get; private set; }
        public int ServiceZoneCount { get; private set; }
        public double BudgetEffect { get; }
        public double TaxEffect { get => 1 / (Budget.CurrentTaxRate > 0 ? Budget.CurrentTaxRate : 1); }

        public Statistics(Budget budget)
        {
            IndustrialZoneCount = 0;
            ServiceZoneCount = 0;
            Budget = budget;
        }

        public void UpdateZoneCount(CityRegistry cityRegistry)
        {
            ServiceZoneCount = cityRegistry.Zones.OfType<IndustrialZone>().Count();
            IndustrialZoneCount = cityRegistry.Zones.OfType<ServiceZone>().Count();
        }

        private void CalculateCitySatisfaction(GameModel gm)
        {
            double totalSatisfactionInZone = gm.CityRegistry.Zones.Sum(zone => zone.GetZoneSatisfaction(gm));
            int zoneCount = gm.CityRegistry.Zones.Count;
            double averageZoneSatisfaction = zoneCount > 0 ? totalSatisfactionInZone / zoneCount : 100; // Default to 100 if no zones


            double citySatisfaction = (averageZoneSatisfaction + TaxEffect + BudgetEffect + GetZoneBalance ()) / 4;
            Satisfaction = Math.Clamp(citySatisfaction, 0.0, 100.0);
        }

        public int GetPopulationCount(CityRegistry cityRegistry)
        {
            return cityRegistry.GetAllCitizens().Count();
        }

        
        /*
         For: Citizens should undertake work in the industrial and service zones in equal proportion
        */
        private double GetZoneBalance()
        {
            int diff = Math.Abs(IndustrialZoneCount - ServiceZoneCount);
            return 1.0 / (diff == 0 ? 1.0 : diff);
        }


        public int CitizensWithSecondaryEducation(CityRegistry cityRegistry)
        {
            int CitizensWithSecondaryEducation = 0;
            foreach (Citizen citizen in cityRegistry.GetAllCitizens())
            {
                if (citizen.EducationLevel == EducationLevel.School)
                    CitizensWithSecondaryEducation++;
            }
            return CitizensWithSecondaryEducation;
        }

        public int CitizensWithHigherEducation(CityRegistry cityRegistry)
        {
            int CitizensWithHigherEducation = 0;
            foreach (Citizen citizen in cityRegistry.GetAllCitizens())
            {
                if (citizen.EducationLevel == EducationLevel.University)
                    CitizensWithHigherEducation++;
            }
            return CitizensWithHigherEducation;
        }

    }
}
