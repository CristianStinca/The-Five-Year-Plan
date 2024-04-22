using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TFYP.Model.Facilities;
using TFYP.Model.City;
using TFYP.Model.Zones;
using TFYP.Utils;

namespace TFYP.Model.Common
{
    internal class CitizenLifecycle
    {
        /*
        public static int StartingNrCitizens = 20;

        // Getting an available random Residential zone.
        public static Zone GetLivingPlace(GameModel gm)
        {
            List<Zone> availableResidentialZones = new List<Zone>();
            foreach (var buildable in gm.ZoneBuildable)
            {
                Zone zone = (Zone)buildable;
                if (zone.BuildableType == BuildableType.Residential &&
                    zone.Statistics.Population < zone.Capacity && zone.IsConnected)
                {
                    availableResidentialZones.Add(zone);
                }
            }
            Random rand = new Random();
            return availableResidentialZones.Count > 0 ? availableResidentialZones[rand.Next(availableResidentialZones.Count)] : null;
        }

        // Getting the closest working place (zone) from the available ones.
        public static Zone GetClosestWorkingPlace(List<Zone> availableWorkingZones, Zone livingPlace, GameModel gm)
        {
            Zone closestZone = null;
            int closestDistance = 100;
            foreach (var zone in availableWorkingZones)
            {
                int distanceToWork = Citizen.GetDistanceLiveWork(gm, zone, livingPlace);
                if (distanceToWork < closestDistance)
                {
                    closestDistance = distanceToWork;
                    closestZone = zone;
                }
            }
            return closestZone;
        }

        // Returns a random available level of education.
        public static LevelOfEducation GetEducationLevel(GameModel gm, Zone livingPlace)
        {
            Random rand = new Random();
            int random = rand.Next(3);

            List<School> availableSchools = GetAvailableSchools(gm, livingPlace);
            List<University> availableUniversities = GetAvailableUniversities(gm, livingPlace);

            if (random == 1 && AreEnoughPlacesSecondaryEducation(gm, availableSchools))
            {
                return LevelOfEducation.School;
            }

            if (random == 2 && AreEnoughPlacesHigherEducation(gm, availableUniversities))
            {
                return LevelOfEducation.University;
            }

            return LevelOfEducation.Primary;
        }

        // Returns a young citizen, assigns it to a random residential zone (if available), and to the closest workPlace (if available)
        public static void CreateYoungCitizen(GameModel gm)
        {
            Zone livingPlace = GetLivingPlace(gm);
            if (livingPlace == null)
            {
                return;
            }
            Zone workPlace = GetWorkingPlace(gm, livingPlace);
            Citizen newCitizen = new Citizen(workPlace, livingPlace, GetEducationLevel(gm, livingPlace));
            livingPlace.AddCitizen(newCitizen);
            workPlace?.AddCitizen(newCitizen);
        }

        // Creates a young citizen with workPlace and livingPlace given.
        public static void CreateYoungCitizen(GameModel gm, Zone workPlace, Zone livingPlace)
        {
            Citizen newCitizen = new Citizen(workPlace, livingPlace, GetEducationLevel(gm, livingPlace));
            livingPlace.AddCitizen(newCitizen);
            workPlace?.AddCitizen(newCitizen);
        }

        // Gets a working place that is connects to the given living place.
        public static Zone GetWorkingPlace(GameModel gm, Zone livingPlace)
        {
            List<Zone> availableWorkingZones = new List<Zone>();
            foreach (var buildable in gm.ZoneBuildable)
            {
                Zone zone = (Zone)buildable;
                int distanceLiveWork = PathFinder.ManhattanDistance(gm.Map, zone, livingPlace);
                if ((zone.BuildableType == BuildableType.Industrial || zone.BuildableType == BuildableType.Commercial) &&
                    zone.Statistics.Population < zone.Capacity && distanceLiveWork != -1)
                {
                    availableWorkingZones.Add(zone);
                }
            }
            return GetClosestWorkingPlace(availableWorkingZones, livingPlace, gm);
        }

        public static List<School> GetAvailableSchools(GameModel gm, Zone livingPlace)
        {
            List<School> availableSchools = new List<School>();
            foreach (Buildable buildable in gm.AllBuildable)
            {
                if (PathFinder.ManhattanDistance(gm.Map, buildable, livingPlace) != -1)
                {
                    if (buildable is School school)
                    {
                        availableSchools.Add(school);
                    }
                }
            }
            return availableSchools;
        }

        private static bool AreEnoughPlacesSecondaryEducation(GameModel gm, List<School> availableSchools)
        {
            return gm.CityStatistics.GetNrCitizenSecondaryEducation(gm.CityRegistry) < GetCapacitySchools(availableSchools);
        }

        private static int GetCapacitySchools(List<School> schools)
        {
            return schools.Sum(school => school.Capacity);
        }

        public static List<University> GetAvailableUniversities(GameModel gm, Zone livingPlace)
        {
            List<University> availableUniversities = new List<University>();
            foreach (Buildable buildable in gm.AllBuildable)
            {
                if (PathFinder.ManhattanDistance(gm.Map, buildable, livingPlace) != -1)
                {
                    if (buildable is University university)
                    {
                        availableUniversities.Add(university);
                    }
                }
            }
            return availableUniversities;
        }

        private static bool AreEnoughPlacesHigherEducation(GameModel gm, List<University> availableUniversities)
        {
            return gm.CitStatistics.GetNrCitizenHigherEducation(gm.CityRegistry) < GetCapacityUniversities(availableUniversities);
        }

        private static int GetCapacityUniversities(List<University> universities)
        {
            return universities.Sum(university => university.Capacity);
        }
        */
    }
}
