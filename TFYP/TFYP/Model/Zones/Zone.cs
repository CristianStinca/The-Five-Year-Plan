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
using ProtoBuf;

namespace TFYP.Model.Zones
{
    [ProtoContract]
    public enum ZoneLevel
    {
        One,
        Two,
        Three
    }
    [ProtoContract]
    public enum ZoneStatus
    {
        Pending,
        Building,
        Done
    }


    [Serializable]
    [ProtoContract]
    public class Zone : Buildable
    {
        [ProtoMember(1)]
        public float Health {  get; set; } 
        [ProtoMember(2)]
        public bool canStartBuilding;
        [ProtoMember(3)]
        public List<Citizen> citizens = new List<Citizen>();
        [ProtoMember(4)]
        public ZoneLevel Level { get; set; }
        [ProtoMember(5)]
        public bool IsConnected { get; set; }
        [ProtoMember(6)]
        public List<Zone> conncetedZone= new List<Zone>();
        [ProtoMember(7)]
        public List<Road> outGoing = new List<Road>();

        [ProtoMember(8)]
        public DateTime DayOfCreation { get; set; }
        [ProtoMember(9)]
        public DateTime DayOfBuildStart { get; set; }
        [ProtoMember(10)]
        public int TimeToBuild { get; set; }
        [ProtoMember(11)]
        public ZoneStatus Status { get; set; }
        [ProtoMember(12)]
        public bool isInitiallyPopulated { get; set; }
        [ProtoMember(13)]
        public int Satisfaction { get; set; }


        public Zone() { }

        /// <summary>
        /// Calculates the average satisfaction level of all citizens in the city.
        /// </summary>
        /// <returns>The average satisfaction level of citizens.</returns>
        public int averageCitizensSatisfaction()
        {
            int totalCitizenSatisfaction = citizens.Sum(citizen => citizen.Satisfaction);
            int citizenCount = citizens.Count;

            if (citizenCount == 0)
            {
                return 0;  
            }

            int averageCitizenSatisfaction = totalCitizenSatisfaction / citizenCount;
            return averageCitizenSatisfaction;
        }

        public void finishBuilding()
        {
            Status = ZoneStatus.Done;
        }

        public int NCitizensInZone
        {
            get { return citizens.Count(c => c.IsActive); } 
        }

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

        /// <summary>
        /// Checks adjacent tiles for roads and adds them to the list of outgoing roads.
        /// </summary>
        /// <param name="CoorL">List of coordinates to check.</param>
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
        
        


        public bool HasFreeCapacity()
        {
            return Capacity > NCitizensInZone;
        }

        /// <summary>
        /// Calculates the total income generated from active citizens based on taxes.
        /// </summary>
        /// <param name="budget">The budget instance used for tax calculations.</param>
        /// <returns>The total income generated from active citizens.</returns> 
        public float GetIncome(Budget budget)
        {
            return citizens.Where(c => c.IsActive).Sum(c => c.TaxAmount(budget));
        }


        public void SetHealth(float health)
        {
            if(health < 0) health = 0;
            Health = health;
        }
        public virtual List<Citizen> GetCitizens()
        {
            return citizens;
        }

        /// <summary>
        /// Starts the construction process of the zone, setting its status to building and recording the start date.
        /// </summary>
        /// <param name="buildingStartDate">The date when the construction begins.</param>
        public override void startBuilding(DateTime buildingStartDate) {
            this.canStartBuilding = true;
            Status = ZoneStatus.Building;
            DayOfBuildStart = buildingStartDate;
        }

        /// <summary>
        /// Calculates and returns the satisfaction level of the zone based on its proximity to police stations, stadiums, and industrial zones.
        /// </summary>
        /// <param name="gm">The GameModel instance containing information about the game world.</param>
        /// <returns>The satisfaction level of the zone.</returns>

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

            return Math.Clamp(Satisfaction, 0, 100);
        }




        public void AddCitizen(Citizen citizen)
        {
            citizens.Add(citizen);
        }
        public void RemoveCitizen(Citizen citizen)
        {
            citizens.Remove(citizen);
        }

        /// <summary>
        /// Upgrades the zone to the next level, increasing its maintenance cost, capacity, and influence radius (if applicable).
        /// </summary>
        /// <returns>The cost of upgrading the zone.</returns>
        /// <exception cref="InvalidOperationException">Thrown when attempting to upgrade a zone already at the maximum level.</exception>
        
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
