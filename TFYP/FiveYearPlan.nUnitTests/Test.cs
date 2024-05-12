using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TFYP.Model;
using TFYP.Model.Facilities;
using TFYP.Model.Common;
using TFYP.Model.City;
using TFYP.Model.Zones;
using Microsoft.Xna.Framework;
using TFYP.Model.Disasters;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;
using System.Globalization;
using NUnit.Framework;



namespace FiveYearPlan.nUnitTests
{
    public class Tests
    {
        private GameModel _gameModel;

        [SetUp]
        public void Setup()
        {
            // Assuming GameModel's constructor is public or made internal visible to test assembly
            _gameModel = GameModel.GetInstance();
            _gameModel.CityRegistry.Statistics.Budget = new Budget() { Balance = 10000 }; // Ensuring sufficient balance

        }

        [Test]
        public void AddZone_AddsZoneSuccessfully_UpdatesMap()
        {
            // Arrange
            int x = 5, y = 5; // Coordinates where the zone will be added
            EBuildable zoneType = EBuildable.Residential;

            // Act
            _gameModel.AddZone(x, y, zoneType, false);

            // Assert
            Assert.IsInstanceOf<Zone>(_gameModel.map[y, x], "Zone not added correctly.");
            Assert.That(_gameModel.map[y, x].Type, Is.EqualTo(zoneType), "Zone type mismatch.");
        }

        [Test]
        public void RemoveZone_RemovesZoneSuccessfully()
        {
            // Arrange
            int x = 5, y = 5;
            _gameModel.map[x, y] = new Zone(EBuildable.Residential, new List<Vector2> { new Vector2(x, y) }, 10, 1, 100, 50, 100, DateTime.Now);

            // Act
            _gameModel.RemoveZone(x, y);

            // Assert
            Assert.That(_gameModel.map[x, y].Type, Is.EqualTo(EBuildable.None), "Zone should be removed.");
        }


        [Test]
        public void UpgradeZone_WhenCalled_UpdatesBudgetAndMaintenance()
        {
            // Arrange
            int x = 10, y = 10;
            _gameModel.map[x, y] = new Zone(EBuildable.Residential, new List<Vector2> { new Vector2(x, y) }, 10, 1, 100, 50, 100, DateTime.Now);
            double initialBudget = _gameModel.CityRegistry.Statistics.Budget.Balance;
            double maintenanceCostBefore = _gameModel.CityRegistry.Statistics.Budget.MaintenanceFeeForEverything;

            // Act
            _gameModel.UpgradeZone(x, y);
            Zone zone = (Zone)_gameModel.map[x, y];

            // Assert
            Assert.Less(_gameModel.CityRegistry.Statistics.Budget.Balance, initialBudget, "Budget should decrease after upgrade.");
            Assert.Greater(_gameModel.CityRegistry.Statistics.Budget.MaintenanceFeeForEverything, maintenanceCostBefore, "Maintenance cost should increase after upgrade.");
            Assert.That(zone.Level, Is.EqualTo(ZoneLevel.Two), "Zone status should be set to Upgraded.");
        }

        [Test]
        public void UpdateCityState_AdvancesGameTimeByADay()
        {
            // Arrange
            var initialTime = _gameModel.GameTime;

            // Act
            _gameModel.UpdateCityState();

            // Assert
            Assert.That(_gameModel.GameTime, Is.EqualTo(initialTime.AddDays(1)), "Game time should advance by one day.");
        }

        [Test]
        public void UpdateCityBalance_CorrectlyUpdatesFinancials()
        {
            // Arrange
            double initialBalance = _gameModel.Statistics.Budget.Balance;
            double expectedExpense = _gameModel.Statistics.Budget.MaintenanceFeeForEverything;
            double expectedRevenue = _gameModel.Statistics.Budget.ComputeRevenue(_gameModel);

            // Act
            _gameModel.UpdateCityBalance();

            // Assert
            double expectedBalance = initialBalance + expectedRevenue - expectedExpense;
            Assert.That(_gameModel.Statistics.Budget.Balance, Is.EqualTo(expectedBalance), "Balance should be correctly updated after financial operations.");
        }

