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

    public enum ZoneStatus
    {
        Pending,
        Building,
        Done
    }


    [Serializable]
    public class Zone : Buildable
    {

        public float Health {  get; private set; } // Health will be probably changed later, so far it is for the disaster and is representing percentage (1-100)
        // health will help us to calculate the cost of the damage which will be OneTimeCost * (Health/100)
        // Tracking the citizens within the zone
        private bool canStartBuilding;
        public List<Citizen> citizens = new List<Citizen>();

        public ZoneLevel Level { get; private set; }
        public bool IsConnected { get; private set; }
        public List<Zone> conncetedZone= new List<Zone>();
        private List<Road> outGoing = new List<Road>();


        public DateTime DayOfCreation { get; set; }

        public DateTime DayOfBuildStart { get; private set; }
        public int TimeToBuild { get; set; }

        public ZoneStatus Status { get; set; }

        public bool isInitiallyPopulated { get; set; }
        public int Satisfaction { get; private set; }


        

        public int averageCitizensSatisfaction()
        {
            int totalCitizenSatisfaction = citizens.Sum(citizen => citizen.Satisfaction);
            int citizenCount = citizens.Count;

            if (citizenCount == 0)
            {
                return 0;  // Return 0 or some default value if there are no citizens
            }

            int averageCitizenSatisfaction = totalCitizenSatisfaction / citizenCount;
            return averageCitizenSatisfaction;
        }

        // when timer has gone through the days needed it will call this function to register that building is done
        public void finishBuilding()
        {
            Status = ZoneStatus.Done;
        }

        public int NCitizensInZone
        {
            get { return citizens.Count(c => c.IsActive); } 
        }
        //public void DeactivateCitizens()
        //{
        //    Random rnd = new Random();
        //    int ind = rnd.Next(citizens.Count());
        //    citizens.Remove(citizens[ind]);
        //}
        public Zone(EBuildable type, List<Vector2> coor, int influenceRadius, int timeToBuild, int capacity, int maintenanceCost, int buildCost, DateTime dayOfCreation)
            : base(coor, type, buildCost, maintenanceCost, influenceRadius, capacity, timeToBuild)
        {
            Health = 100;
            canStartBuilding = false;
            Level = ZoneLevel.One;
            IsConnected = false;
            TimeToBuild = timeToBuild;
            Status = ZoneStatus.Pending;
            Satisfaction = 50;
            DayOfCreation = dayOfCreation;
        }


        public void checkOutGoing() {
            GameModel gm = GameModel.GetInstance();
            outGoing.Clear();
            ThisCheck(this.Coor);
        }

        private void ThisCheck(List<Vector2> CoorL) {
            GameModel gm = GameModel.GetInstance();
            for (int i = 0; i < CoorL.Count; i++)
            {
                if (gm.map[(int)CoorL[i].X + 1, (int)CoorL[i].Y].Type.Equals(EBuildable.Road))
                {
                    outGoing.Add((Road)gm.map[(int)CoorL[i].X + 1, (int)CoorL[i].Y]);
                }
                if (gm.map[(int)CoorL[i].X - 1, (int)CoorL[i].Y].Type.Equals(EBuildable.Road))
                {
                    outGoing.Add((Road)gm.map[(int)CoorL[i].X - 1, (int)CoorL[i].Y]);
                }
                if ((int)CoorL[i].X % 2 == 0)
                {
                    if (gm.map[(int)CoorL[i].X + 1, (int)CoorL[i].Y - 1].Type.Equals(EBuildable.Road))
                    {
                        outGoing.Add((Road)gm.map[(int)CoorL[i].X + 1, (int)CoorL[i].Y - 1]);
                    }
                    if (gm.map[(int)CoorL[i].X - 1, (int)CoorL[i].Y - 1].Type.Equals(EBuildable.Road))
                    {
                        outGoing.Add((Road)gm.map[(int)CoorL[i].X - 1, (int)CoorL[i].Y - 1]);
                    }
                }
                else
                {
                    if (gm.map[(int)CoorL[i].X - 1, (int)CoorL[i].Y + 1].Type.Equals(EBuildable.Road))
                    {
                        outGoing.Add((Road)gm.map[(int)CoorL[i].X - 1, (int)CoorL[i].Y + 1]);
                    }
                    if (gm.map[(int)CoorL[i].X + 1, (int)CoorL[i].Y + 1].Type.Equals(EBuildable.Road))
                    {
                        outGoing.Add((Road)gm.map[(int)CoorL[i].X + 1, (int)CoorL[i].Y + 1]);
                    }
                }
            }
        }


        public void Heal() {
            this.Health += 10;
            Health= Health > 100 ? 100 : Health;
        }
    

        public List<Road> GetOutgoing() {
            return this.outGoing;
        }
        //TO DO: Need to implement method for finding paths and set Connected for every zone

        


        public bool HasFreeCapacity()
        {
            return Capacity > NCitizensInZone;
        }

        public float GetIncome(Budget budget)
        {
            return citizens.Where(c => c.IsActive).Sum(c => c.TaxAmount(budget));
        }


        public void SetHealth(float health)
        {
            // this will be called in disasters at first and will decrease the HP of the zone
            // then will be increased again to 100% after user repairs it, so RepairZone function is needed
            if(health < 0) health = 0;
            Health = health;
        }
        public virtual List<Citizen> GetCitizens()
        {
            return citizens;
        }

        public override void startBuilding(DateTime buildingStartDate) {
            this.canStartBuilding = true;
            Status = ZoneStatus.Building;
            DayOfBuildStart = buildingStartDate;
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
        

        public int GetZoneSatisfaction(GameModel gm)
        {
            double dampingFactor = 0.5;

            int policeEffect = 0;
            int stadiumEffect = 0;
            int industrialEffect = 100;// Start with the highest satisfaction if no industrial zones are found

            List<Facility> policeStations = gm.getEveryPolice();
            if(policeStations.Any()) 
            {
                int minDistance = gm.MaxDistance;
                foreach (Facility b in policeStations)
                {
                    int distance = gm.CalculateDistanceBetweenTwo(this, b);
                    if (distance < minDistance)
                    {
                        minDistance = distance;
                    }
                }
                policeEffect = (int)(100 - (minDistance * 100.0 / gm.MaxDistance) * dampingFactor);
            }

            List<Facility> stadiums = gm.getEveryStadium();
            if(stadiums.Any())
            {
                int minDistance = gm.MaxDistance;
                foreach (Facility b in stadiums)
                {
                    int distance = gm.CalculateDistanceBetweenTwo(this, b);
                    if (distance < minDistance)
                    {
                        minDistance = distance;
                    }

                }
                stadiumEffect = (int)(100 - (minDistance * 100.0 / gm.MaxDistance) * dampingFactor);
            }

            if(this.Type != EBuildable.Industrial)
            {
                List<Zone> industrials = gm.getEveryIndustrial();
                if (industrials.Any())
                {
                    int minDistance = gm.MaxDistance;
                    foreach (Zone b in industrials)
                    {
                        int distance = gm.CalculateDistanceBetweenTwo(this, b);
                        if (distance < minDistance)
                        {
                            minDistance = distance;
                        }
                    }
                    industrialEffect = (int)((minDistance * 100.0 / gm.MaxDistance) * dampingFactor); // Farther industrial zones reduce negative impact
                }
            }
            


            Satisfaction = (Satisfaction + policeEffect + stadiumEffect + industrialEffect) / 4;

            return Satisfaction;

            //return Math.Clamp(totalSatisfaction, 0, 100);
        }




        public void AddCitizen(Citizen citizen)
        {
            citizens.Add(citizen);
        }
        public void RemoveCitizen(Citizen citizen)
        {
            citizens.Remove(citizen);
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


        public override void ClearConnectedZone()
        {
            this.conncetedZone.Clear();
        }

        public override void AddConnectedZone(Zone z) {
            this.IsConnected = true;
            this.conncetedZone.Add(z);
            this.conncetedZone = this.conncetedZone.Distinct().ToList();
        }


        public override void AddOutgoingRoad(Road r) {
            this.outGoing.Add(r);
            this.outGoing=this.outGoing.Distinct().ToList();
        }

        public List<Zone> GetConnectedZones() {
            return this.conncetedZone;
        }


        public override string ToString()
        {
            var citizensInfo = citizens.Where(c => c.IsActive).Select(c => c.ToString()).ToArray();

            return $"Zone Details:\n" +
                   $"Type: {Type}, Level: {Level}, Health: {Health}%\n" +
                   $"Is Building Started: {(canStartBuilding ? "Yes" : "No")}, Is Connected: {(IsConnected ? "Yes" : "No")}\n" +
                   $"Number of Citizens: {NCitizensInZone}, Capacity: {Capacity}\n" +
                   $"Active Citizens Details: [{string.Join(", ", citizensInfo)}]";
        }

    }
}
