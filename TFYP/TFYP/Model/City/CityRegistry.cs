using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TFYP.Model.Facilities;
using TFYP.Model.Zones;
using TFYP.Model.Common;

namespace TFYP.Model.City
{
    public class CityRegistry
    {
        public double TaxRate { get; private set; }
        public double AverageIncome { get; private set; } = 1;
        public Budget budget;
        public Statistics Statistics { get; private set; }
        private ConcurrentDictionary<Type, ConcurrentBag<Buildable>> registry;
        private ConcurrentBag<Citizen> citizens;

        private ConcurrentDictionary<int, Zone> zones;
        private int nextZoneId; // to generate unique ID for zones if necessary


        public CityRegistry()
        {
            TaxRate = 0.1; // or 10%
            budget = new Budget();
            registry = new ConcurrentDictionary<Type, ConcurrentBag<Buildable>>();
            citizens = new ConcurrentBag<Citizen>();
            zones = new ConcurrentDictionary<int, Zone>();
            Statistics = new Statistics();
            nextZoneId = 0;
        }

        public void ChangeTaxRate(double newRate)
        {
            TaxRate = newRate;
        }

        public double QueryBudget()
        {
            return budget.Balance; // current balance
        }

        public void UpdateBudget(double dt)
        {
            int income = CalculateIncome(dt); // A method to calculate income based on tax rate and other factors
            int expenses = CalculateExpenses(dt); // A method to calculate expenses
            budget.UpdateBalance(income - expenses);
        }
        private int CalculateIncome(double dt)
        {
            int taxIncome = citizens.Count * (int)(TaxRate * AverageIncome);
            return taxIncome; // Replace with actual calculation
        }

        private int CalculateExpenses(double dt)
        {
            // Placeholder for expenses calculation logic
            // Example: maintenance for facilities
            int maintenanceExpenses = GetAllZones().Sum(z => z.MaintenanceCost);
            return maintenanceExpenses; // Replace with actual calculation
        }
        // citizen handling
        public void RegisterCitizen(Citizen c)
        {
            if (c == null)
                throw new System.ArgumentNullException(nameof(c));

            citizens.Add(c);
            UpdateStatistics();
        }
        public void DeregisterCitizen(Citizen c)
        {
            c.LeaveCity();
            UpdateStatistics();
        }
        public ConcurrentBag<Citizen> GetActiveCitizens()
        {
            var activeCitizens = new ConcurrentBag<Citizen>();
            foreach (var citizen in citizens.Where(c => c.IsActive))
            {
                activeCitizens.Add(citizen);
            }
            return activeCitizens;
        }
        // statistics
        private void UpdateStatistics()
        {
            Statistics.Population = GetActiveCitizens().Count;
            Statistics.Satisfaction = CalculateAverageSatisfaction();
        }
        private int CalculateAverageSatisfaction()
        {
            var activeCitizens = citizens.Where(c => c.IsActive).ToList();
            int totalSatisfaction = activeCitizens.Sum(c => c.Satisfaction);
            int activeCitizenCount = activeCitizens.Count;

            return activeCitizenCount > 0 ? totalSatisfaction / activeCitizenCount : 0;
        }

        // entities
        public void AddEntity<T>(T entity) where T : Buildable
        {
            var bag = registry.GetOrAdd(typeof(T), new ConcurrentBag<Buildable>());
            bag.Add(entity);
        }

        public ConcurrentBag<Buildable> GetEntities<T>() where T : Buildable
        {
            registry.TryGetValue(typeof(T), out ConcurrentBag<Buildable> bag);
            return bag ?? new ConcurrentBag<Buildable>(); // Return an empty bag if the key doesn't exist
        }

        // zones
        public void AddZone(Zone zone)
        {
            if (zone == null)
                throw new System.ArgumentNullException(nameof(zone));

            int zoneId = nextZoneId++;
            zones.TryAdd(zoneId, zone);
            UpdateStatistics();
        }
        public bool RemoveZone(int zoneId)
        {
            return zones.TryRemove(zoneId, out _);
        }

        // returns all zones for processing
        public IEnumerable<Zone> GetAllZones()
        {
            return zones.Values;
        }
    }
}
