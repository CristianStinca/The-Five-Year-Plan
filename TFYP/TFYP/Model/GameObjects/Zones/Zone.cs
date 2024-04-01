using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TFYP.Model.GameObjects.Zones
{
    internal class Zone
    {
        private Statistics statistics;
        public int Level { get; private set; }
        public double TimeToBuild { get; private set; }
        public int Capacity { get; private set; }

        private List<Citizen> citizens; // Tracking the citizens within the zone
        public int MaintenanceCost { get; private set; }

        public Zone(Statistics stats, int level, double timeToBuild, int capacity, int maintenanceCost)
        {
            statistics = stats;
            Level = level;
            TimeToBuild = timeToBuild;
            Capacity = capacity;
            MaintenanceCost = maintenanceCost;
            citizens = new List<Citizen>();
        }

        public void AddCitizen(Citizen c)
        {
            if (c == null)
                throw new ArgumentNullException(nameof(c));

            if (!c.IsActive)
                throw new InvalidOperationException("Cannot add an inactive citizen.");

            if (citizens.Count >= Capacity)
                throw new InvalidOperationException("Cannot add new citizen; zone capacity reached.");

            if (citizens.Contains(c))
                throw new InvalidOperationException("Citizen is already in the zone.");

            citizens.Add(c);
        }
        public Statistics GetOverallSatisfaction()
        {
            int totalSatisfaction = citizens.Where(c => c.IsActive).Sum(c => c.Satisfaction);
            int activeCitizenCount = citizens.Count(c => c.IsActive);
            int averageSatisfaction = activeCitizenCount > 0 ? totalSatisfaction / activeCitizenCount : 0;

            // statistics class can take satisfaction as a constructor parameter
            return new Statistics { Satisfaction = averageSatisfaction };
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
    }
}
