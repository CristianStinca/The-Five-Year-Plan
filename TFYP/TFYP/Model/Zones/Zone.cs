using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TFYP.Model.Facilities;
using TFYP.Model.Common;
using TFYP.Model.City;
using Microsoft.Xna.Framework;
using System.Xml.Serialization;

namespace TFYP.Model.Zones
{
    public enum ZoneLevel
    {
        One,
        Two,
        Three
    }


    [Serializable]
    public class Zone : Buildable
    {

        public float Health {  get; private set; } // Health will be probably changed later, so far it is for the disaster and is representing percentage (1-100)
        // health will help us to calculate the cost of the damage which will be OneTimeCost * (Health/100)
        // Tracking the citizens within the zone
        private bool canStartBuilding;
        private List<Citizen> citizens = new List<Citizen>();
        //public bool IsConnected { get; protected set; } // maybe we will need this after building roads
        public ZoneLevel Level { get; private set; } 

        public Zone(EBuildable type, List<Vector2> coor, int influenceRadius, int timeToBuild, int capacity, int maintenanceCost, int buildCost)
            : base(coor, type, buildCost, maintenanceCost, influenceRadius, capacity, timeToBuild)
        {
            Health = 100;
            canStartBuilding = false;
            Level = ZoneLevel.One;
        }

        //TO DO: Need to implement method for finding paths and set Connected for every zone


        public void RemoveCitizen(Citizen citizen, GameModel _gameModel)
        {
            citizens.Remove(citizen);
            //Statistics.Population -= 1;
            //_gameModel.CityStatistics.SetCitySatisfaction(_gameModel);
        }




        public float GetIncome(Budget budget)
        {
            return citizens.Where(c => c.IsActive).Sum(c => c.TaxAmount(budget));
        }


        public void SetHealth(float health)
        {
            // this will be called in disasters at first and will decrease the HP of the zone
            // then will be increased again to 100% after user repairs it, so RepairZone function is needed
            Health = health;
        }
        public virtual List<Citizen> GetCitizens()
        {
            return citizens;
        }

        public override void startBuilding() {
            this.canStartBuilding = true;
        }

        public override bool checkToBuild()
        {
            return this.canStartBuilding;
        }

        public override void stopBuilding() {
            this.canStartBuilding = false;
        }


        // Method to calculate effects based on distance
        private double CalculateDistanceEffect(double maxEffect, double distance, double decayRate)
        {
            // Effect diminishes as distance increases, decayRate controls how quickly the effect diminishes
            return Math.Max(0, maxEffect - (distance * decayRate));
        }

        public double GetZoneSatisfaction(GameModel gm)
        {
            // Calculate effects based on the distance to the nearest police station, stadium, and industrial area
            double mindistPolice=this.Coor.Min(s => gm.GetDistanceToNearestPoliceStation(s));
            double policeEffect = CalculateDistanceEffect(100, mindistPolice, 0.5);
            double mindistStad = this.Coor.Min(s => gm.GetDistanceToNearestStadium(s));
            double stadiumEffect = CalculateDistanceEffect(80, mindistStad, 0.3);
            var mindistInd = this.Coor.Min(s=>gm.GetDistanceToNearestIndustrialArea(s));
            double industrialEffect = -CalculateDistanceEffect(50, mindistInd, 0.7);

            double freeWorkplaceEffect = (Capacity - citizens.Count) * 10; // more free capacity increases satisfaction

            double citizenSatisfaction = citizens.Any(c => c.IsActive)
                ? citizens.Where(c => c.IsActive).Average(c => c.Satisfaction)
                : 0;

            double totalSatisfaction = Constants.baseZoneSatisfaction +
                                       policeEffect +
                                       stadiumEffect +
                                       industrialEffect +
                                       freeWorkplaceEffect +
                                       citizenSatisfaction;

            return Math.Clamp(totalSatisfaction, 0, 100);

        }

        public void AddCitizen(Citizen citizen, GameModel _gameModel)
        {
            if (citizen == null)
                throw new ArgumentNullException(nameof(citizen), "Citizen cannot be null.");

            if (!citizen.IsActive)
                throw new InvalidOperationException("Cannot add an inactive citizen.");

            if (citizens.Count >= Capacity)
                throw new InvalidOperationException("Cannot add new citizen; zone capacity reached.");

            if (citizens.Contains(citizen))
                throw new InvalidOperationException("Citizen is already in the zone.");

            citizens.Add(citizen);

            //this.statistics.Population += 1; 

        }

        public double UpgradeZone()
        {
            double upgradeCost = 0;
            if (Level == ZoneLevel.Three)
            {
                throw new InvalidOperationException("Maximum level reached. No further upgrades possible.");
            }
            else if(Level == ZoneLevel.One)
            {
                Level = ZoneLevel.Two;
                // Increase by 20%
                MaintenanceCost = (int)Math.Round(MaintenanceCost * 1.20);
                Capacity = (int)Math.Round(Capacity * 1.20);
                if (Type == EBuildable.Industrial)
                {
                    InfluenceRadius = (int)Math.Round(InfluenceRadius * 1.20);
                }
                upgradeCost = ConstructionCost * 1.20;
            }
            else
            {
                Level = ZoneLevel.Three;
                // Increase by 40%
                MaintenanceCost = (int)Math.Round(MaintenanceCost * 1.40);
                Capacity = (int)Math.Round(Capacity * 1.40);
                if (Type == EBuildable.Industrial)
                {
                    InfluenceRadius = (int)Math.Round(InfluenceRadius * 1.40);
                }
                upgradeCost = ConstructionCost * 1.40;
            }

            return upgradeCost;
        }




    }
}
