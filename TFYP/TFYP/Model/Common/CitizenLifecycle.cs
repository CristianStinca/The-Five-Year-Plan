using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TFYP.Model.Facilities;
using TFYP.Model.City;
using TFYP.Model.Zones;
using TFYP.Utils;
using System.Security.Policy;


namespace TFYP.Model.Common
{
    public class CitizenLifecycle
    {
        
        
        public static int StartingNrCitizens = 25;
        

        // Getting an available random Residential zone randomly
        public static Zone GetLivingPlace(GameModel gm)
        {
            List<Zone> availableResidentialZones = new List<Zone>();
            foreach (var residentialZone in gm.CityRegistry.Zones.Where(z => z.Type == EBuildable.Residential))
            {
                if (residentialZone.HasFreeCapacity() && residentialZone.IsConnected)
                {
                    availableResidentialZones.Add(residentialZone);
                }
            }
            Random rand = new Random();
            return availableResidentialZones.Count > 0 ? availableResidentialZones[rand.Next(availableResidentialZones.Count)] : null;
        }

        // Getting the closest working place (zone) from the available ones.
        public static Zone GetClosestWorkingPlace(Zone livingPlace, GameModel gm)
        {
            Zone closestZone = null;
            int closestDistance = 1000;//just starting with big integer
            foreach (var workZone in gm.CityRegistry.Zones.Where(z => z.Type == EBuildable.Industrial || z.Type == EBuildable.Service))
            {
                int distanceToWork = gm.CalculateDistanceBetweenZones(livingPlace, workZone);
                if (distanceToWork < closestDistance)
                {
                    closestDistance = distanceToWork;
                    closestZone = workZone;
                }
            }
            return closestZone;
        }

        public static List<School> GetAvailableSchools(GameModel gm)
        {
            List<School> availableSchools = new List<School>();

            foreach (School school in gm.CityRegistry.Facilities.Where(z => z.Type == EBuildable.School))
            {
                availableSchools.Add(school);
            }

            return availableSchools;
        }

        public static List<University> GetAvailableUniversities(GameModel gm)
        {
            List<University> availableUniversities = new List<University>();

            foreach (University university in gm.CityRegistry.Facilities.Where(z => z.Type == EBuildable.University))
            {
                availableUniversities.Add(university);
            }

            return availableUniversities;
        }

        private static bool AreEnoughPlacesSecondaryEducation(GameModel gm, List<School> availableSchools)
        {
            return gm.Statistics.CitizensWithSecondaryEducation(gm.CityRegistry) < GetCapacitySchools(availableSchools);
        }

        private static int GetCapacitySchools(List<School> schools)
        {
            return schools.Sum(school => school.Capacity);
        }



        private static bool AreEnoughPlacesHigherEducation(GameModel gm, List<University> availableUniversities)
        {
            return gm.Statistics.CitizensWithHigherEducation(gm.CityRegistry) < GetCapacityUniversities(availableUniversities);
        }

        private static int GetCapacityUniversities(List<University> universities)
        {
            return universities.Sum(university => university.Capacity);
        }



        // Returns a random available level of education.
        public static EducationLevel GetEducationLevel(GameModel gm, Zone livingPlace)
        {
            Random rand = new Random();
            int random = rand.Next(3); 

            List<School> availableSchools = GetAvailableSchools(gm);
            List<University> availableUniversities = GetAvailableUniversities(gm);

            switch (random)
            {
                case 1:
                    if (AreEnoughPlacesSecondaryEducation(gm, availableSchools))
                    {
                        return EducationLevel.School;
                    }
                    break;
                case 2:
                    if (AreEnoughPlacesHigherEducation(gm, availableUniversities))
                    {
                        return EducationLevel.University;
                    }
                    break;
            }

            return EducationLevel.Primary;
        }


        // Returns a young citizen, assigns it to a random residential zone and to the closest workPlace
        public static void CreateYoungCitizen(GameModel gm)
        {
            Zone livingPlace = GetLivingPlace(gm);
            Zone workPlace = GetClosestWorkingPlace(livingPlace, gm);
            Citizen newCitizen = new Citizen(workPlace, livingPlace, GetEducationLevel(gm, livingPlace));
            livingPlace.AddCitizen(newCitizen, gm);
            workPlace?.AddCitizen(newCitizen, gm);
        }

    }
}