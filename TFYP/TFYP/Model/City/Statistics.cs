﻿using System;
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
        public int Satisfaction { get; set; }
        public Budget Budget { get; set; }

        public Statistics(Budget budget)
        {
            Budget = budget;
        }


        public int CalculateCitySatisfaction(GameModel gm, int NrCitizensLeft)
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
                financialHealthEffect -= yearsInLoan * 5; // Subtract 5 points per year in loan
            }

            
            Satisfaction = (averageZoneSatisfaction + averageCitizenSatisfaction + financialHealthEffect) / 3 - NrCitizensLeft;

            return Satisfaction;
            //Satisfaction = Math.Clamp(citySatisfaction, 0, 100);
        }

        public int GetPopulationCount(CityRegistry cityRegistry)
        {
            return cityRegistry.GetAllCitizens().Count();
        }



        public int CitizensWithSecondaryEducation(CityRegistry cityRegistry)
        {
            List <Citizen> CitizensWithSecondaryEducation = new List <Citizen>();   
            foreach (Citizen citizen in cityRegistry.GetAllCitizens())
            {
                if (citizen.EducationLevel == EducationLevel.School)
                    CitizensWithSecondaryEducation.Add(citizen);
            }
            return CitizensWithSecondaryEducation.Count();
        }

        public int CitizensWithHigherEducation(CityRegistry cityRegistry)
        {
            List<Citizen> CitizensWithHigherEducation = new List<Citizen>();
            foreach (Citizen citizen in cityRegistry.GetAllCitizens())
            {
                if (citizen.EducationLevel == EducationLevel.University)
                    CitizensWithHigherEducation.Add(citizen); 
            }
            return CitizensWithHigherEducation.Count();
        }

    }
}