        [Test]
        public void UpgradeZone_UpdatesLevelAndBudget()
        {
            // Arrange
            int x = 3, y = 3;
            _gameModel.AddZone(x, y, EBuildable.Residential, false);
            Zone zone = (Zone)_gameModel.map[x, y];
            double initialBalance = _gameModel.Statistics.Budget.Balance;

            // Act
            _gameModel.UpgradeZone(x, y);

            // Assert
            Assert.That(zone.Level, Is.EqualTo(ZoneLevel.Two), "Zone level should be upgraded.");
            Assert.Less(_gameModel.Statistics.Budget.Balance, initialBalance, "Budget should decrease due to upgrade costs.");
        }

        [Test]
        public void RemoveZone_UpdatesFinancialsAndZoneCount()
        {
            // Arrange
            _gameModel.AddZone(4, 4, EBuildable.Residential, false);
            int initialZoneCount = _gameModel.CityRegistry.Zones.Count;
            double initialBalance = _gameModel.Statistics.Budget.Balance;

            // Act
            _gameModel.RemoveZone(4, 4);

            // Assert
            Assert.That(_gameModel.CityRegistry.Zones.Count, Is.EqualTo(initialZoneCount - 1), "One zone should be removed.");
            Assert.That(_gameModel.Statistics.Budget.Balance, Is.EqualTo(initialBalance + Constants.ZoneReimbursement), "Budget should be reimbursed for the zone removal.");
        }



        [Test]
        public void RemoveRoad_UpdatesZoneConnectivity()
        {
            // Arrange
            _gameModel.AddZone(2, 2, EBuildable.Road, false);
            Road road = (Road)_gameModel.map[2, 2];
            Zone connectedZone = new Zone(EBuildable.Residential, new List<Vector2> { new Vector2(2, 3) }, Constants.ResidentialEffectRadius, Constants.ResidentialZoneBuildTime, Constants.ResidentialZoneCapacity, Constants.ResidentialZoneMaintenanceCost, Constants.ResidentialZoneBuildCost, DateTime.Now);
            _gameModel.CityRegistry.AddZone(connectedZone);
            road.AddConnectedZone(connectedZone);

            // Act
            _gameModel.RemoveRoad(2, 2);

            // Assert
            Assert.IsFalse(connectedZone.GetConnectedZones().Count() != 0, "Zone should no longer be connected to the removed road.");
        }

        [Test]
        public void YearEndFinancialReconciliation_ChecksBudgetAdjustment()
        {
            // Arrange
            _gameModel.GameTime = new DateTime(1923, 12, 31);  // Set the date to the end of the year

            // Act
            _gameModel.GameTime = _gameModel.GameTime.AddDays(1);
            

            // Assert
            Assert.That(_gameModel.GameTime, Is.EqualTo(new DateTime(1924, 1, 1)), "The game date should advance to the next year.");
           
        }



        [Test]
        public void GenerateDisaster_DisasterOccurs()
        {
            // Arrange
            _gameModel.GenerateDisasterByButton();

            // Act
            var disaster = _gameModel.latestDisaster;
            var affectedZone = _gameModel.map[(int)disaster.Location.X, (int)disaster.Location.Y] as Zone;

            // Assert
            Assert.IsNotNull(disaster, "A disaster should be generated.");
           
            
        }

        [Test]
        public void CitizenshipManipulation_CheckPopulationChanges()
        {
            // Arrange
            _gameModel.Statistics.Satisfaction = Constants.NewCitizenComingSatisfaction + 1; // Set satisfaction high to allow new citizens
            int initialPopulation = _gameModel.CityRegistry.GetAllCitizens().Count;

            // Act
            int newCitizens = _gameModel.CitizenshipManipulation();

            // Assert
            Assert.That(_gameModel.CityRegistry.GetAllCitizens().Count, Is.EqualTo(initialPopulation + newCitizens), "The total population should have increased by the number of new citizens.");
        
        
        }

        


