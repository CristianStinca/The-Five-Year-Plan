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
        public bool IsConnected { get; private set; }//ეს უნდა დაიმპლემენტდეს გზების ლოგიკის მერე!
        private List<Zone> conncetedZone= new List<Zone>();
        private List<Road> outGoing = new List<Road>();

        public DateTime DayOfBuildStart { get; private set; }
        public int TimeToBuild { get; set; }
        public bool IsBuilt { get; set; }

        // when timer has gone through the days needed it will call this function to register that building is done
        public void finishBuilding()
        {
            IsBuilt = true;
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
        public Zone(EBuildable type, List<Vector2> coor, int influenceRadius, int timeToBuild, int capacity, int maintenanceCost, int buildCost, DateTime dayOfBuildStart)
            : base(coor, type, buildCost, maintenanceCost, influenceRadius, capacity, timeToBuild)
        {
            Health = 100;
            canStartBuilding = false;
            Level = ZoneLevel.One;
            IsConnected = false;
            TimeToBuild = timeToBuild;
            IsBuilt = false;
            DayOfBuildStart = dayOfBuildStart;
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

            double freeWorkplaceEffect = (Capacity - NCitizensInZone) * 10; // more free capacity increases satisfaction

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
            citizens.Add(citizen);
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

        /*
         dictionary --> key is field, value is the actual value, 

         */
    }
}
