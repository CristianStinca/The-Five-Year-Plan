using System.Threading;

namespace TFYP.Model.Common
{
    public static class Constants // Leaving constants for now, can be changed later
    {
        // BUILD COSTS
        public const int ServiceZoneBuildCost = 500;
        public const int ResidentialZoneBuildCost = 500;
        public const int IndustrialZoneBuildCost = 500;
        public const int StadiumBuildCost = 500;
        public const int PoliceStationBuildCost = 500;
        public const int RoadBuildCost = 500;
        public const int SchoolBuildCost = 500;
        public const int UniversityBuildCost = 500;
        // HEAL COST FOR 10%
        public const int HealZone = 50;

        // CAPACITIES OF ZONES
        public const int ServiceZoneCapacity = 200;
        public const int ResidentialZoneCapacity = 200;
        public const int IndustrialZoneCapacity = 200;

        // MAINTENANCE FEES
        public const int ServiceZoneMaintenanceCost = 500;
        public const int ResidentialZoneMaintenanceCost = 500;
        public const int IndustrialZoneMaintenanceCost = 500;
        public const int StadiumMaintenanceFee = 100;
        public const int PoliceStationMaintenanceFee = 100;
        public const int RoadMaintenanceFee = 100;
        public const int SchoolMaintenanceFee = 100;
        public const int UniversityMaintenanceFee = 100;

        // LOCATION EFFECT
        public const int IndustrialEffectRadius = 10;
        public const int ServiceEffectRadius = 10;
        public const int ResidentialEffectRadius = 0;
        public const int StadiumEffectRadius = 10;
        public const int PoliceStationEffectRadius = 10;


        //TO DO - Should be added constants for Disaster


        // GENERAL CONSTANTS
        public const int CityBaseTax = 750;
        public const int InitialBalance = 1000000;


        // TIME in days
        public const int SchoolGraduationTime = 1100;
        public const int UniversityGraduationTime = 700;
        public const int ResidentialZoneBuildTime = 5;
        public const int ServiceZoneBuildTime = 5;
        public const int IndustrialBuildTime = 5;
        public const int PoliceConstructionTime = 5;
        public const int SchoolConstructionTime = 5;
        public const int UniversityConstructionTime = 5;
        public const int StadiumConstructionTime = 5;


        //SATTISFACTION WEIGHTS (for citizen)
        public const float EducationWeight = 0.3f; // 30% of total satisfaction
        public const float EmploymentWeight = 0.2f; // 20% of total satisfaction
        public const float DistanceWeight = 0.2f; // 20% of total satisfaction
        public const float TaxWeight = 0.1f; // 10% of total satisfaction
        public const float ZoneSatisfactionWeight = 0.2f; // 20% of total satisfaction

        public const float baseZoneSatisfaction = 60.0f;


        public const int SatisfactionLowerLimit = 25;
        public const int SatisfactionUpperLimit = 75;



    }
}