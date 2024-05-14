using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TFYP.Model.Facilities;
using TFYP.Model.Common;
using TFYP.Model.Zones;
using ProtoBuf;

namespace TFYP.Model.City
{
    [ProtoContract]
    [Serializable]
    public class Statistics
    {
        [ProtoMember(1)]
        public int Satisfaction { get;  set; }
        [ProtoMember(1)]
        public Budget Budget { get;  set; }
        public Statistics() { }
        public Statistics(Budget budget)
        {
            Budget = budget;
        }

        /// <summary>
        /// Calculates the overall satisfaction level of the city based on zone satisfaction, citizen satisfaction, and financial health.
        /// </summary>
        /// <param name="gm">The GameModel object representing the city.</param>
        /// <param name="NrCitizensLeft">The number of citizens left in the city.</param>
        /// <returns>The overall satisfaction level of the city, clamped between 0 and 100.</returns>

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


            return Math.Clamp(Satisfaction, 0, 100);
        }



        /// <summary>
        /// Counts the number of citizens with secondary education in the city.
        /// </summary>
        /// <param name="cityRegistry">The CityRegistry object containing information about the city.</param>
        /// <returns>The number of citizens with secondary education.</returns>

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
        /// <summary>
        /// Counts the number of citizens with higher education in the city.
        /// </summary>
        /// <param name="cityRegistry">The CityRegistry object containing information about the city.</param>
        /// <returns>The number of citizens with higher education.</returns>

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
