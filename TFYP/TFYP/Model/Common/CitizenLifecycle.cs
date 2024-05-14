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
using System.Diagnostics;
using ProtoBuf;



namespace TFYP.Model.Common
{
    public class CitizenLifecycle
    {
        public static int StartingNrCitizens = Constants.StartingPopulation;
        public static int ImmigrantsCount = Constants.ComingPopulation;

        /// <summary>
        /// Retrieves an available random Residential zone from the given game model.
        /// </summary>
        /// <param name="gm">The game model instance.</param>
        /// <returns>An available random Residential zone, or null if none are available.</returns>

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

        /// <summary>
        /// Retrieves the closest working place (zone) from the available ones connected to the given living place.
        /// </summary>
        /// <param name="livingPlace">The living place zone.</param>
        /// <param name="gm">The game model instance.</param>
        /// <returns>The closest working place (zone), or null if none are found.</returns>

        public static Zone GetClosestWorkingPlace(Zone livingPlace, GameModel gm)
        {
            Zone closestZone = null;
            int closestDistance = gm.MaxDistance;
            
            List<Zone> ConnectedZones = livingPlace.GetConnectedZones();
            foreach (var workZone in ConnectedZones)
            {
                if(workZone.Type == EBuildable.Industrial || workZone.Type == EBuildable.Service)
                {
                    int distanceToWork = gm.CalculateDistanceBetweenTwo(livingPlace, workZone);
                    if (distanceToWork < closestDistance)
                    {
                        closestDistance = distanceToWork;
                        closestZone = workZone;
                    }
                }
                
            }
            return closestZone;
        }

        /// <summary>
        /// Retrieves a list of available schools from the game model.
        /// </summary>
        /// <param name="gm">The game model instance.</param>
        /// <returns>A list of available schools.</returns>

        public static List<School> GetAvailableSchools(GameModel gm)
        {
            List<School> availableSchools = new List<School>();

            foreach (School school in gm.CityRegistry.Facilities.Where(z => z.Type == EBuildable.School))
            {
                availableSchools.Add(school);
            }

            return availableSchools;
        }

        /// <summary>
        /// Retrieves a list of available universities from the game model.
        /// </summary>
        /// <param name="gm">The game model instance.</param>
        /// <returns>A list of available universities.</returns>

        public static List<University> GetAvailableUniversities(GameModel gm)
        {
            List<University> availableUniversities = new List<University>();

            foreach (University university in gm.CityRegistry.Facilities.Where(z => z.Type == EBuildable.University))
            {
                availableUniversities.Add(university);
            }

            return availableUniversities;
        }

        /// <summary>
        /// Checks if there are enough places for secondary education based on available schools.
        /// </summary>
        /// <param name="gm">The game model instance.</param>
        /// <param name="availableSchools">The list of available schools.</param>
        /// <returns>True if there are enough places for secondary education; otherwise, false.</returns>

        private static bool AreEnoughPlacesSecondaryEducation(GameModel gm, List<School> availableSchools)
        {
            return gm.Statistics.CitizensWithSecondaryEducation(gm.CityRegistry) < GetCapacitySchools(availableSchools);
        }

        /// <summary>
        /// Calculates the total capacity of all schools in the given list.
        /// </summary>
        /// <param name="schools">The list of schools.</param>
        /// <returns>The total capacity of all schools.</returns>

        private static int GetCapacitySchools(List<School> schools)
        {
            return schools.Sum(school => school.Capacity);
        }


        /// <summary>
        /// Checks if there are enough places for higher education in the city.
        /// </summary>
        /// <param name="gm">The game model containing city data.</param>
        /// <param name="availableUniversities">The list of available universities.</param>
        /// <returns>True if there are enough places for higher education; otherwise, false.</returns>

        private static bool AreEnoughPlacesHigherEducation(GameModel gm, List<University> availableUniversities)
        {
            return gm.Statistics.CitizensWithHigherEducation(gm.CityRegistry) < GetCapacityUniversities(availableUniversities);
        }

        /// <summary>
        /// Calculates the total capacity of all available universities.
        /// </summary>
        /// <param name="universities">The list of available universities.</param>
        /// <returns>The total capacity of all available universities.</returns>

        private static int GetCapacityUniversities(List<University> universities)
        {
            return universities.Sum(university => university.Capacity);
        }


        /// <summary>
        /// Returns a random available level of education based on the availability of schools and universities.
        /// </summary>
        /// <param name="gm">The GameModel instance.</param>
        /// <param name="livingPlace">The living place (zone) of the citizen.</param>
        /// <returns>A randomly selected available level of education.</returns>

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


        /// <summary>
        /// Creates a young citizen, assigns it to a random residential zone, and assigns it to the closest workplace.
        /// </summary>
        /// <param name="gm">The GameModel instance.</param>

        public static void CreateYoungCitizen(GameModel gm)
        {
            Zone livingPlace = GetLivingPlace(gm);
            populate(livingPlace, gm);
        }


        /// <summary>
        /// Populates the given residential zone with a citizen and assigns them to the closest available workplace.
        /// </summary>
        /// <param name="livingPlace">The residential zone to populate.</param>
        /// <param name="gm">The GameModel instance.</param>

        public static void populate(Zone livingPlace, GameModel gm)
        {
            Zone workPlace = null;

            if (livingPlace != null && livingPlace.IsConnected && livingPlace.HasFreeCapacity())
            {
                List<Zone> potentialWorkingZones = livingPlace.conncetedZone.Where(z => z.Type == EBuildable.Service || z.Type == EBuildable.Industrial).ToList();

                if (potentialWorkingZones.Any())
                {

                    int minDistance = gm.MaxDistance;
                    foreach (Zone workZone in potentialWorkingZones)
                    {
                        int distance = gm.CalculateDistanceBetweenTwo(livingPlace, workZone);
                        if (distance < minDistance)
                        {
                            minDistance = distance;
                            workPlace = workZone;
                        }
                    }

                }

                if (workPlace != null && workPlace.HasFreeCapacity())
                {
                    Citizen newCitizen = new Citizen(workPlace, livingPlace, GetEducationLevel(gm, livingPlace));
                    livingPlace.AddCitizen(newCitizen);
                    workPlace?.AddCitizen(newCitizen);
                    livingPlace.isInitiallyPopulated = true;
                }
            }


        }

    }
}