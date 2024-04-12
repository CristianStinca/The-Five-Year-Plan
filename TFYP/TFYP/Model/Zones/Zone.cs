using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TFYP.Model.Facilities;
using TFYP.Model.Common;
using TFYP.Model.City;

namespace TFYP.Model.Zones
{
    [Serializable]
    public class Zone : Buildable
    {
        //*removed level
        //*might be needed to store x and y coordinates of the zone as attributes
        //*might be needed to store height and width of the zone for dimensions

        public float Health {  get; private set; } // Health will be probably changed later, so far it is for the disaster and is representing percentage (1-100)
        // health will help us to calculate the cost of the damage which will be OneTimeCost * (Health/100)
        public List<Citizen> citizens;// Tracking the citizens within the zone
        public EBuildable type { get; }
        public float effectRadius { get; private set; }
        public double TimeToBuild { get; private set; }
        public int Capacity { get; private set; }
        public int MaintenanceCost { get; private set; }
        public int BuildCost { get; private set; }
        //public bool IsConnected { get; protected set; } // maybe we will need this after building roads

        public Zone(EBuildable type, float effectRadius, double timeToBuild, int capacity, int maintenanceCost, int buildCost)
        { // Initialize or assign
            this.citizens = new List<Citizen>();
            this.type = type;
            this.effectRadius = effectRadius;
            TimeToBuild = timeToBuild;
            Capacity = capacity;
            MaintenanceCost = maintenanceCost;
            BuildCost = buildCost;
            Health = 100;
        }

        //This constructor is to use temporarily, will be deleted later!

        public Zone(EBuildable type)
        {
            this.type = type; 
            this.citizens = new List<Citizen>();
            this.effectRadius = 0f; 
            this.TimeToBuild = 0; 
            this.Capacity = 0; 
            this.MaintenanceCost = 0; 
            this.BuildCost = 0; 
        }

        //TO DO: Need to implement method for finding paths and set Connected for every zone

        public void AddCitizen(Citizen citizen, GameModel _gameModel)
        {
            if (citizen == null)
                throw new ArgumentNullException(nameof(citizen));

            if (!citizen.IsActive)
                throw new InvalidOperationException("Cannot add an inactive citizen.");

            if (citizens.Count >= Capacity)
                throw new InvalidOperationException("Cannot add new citizen; zone capacity reached.");

            if (citizens.Contains(citizen))
                throw new InvalidOperationException("Citizen is already in the zone.");
            citizens.Add(citizen);
            //Statistics.Population += 1;
            //_gameModel.CityStatistics.SetCitySatisfaction(_gameModel);
        }

        public void RemoveCitizen(Citizen citizen, GameModel _gameModel)
        {
            citizens.Remove(citizen);
            //Statistics.Population -= 1;
            //_gameModel.CityStatistics.SetCitySatisfaction(_gameModel);
        }



        public Statistics GetOverallSatisfaction()
        {
            int totalSatisfaction = citizens.Where(c => c.IsActive).Sum(c => c.Satisfaction);
            int activeCitizenCount = citizens.Count(c => c.IsActive);
            int averageSatisfaction = activeCitizenCount > 0 ? totalSatisfaction / activeCitizenCount : 0;

            // statistics class can take satisfaction as a constructor parameter
            return new Statistics(new Budget()){ Satisfaction = averageSatisfaction };//******************!!!!!!!!!!!!
        }


        public int GetIncome(int taxRate)
        {
            return citizens.Where(c => c.IsActive).Sum(c => c.PayTax(taxRate));
        }


        public void IncCapacity(int num)
        {
            if (num < 0)
                throw new ArgumentException("Capacity increment must be positive.", nameof(num));

            int maxCapacity = 100; // for zone
            Capacity = Math.Min(Capacity + num, maxCapacity);
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
    }
}
