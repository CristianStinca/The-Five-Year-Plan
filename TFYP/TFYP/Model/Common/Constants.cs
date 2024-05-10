using System.Threading;

namespace TFYP.Model.Common
{
    public static class Constants 
    {
        // BUILD COSTS
        public const int ServiceZoneBuildCost = 1000;
        public const int ResidentialZoneBuildCost = 1000;
        public const int IndustrialZoneBuildCost = 1000;
        public const int StadiumBuildCost = 1500;
        public const int PoliceStationBuildCost = 1000;
        public const int RoadBuildCost = 500;
        public const int SchoolBuildCost = 2000;
        public const int UniversityBuildCost = 3000;
        // HEAL COST FOR 10%
        public const int HealZone = 50;

        // CAPACITIES OF ZONES
        public const int ServiceZoneCapacity = 25;
        public const int ResidentialZoneCapacity = 50;
        public const int IndustrialZoneCapacity = 25;

        // MAINTENANCE FEES
        public const int ServiceZoneMaintenanceCost = 1000;
        public const int ResidentialZoneMaintenanceCost = 1000;
        public const int IndustrialZoneMaintenanceCost = 1000;
        public const int StadiumMaintenanceFee = 2000;
        public const int PoliceStationMaintenanceFee = 2000;
        public const int RoadMaintenanceFee = 500;
        public const int SchoolMaintenanceFee = 2500;
        public const int UniversityMaintenanceFee = 3000;

        //FOR REMOVING
        public const int ZoneReimbursement = 500;
        public const int FacilityReimbursement = 750;
        public const int RoadReimbursement = 250;
        public const int ZoneMaintenanceCost = 1000;
        public const int FacilityMaintenanceCost = 1250;
        public const int RoadMaintenanceCost = 500;
        

        //TO DO - Should be added constants for Disaster


        // GENERAL CONSTANTS
        public const int CityBaseTax = 100;
        public const float CityBaseTaxRate = 0.4f;
        public const int InitialBalance = 10000;


        // TIME in days
        public const int SchoolGraduationTime = 1460;//4 years
        public const int UniversityGraduationTime = 1095;//3 years

        public const int ResidentialZoneBuildTime = 10;
        public const int ServiceZoneBuildTime = 10;
        public const int IndustrialBuildTime = 10;
        
        public const int CitizenLeavingSatisfaction = 10;
        public const int NewCitizenComingSatisfaction = 85;
        public const int GameOverSatisfaction = 30;

        // LOCATION EFFECT
        public const int IndustrialEffectRadius = 10;
        public const int ServiceEffectRadius = 10;
        public const int ResidentialEffectRadius = 0;
        public const int StadiumEffectRadius = 10;
        public const int PoliceStationEffectRadius = 10;

        // MAXIMUM NUMBER OF BUILDABLES
        public const int MaxUniversityCount = 3;
        public const int MaxSchoolCount = 3;
        public const int MaxPoliceCount = 6;
        public const int MaxStadiumCount = 4;

        public const int MaxResidentialCount = 30;
        public const int MaxIndustrialCount = 15;
        public const int MaxServiceCount = 15;

        public const int MaxRoadCount = 50;

    }
}