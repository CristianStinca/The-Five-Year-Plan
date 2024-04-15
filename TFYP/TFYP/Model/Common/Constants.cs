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
        public const float IndustrialEffectRadius = 10;
        public const float ServiceEffectRadius = 10;
        public const float ResidentialEffectRadius = 0;
        public const float StadiumEffectRadius = 10;
        public const float PoliceStationEffectRadius = 10;


        //TO DO - Should be added constants for Disaster


        // GENERAL CONSTANTS
        public const int TaxRate = 100;
        public const int InitialBalance = 1000000;


        // TIME
        public const int SchoolGraduationTime = 1100; // in days
        public const int UniversityGraduationTime = 700; // in days

    }
}