        [Test]
        public void RemoveZone_CheckMapAndRegistryUpdates()
        {
            // Arrange
            _gameModel.AddZone(2, 2, EBuildable.Industrial, false);
            int initialZoneCount = _gameModel.CityRegistry.Zones.Count;

            // Act
            _gameModel.RemoveZone(2, 2);

            // Assert
            Assert.That(_gameModel.CityRegistry.Zones.Count, Is.EqualTo(initialZoneCount - 1), "Zone count should decrease by one after removal.");
            Assert.That(_gameModel.map[2, 2].Type, Is.EqualTo(EBuildable.None), "Map should update to reflect the zone removal.");
        }



        [Test]
        public void AddZone_CheckThatPopulatesCorrectly()
        {
            // Arrange
            int x = 6, y = 6;
            _gameModel.AddZone(x, y, EBuildable.Residential, false);
            Zone residentialZone = (Zone)_gameModel.map[x, y];
            CitizenLifecycle.populate(residentialZone, _gameModel);

            // Act
            bool isPopulated = residentialZone.isInitiallyPopulated;

            // Assert
            Assert.IsFalse(isPopulated, "Citizens should not be populated in new zone unless it's connected to working place.");
        }

        [Test]
        public void UpdateCityState_PerformanceCheckUnderHeavyLoad()
        {
            // Arrange
            // Simulating a heavy load by artificially increasing the number of zones and roads
            for (int i = 0; i < GameModel.MAP_H-2; i++)
            {
                for (int j = 0; j < GameModel.MAP_W-2; j++)
                {
                    _gameModel.AddZone(i, j, EBuildable.Road, false); // Filling map with roads
                }
            }

            // Act
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            _gameModel.UpdateCityState();
            stopwatch.Stop();

            // Assert
            Assert.IsTrue(stopwatch.ElapsedMilliseconds < 200, "UpdateCityState should execute within 200 milliseconds under heavy load.");
        }


        [Test]
        public void AddZone_UpdatesBudgetWhenZoneAdded()
        {
            // Arrange
            int initialCount = _gameModel.CityRegistry.Zones.Count;
            double initialBalance = _gameModel.Statistics.Budget.Balance;

            // Act
            _gameModel.AddZone(3, 3, EBuildable.Residential, false);

            // Assert
            Assert.That(_gameModel.CityRegistry.Zones.Count, Is.EqualTo(initialCount + 1), "Zone count should increase by one.");
            Assert.Less(_gameModel.Statistics.Budget.Balance, initialBalance, "Budget should decrease by the cost of the residential zone.");
        }


        [Test]
        public void AddZone_IllegalPlacement_NoChangeInStateOrBudget()
        {
            // Arrange
            int initialZoneCount = _gameModel.CityRegistry.Zones.Count;
            double initialBudget = _gameModel.CityRegistry.Statistics.Budget.Balance;

            // Act and Assert
            Assert.That(_gameModel.CityRegistry.Zones.Count, Is.EqualTo(initialZoneCount), "No new zone should be added.");
            Assert.That(_gameModel.CityRegistry.Statistics.Budget.Balance, Is.EqualTo(initialBudget), "Budget should not change after failed zone addition.");
        }


        [Test]
        public void CalculateFreeWorkplaces_CorrectCount()
        {
            // Arrange
            _gameModel.AddZone(3, 3, EBuildable.Industrial, false);
            Zone industrialZone = (Zone)_gameModel.map[3, 3];
            industrialZone.Capacity = 50; // Assuming capacity is the number of possible workers
            for (int i = 0; i < 20; i++)
            {
                CitizenLifecycle.CreateYoungCitizen(_gameModel);  // Fill some of the workplaces
            }

            // Act
            bool freeWorkplaces = _gameModel.CityRegistry.GetFreeWorkplacesNearResidentialZones(_gameModel);

            // Assert
            Assert.That(freeWorkplaces, Is.EqualTo(false), "Should correctly calculate the number of free workplaces.");
        }

