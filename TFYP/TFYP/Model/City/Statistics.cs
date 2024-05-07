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
        public int Satisfaction { get; private set; }
        public Budget Budget { get; private set; }

        public Statistics(Budget budget)
        {
            Budget = budget;
        }


        public void CalculateCitySatisfaction(GameModel gm)
        {

            // Calculate average zone satisfaction
            int totalSatisfactionInZone = gm.CityRegistry.Zones.Sum(zone => zone.GetZoneSatisfaction(gm));
            int zoneCount = gm.CityRegistry.Zones.Count;
            int averageZoneSatisfaction = zoneCount > 0 ? totalSatisfactionInZone / zoneCount : 100;

            // Calculate average citizen satisfaction
            int totalCitizenSatisfaction = gm.CityRegistry.GetAllCitizens().Sum(citizen => citizen.Satisfaction);
            int citizenCount = gm.CityRegistry.GetAllCitizens().Count;
            int averageCitizenSatisfaction = citizenCount > 0 ? totalCitizenSatisfaction / citizenCount : 100;

            int financialHealthEffect = 100;
            int yearsInLoan = Budget.YearsOfBeingInLoan(gm.GameTime);
            if (yearsInLoan > 0)
            {
                // Decrease satisfaction based on the number of years in loan
                financialHealthEffect -= yearsInLoan * 10; // Subtract 10 points per year in loan
            }


            Satisfaction = (averageZoneSatisfaction + averageCitizenSatisfaction + financialHealthEffect) / 3;

            //Satisfaction = Math.Clamp(citySatisfaction, 0, 100);
        }

        public int GetPopulationCount(CityRegistry cityRegistry)
        {
            return cityRegistry.GetAllCitizens().Count();
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