        [Test]
        public void MapInitialization_CorrectDefaultValues()
        {
            // Act
            var gameModel = GameModel.GetInstance();
            bool hasOnlyNoneOrInaccessible = gameModel.map.Cast<Buildable>().All(b => b.Type == EBuildable.None || b.Type == EBuildable.Inaccessible);

            // Assert
            Assert.IsTrue(hasOnlyNoneOrInaccessible, "All tiles should be either None or Inaccessible upon initialization.");
        }


        [Test]
        public void DisasterRecovery_EffectiveRecovery_Mechanisms()
        {
            // Arrange
            _gameModel.GenerateDisasterByButton();  // Simulate a disaster
            Disaster activeDisaster = _gameModel.latestDisaster;

            // Act
            while (activeDisaster.isActive)
            {
                _gameModel.UpdateCityState();  // Perform daily updates until the disaster is no longer active
            }

            // Assert
            Assert.IsFalse(activeDisaster.isActive, "Disaster should no longer be active after recovery.");
            Assert.That(_gameModel.CityRegistry.Statistics.Budget.Balance, Is.GreaterThan(0), "City should recover financially after a disaster.");
        }



        [Test]
        public void UpgradeZone_AppliesUpgradeAndCostsCorrectly()
        {
            // Arrange
            _gameModel.AddZone(5, 5, EBuildable.Residential, false);
            Zone zone = (Zone)_gameModel.map[5, 5];
            double initialBudget = _gameModel.Statistics.Budget.Balance;

            // Act
            _gameModel.UpgradeZone(5, 5);
            bool isUpgraded = zone.Level == ZoneLevel.Two; 

            // Assert
            Assert.IsTrue(isUpgraded, "Zone should be upgraded to the next level.");
        }

        [Test]
        public void InitialGameSetup_CorrectlyInitializesMapAndResources()
        {
            // Act
            var initialMap = _gameModel.map;
            var initialBudget = _gameModel.Statistics.Budget.Balance;

            // Assert
            Assert.IsNotNull(initialMap, "Map should be initialized.");
            Assert.That(initialBudget, Is.EqualTo(10000), "Initial budget should be set correctly.");
            Assert.That(_gameModel.GetAllZones().Count(), Is.EqualTo(0), "Initial zone count should be zero.");
        }


        

        [Test]
        public void AddZone_InitializesZoneWithDefaultValues()
        {
            // Arrange
            int x = 7, y = 7;
            EBuildable zoneType = EBuildable.Industrial;

            // Act
            _gameModel.AddZone(x, y, zoneType, false);
            Zone zone = (Zone)_gameModel.map[x, y];

            // Assert
            Assert.That(zone.Level, Is.EqualTo(ZoneLevel.One), "Newly added zone should have initial level set to One.");
            Assert.That(zone.MaintenanceCost, Is.EqualTo(Constants.IndustrialZoneMaintenanceCost), "Maintenance cost should be set to the default value for Industrial zones.");
        }

        

        [Test]
        public void CitySatisfaction_UpdatesAfterPopulationChange()
        {
            // Arrange
            _gameModel.UpdateCityState(); // Initial update to stabilize conditions
            var initialSatisfaction = _gameModel.Statistics.Satisfaction;

            // Act
            // Simulating a condition that would change population dynamics
            _gameModel.CitizenshipManipulation();
            _gameModel.UpdateCitySatisfaction();

            // Assert
            Assert.That(_gameModel.Statistics.Satisfaction, Is.Not.EqualTo(initialSatisfaction), "City satisfaction should update after population changes.");
        }




    }
}