﻿using System;
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


namespace TFYP.Model
{
    /* Made this class serializable to save the current state of the game, including player progress, game settings, and the world state, so that it can be paused and resumed */

    [Serializable]
    public class GameModel : ISteppable
    {       
        private static GameModel instance = null;
        private static int _mapH, _mapW;
        public Buildable[,] map;

        public DateTime GameTime { get; private set; }

        public DateTime CreationDate; // DateTime built-in in c#
        public Statistics Statistics { get; private set; }
        public CityRegistry CityRegistry { get; private set; }
        public List<Road> Roads { get; private set; }

        public double MaxDistance { get; private set; }
        public float MaxTax { get; private set; }


        private GameModel(int _mapH, int _mapW)
        {
            MAP_H = _mapH;
            MAP_W = _mapW;
            map = new Buildable[_mapH, _mapW];
            Budget budget = new Budget();
            Statistics = new Statistics(budget);
            CityRegistry = new CityRegistry(Statistics);
            CreationDate = DateTime.Now; // Year, Month, Day - we will change date later
            Roads = new List<Road>();


            InitializeMap();
            InitializeMaxValues();
        }

        private void InitializeMap()
        {
            for (int i = 0; i < map.GetLength(0); i++)
            {
                for (int j = 0; j < map.GetLength(1); j++)
                {
                    if (i <= 1 || j == 0 || i >= map.GetLength(0) - 2 || j == map.GetLength(1) - 1)
                    {
                        map[i, j] = new Buildable(new List<Vector2> { new Vector2(i, j) }, EBuildable.Inaccessible);
                    }
                    else
                    {
                        map[i, j] = new Buildable(new List<Vector2> { new Vector2(i, j) }, EBuildable.None);
                    }
                }
            }
        }

        private void InitializeMaxValues()
        {
            // Assuming maximum distance is from one corner of the map to the opposite corner
            MaxDistance = Math.Sqrt(Math.Pow(MAP_H - 1, 2) + Math.Pow(MAP_W - 1, 2));
            MaxTax = 50; // --> we will change in future
        }

        public static GameModel GetInstance()
        {
            if (instance == null)
            {
                instance = new GameModel(20, 20);
            }
            return instance;
        }

        public static int MAP_H
        {
            get { return _mapH; }
            set { _mapH = value; }
        }


        public static int MAP_W
        {
            get { return _mapW; }
            set { _mapW = value; }
        }

        public void HealZone(int _x, int _y) {
            if (map[_x, _y].Type.Equals(EBuildable.Residential) || map[_x, _y].Type.Equals(EBuildable.Industrial) || map[_x, _y].Type.Equals(EBuildable.Service)) { 
                    Zone z= (Zone)map[_x, _y];
                if (CityRegistry.Statistics.Budget.Balance >= Constants.HealZone)
                {
                    CityRegistry.Statistics.Budget.UpdateBalance(-1*Constants.HealZone, this.GameTime);
                    z.Heal();
                }
            }
        }

        public void AddZone(int _x, int _y, EBuildable zone, bool rotate)
        {
            // TO DO: after adding a zone, roads should be checked, where is it connected now, and what effect did building of this zone cause
            try
            {
                AddToMap(_y, _x, zone, rotate);

                foreach (Road tmp in Roads)
                {
                    tmp.checkForZones();
                    foreach (var b in tmp.connected)
                    {
                        if (b.Type.Equals(EBuildable.Residential) || b.Type.Equals(EBuildable.Industrial) || b.Type.Equals(EBuildable.Service))
                        {
                            b.AddOutgoingRoad(tmp);
                        }
                    }
                }

                foreach (var z in CityRegistry.Zones)
                {
                    for(int i=0; i<z.Coor.Count; i++)
                    {
                        var tmp= z.Coor[i];
                        if (!z.Coor.Contains(new Vector2((tmp.X+1), tmp.Y)))
                        {
                            if (this.map[(int)tmp.X+1, (int)tmp.Y].Type.Equals(z.Type)) {
                                z.Coor.Add(new Vector2((tmp.X + 1),tmp.Y));
                            }
                        }
                        if (!z.Coor.Contains(new Vector2((tmp.X - 1), tmp.Y)))
                        {
                            if (this.map[(int)tmp.X - 1, (int)tmp.Y].Type.Equals(z.Type))
                            {
                                z.Coor.Add(new Vector2((tmp.X - 1), tmp.Y));
                            }
                        }
                        if (tmp.X % 2 == 0)
                        {
                            if (!z.Coor.Contains(new Vector2((tmp.X - 1), tmp.Y - 1)))
                            {
                                if (this.map[(int)tmp.X - 1, (int)tmp.Y-1].Type.Equals(z.Type))
                                {
                                    z.Coor.Add(new Vector2((tmp.X - 1), tmp.Y-1));
                                }
                            }
                            if (!z.Coor.Contains(new Vector2((tmp.X + 1), tmp.Y - 1)))
                            {
                                if (this.map[(int)tmp.X + 1, (int)tmp.Y - 1].Type.Equals(z.Type))
                                {
                                    z.Coor.Add(new Vector2((tmp.X + 1), tmp.Y - 1));
                                }
                            }
                        }
                        else {
                            if (!z.Coor.Contains(new Vector2((tmp.X - 1), tmp.Y + 1)))
                            {
                                if (this.map[(int)tmp.X - 1, (int)tmp.Y+1].Type.Equals(z.Type))
                                {
                                    z.Coor.Add(new Vector2((tmp.X - 1), tmp.Y+1));
                                }
                            }
                            if (!z.Coor.Contains(new Vector2((tmp.X + 1), tmp.Y + 1)))
                            {
                                if (this.map[(int)tmp.X + 1, (int)tmp.Y + 1].Type.Equals(z.Type))
                                {
                                    z.Coor.Add(new Vector2((tmp.X + 1), tmp.Y + 1));
                                }
                            }

                        }
                    }
                }

                foreach (var i in CityRegistry.Zones) {
                    foreach (var tmp in CityRegistry.Zones) {
                        if (!tmp.Equals(i)) {
                            foreach (var r in i.GetOutgoing()) 
                            {
                                if (r.connection(tmp)) {
                                    i.AddConnectedZone(tmp);
                                    tmp.AddConnectedZone(i);
                                    break;
                                }
                            }
                        }
                    }
                }
                foreach (var i in CityRegistry.Zones) {
                    var Connection = i.GetConnectedZones();
                    foreach (var tmp in i.Coor) {
                        if (!i.Equals(map[(int)tmp.X, (int)tmp.Y]))
                        {
                            foreach (var z in Connection)
                            {
                                map[(int)tmp.X, (int)tmp.Y].AddConnectedZone(z);
                            }
                        }
                    }
                }

                this.CityRegistry.Zones.ForEach(x => x.checkOutGoing());
            }
            catch (Exception ex) {
                Debug.WriteLine(ex);    
            }
            
        }
        private void AddToMap(int _x, int _y, EBuildable zone, bool rotate) {

            List<Vector2> t = new List<Vector2>();
            t.Add((new Vector2(_x, _y)));


            switch (zone)
            {
                case EBuildable.Stadium:

                   

                    Point[] points = new Point[4];
                    points[3] = new Point(_x, _y);
                    points[2] = GetCoordAt(0b_0100, points[3]);
                    points[1] = GetCoordAt(0b_1000, points[3]);
                    points[0] = GetCoordAt(0b_0100, points[1]);

              

                    if (!AreFree(points))
                    {
                        throw new Exception("second tile was already filled!");
                    }

                    CityRegistry.AddFacility(new Stadium(t, zone));
                    Statistics.Budget.UpdateBalance(-Constants.StadiumBuildCost, GameTime);
                    CityRegistry.Statistics.Budget.AddToMaintenanceFee(Constants.StadiumMaintenanceFee);
                    Stadium stad;

                    t.Add(points[0].ToVector2());
                    t.Add(points[1].ToVector2());
                    t.Add(points[2].ToVector2());

                    stad = new Stadium(t, zone);
                    map[points[0].X, points[0].Y] = stad;
                    map[points[1].X, points[1].Y] = stad;
                    map[points[2].X, points[2].Y] = stad;
                    map[points[3].X, points[3].Y] = stad;

                    break;

                case EBuildable.None:
                    this.RemoveFromMap(_x,_y);
                    break;

                case EBuildable.PoliceStation:
                    if(!map[_x, _y].Type.Equals(EBuildable.None))
                    {
                        break;
                    }
                    PoliceStation s = new PoliceStation(t, zone);
                    CityRegistry.AddFacility(s);
                    Statistics.Budget.UpdateBalance(-Constants.PoliceStationBuildCost, GameTime);
                    CityRegistry.Statistics.Budget.AddToMaintenanceFee(Constants.PoliceStationMaintenanceFee);
                    map[_x, _y] = s;
                    break;

                case EBuildable.Residential:
                    if (!map[_x, _y].Type.Equals(EBuildable.None))
                    {
                        break;
                    }
                    Zone z = new Zone(EBuildable.Residential, t, Constants.ResidentialEffectRadius, Constants.ResidentialZoneBuildTime, Constants.ResidentialZoneCapacity, Constants.ResidentialZoneMaintenanceCost, Constants.ResidentialZoneBuildCost, DateTime.Now);
                    CityRegistry.AddZone(z);
                    Statistics.Budget.UpdateBalance(-Constants.ResidentialZoneBuildCost, GameTime);
                    CityRegistry.Statistics.Budget.AddToMaintenanceFee(Constants.ResidentialZoneMaintenanceCost);
                    map[_x, _y] = z;
                    break;

                case EBuildable.Service:
                    if (!map[_x, _y].Type.Equals(EBuildable.None))
                    {
                        break;
                    }
                    Zone z1 = new Zone(EBuildable.Service, t, Constants.ServiceEffectRadius, Constants.ServiceZoneBuildTime, Constants.ServiceZoneCapacity, Constants.ServiceZoneMaintenanceCost, Constants.ServiceZoneBuildCost, DateTime.Now);
                    CityRegistry.AddZone(z1);
                    Statistics.Budget.UpdateBalance(-Constants.ServiceZoneBuildCost, GameTime);
                    CityRegistry.Statistics.Budget.AddToMaintenanceFee(Constants.ServiceZoneMaintenanceCost);
                    map[_x, _y] = z1;
                    break;

                case EBuildable.Industrial:
                    if (!map[_x, _y].Type.Equals(EBuildable.None))
                    {
                        break;
                    }
                    Zone z2 = new Zone(EBuildable.Industrial, t, Constants.IndustrialEffectRadius, Constants.IndustrialBuildTime, Constants.IndustrialZoneCapacity, Constants.IndustrialZoneMaintenanceCost, Constants.IndustrialZoneBuildCost, DateTime.Now);
                    CityRegistry.AddZone(z2);
                    Statistics.Budget.UpdateBalance(-Constants.IndustrialZoneBuildCost, GameTime);
                    CityRegistry.Statistics.Budget.AddToMaintenanceFee(Constants.IndustrialZoneMaintenanceCost);
                    map[_x, _y] = z2;
                    break;

                case EBuildable.Road:
                    if (map[_x, _y].Type.Equals(EBuildable.None))
                    {
                        Road r = new Road(t, EBuildable.Road);
                        map[_x, _y] = r;
                        Roads.Add(r);
                        this.Roads = this.Roads.Distinct().ToList();
                        Statistics.Budget.UpdateBalance(-Constants.RoadBuildCost, GameTime);
                        CityRegistry.Statistics.Budget.AddToMaintenanceFee(Constants.RoadMaintenanceFee);
                    }
                    break;
                    
                case EBuildable.University:
                    if (!map[_x, _y].Type.Equals(EBuildable.None))
                    {
                        break;
                    }
                    University u = new University(t);
                    CityRegistry.AddFacility(u);
                    map[_x, _y] = u;
                    Statistics.Budget.UpdateBalance(-Constants.UniversityBuildCost, GameTime);
                    CityRegistry.Statistics.Budget.AddToMaintenanceFee(Constants.StadiumMaintenanceFee);
                    break;

                case EBuildable.School:


                    points = new Point[2];
                    points[1] = new Point(_x, _y);
                    if (rotate)
                        points[0] = GetCoordAt(0b_0100, points[1]);
                    else
                        points[0] = GetCoordAt(0b_1000, points[1]);

                    if (!AreFree(points))
                    {
                        throw new Exception("second tile was already filled!");
                    }
                    CityRegistry.AddFacility(new School(t));
                    Statistics.Budget.UpdateBalance(-Constants.SchoolBuildCost, GameTime);
                    CityRegistry.Statistics.Budget.AddToMaintenanceFee(Constants.SchoolMaintenanceFee);

                    t.Add(points[0].ToVector2());

                    School tmp = new School(t);
                    map[points[0].X, points[0].Y] = tmp;
                    map[points[1].X, points[1].Y] = tmp;

                    break;

            }
        }


        public IEnumerable<Zone> GetAllZones()
        {
            for (int i = 0; i < _mapH; i++)
            {
                for (int j = 0; j < _mapW; j++)
                {
                    if (map[i, j] is Zone zone)
                    {
                        yield return zone;
                    }
                }
            }
        }
        public void ApplyDisasterToZone(Disaster disaster, Zone zone)
        {
            // Check if the zone is within the effect radius of the disaster
            // If so, apply the disaster effects to the zone and its citizens
        }
        // Example method to trigger a disaster
        public void TriggerDisaster(Disaster disaster)
        {
            
        }
        // we might also need a method to update the game world after the disaster effects
        public void UpdateAfterDisaster()
        {
            // update game world state here, like repairing buildings, updating citizen satisfaction, and so on
        }
        /// <summary>
        /// removing the tile fomr the map making it Ebuildable.None
        /// it's the outer function calling inner fucntions when needed like remove Zone or remove road
        /// </summary>
        /// <param name="_x"> x coridnate fo the removed </param>
        /// <param name="_y">y coridnate of the removed tile</param>
        private void RemoveFromMap(int _x, int _y)
        {
          
            var obj = map[_x, _y];
            if (obj.Type.Equals(EBuildable.Road))
            {
                this.RemoveRoad(_x, _y);
            }
            else if (obj.GetType().Equals(typeof(Zone)))
            {
                RemoveZone(_x, _y);
            }
            else {
                RemoveFacility(_x, _y);
            }
        }

        /// <summary>
        /// remove one til eof ht zone without removing the entire zone itself
        /// </summary>
        /// <param name="_x"></param>
        /// <param name="_y"></param>
        private void RemoveZone(int _x, int _y) {

            foreach (var zone in this.CityRegistry.Zones)
            {
                var c = zone.Coor[0];
                zone.Coor.Clear();
                zone.Coor.Add(c);
            }
            var obj = map[_x, _y];
            Zone z = (Zone)obj;
            this.CityRegistry.Zones.Remove(z);
            map[_x, _y] = new Buildable(new List<Vector2>() { new Vector2(_x, _y) },EBuildable.None);
            obj.Coor.Remove(new Vector2(_x, _y));
            foreach (var i in obj.Coor) {
                var t = map[(int)i.X, (int)i.Y];
                t.Coor = obj.Coor;
            }

            foreach (var i in CityRegistry.Zones) {
                i.ClearConnectedZone();
            }

        }


        /// <summary>
        /// checking if after the removal of the road the connected zones will be connected or the connection will be severed 
        /// making decision to procceed with the removal according to the result
        /// </summary>
        /// <param name="_x"></param>
        /// <param name="_y"></param>
        private void RemoveRoad(int _x, int _y) {
            var obj= map[_x, _y];
            map[_x, _y] = new Buildable(new List<Vector2> { new Vector2(_x, _y) }, EBuildable.None);
            this.Roads.ForEach(x => x.checkForZones());
            this.CityRegistry.Zones.ForEach(x=>x.checkOutGoing());
            bool test = true;
            foreach (var tmp in CityRegistry.Zones)
            {
                foreach (var z in tmp.GetConnectedZones())
                {
                    foreach (var i in tmp.GetOutgoing())
                    {
                        if (!i.connection(z))
                        {
                            test = false;
                        }
                        else
                        {
                            test = true;
                            break;
                        }
                    }
                    if (!test)
                    {
                        break;
                    }
                }
                if (!test)
                {
                    break;
                }
            }
            if (!test)
            {
                map[_x, _y] = obj;
                this.CityRegistry.Zones.ForEach(x => x.checkOutGoing());
            }
            else
            {
                this.Roads.Remove((Road)obj);
                this.Roads = this.Roads.Distinct().ToList();
                this.Roads.ForEach(x => x.checkForZones());
                this.CityRegistry.Zones.ForEach(x => x.checkOutGoing());
            }
        }
        private void RemoveFacility(int _x, int _y) {
            var obj = map[_x, _y];
            obj.Coor.ForEach(c => map[(int)c.X, (int)c.Y] = new Buildable(new List<Vector2> { c }, EBuildable.None));

        }


        //These 3 helper functions to calculate zone sattisfaction!!


        public float GetDistanceToNearestPoliceStation(Vector2 zoneCoordinate)
        {
            float minDistance = float.MaxValue;

            // Iterate over every cell in your grid map
            for (int i = 0; i < MAP_H; i++)
            {
                for (int j = 0; j < MAP_W; j++)
                {
                    // Check if the current cell contains a Police Station
                    if (map[i, j].Type == EBuildable.PoliceStation)
                    {
                        float distance = Math.Abs(zoneCoordinate.X - i) + Math.Abs(zoneCoordinate.Y - j); // Manhattan distance
                        if (distance < minDistance)
                        {
                            minDistance = distance;
                        }
                    }
                }
            }

            return minDistance;
        }

        public float GetDistanceToNearestStadium(Vector2 zoneCoordinate)
        {
            float minDistance = float.MaxValue;

            // Iterate over every cell in your grid map
            for (int i = 0; i < MAP_H; i++)
            {
                for (int j = 0; j < MAP_W; j++)
                {
                    // Check if the current cell contains a Stadium
                    if (map[i, j].Type == EBuildable.Stadium)
                    {
                        float distance = Math.Abs(zoneCoordinate.X - i) + Math.Abs(zoneCoordinate.Y - j); // Manhattan distance
                        if (distance < minDistance)
                        {
                            minDistance = distance;
                        }
                    }
                }
            }

            return minDistance;
        }

        public float GetDistanceToNearestIndustrialArea(Vector2 zoneCoordinate)
        {
            float minDistance = float.MaxValue;

            // Iterate over every cell in your grid map
            for (int i = 0; i < MAP_H; i++)
            {
                for (int j = 0; j < MAP_W; j++)
                {
                    // Check if the current cell is an Industrial area
                    if (map[i, j].Type == EBuildable.Industrial)
                    {
                        float distance = Math.Abs(zoneCoordinate.X - i) + Math.Abs(zoneCoordinate.Y - j); // Manhattan distance
                        if (distance < minDistance)
                        {
                            minDistance = distance;
                        }
                    }
                }
            }

            return minDistance;
        }


        //this is for citizen satisfaction, to measure the distance between his workplace and livingplace
        //i think it is not necessary to calculate it using roads - მოკლედ ანუ იქნებ მაგ ორს შორის პირდაპირ მანძილს
        //რომ ვპოულობ გზა არც გადის, მაგრამ მარტივად შეგვიძლია ასე გამოვთვალოთ ჩემი აზრით რადგან მაინც სეთისფექშენის
        //დასათვლელად ვიყენებთ ამ პარამეტრს მხოლოდ - კრისთან გადაამოწმე
        public int CalculateDistanceBetweenZones(Zone zone1, Zone zone2)
        {
            if (zone1 == null || zone2 == null)
            {
                throw new ArgumentNullException("Both zones must be non-null to calculate distance.");
            }

            List<Vector2> position1 = zone1.Coor;
            List<Vector2> position2 = zone2.Coor;
            int min = 1000000;

            foreach (Vector2 pos1 in position1)
            {
                foreach (Vector2 pos2 in position2)
                {
                    int distance = (int)(Math.Abs(pos1.X - pos2.X) + Math.Abs(pos1.Y - pos2.Y));
                    if (distance < min)
                    {
                        min = distance;
                    }
                }
            }


            return min;
        }

        public Buildable GetMapElementAt(int x, int y)
        {
            try
            {
                return map[y, x];
            }
            catch (IndexOutOfRangeException e)
            {
                return null;
            }
        }

        public Buildable GetMapElementAt(Point point)
        {
            return GetMapElementAt(point.X, point.Y);
        }

        public void UpgradeZone(int x, int y)
        {
            double upgradeCost = 0;
            // Find the zone at the given coordinates
            Zone zone = map[x, y] as Zone;
            if (zone != null)
            {
                Statistics.Budget.RemoveFromMaintenanceFee(zone.MaintenanceCost);//remove old maintenanceCost before upgrade  
                upgradeCost = zone.UpgradeZone();
                CityRegistry.SetBalance(-upgradeCost, GameTime);
                Statistics.Budget.AddToMaintenanceFee(zone.MaintenanceCost);//add new maintenanceCost after upgrade

            }

        }

        public bool AreNewCitizensEligible()
        {
            // Check general satisfaction level
            bool highSatisfaction = Statistics.Satisfaction >= Constants.SatisfactionUpperLimit;

            // Check for free workplaces near available residential zones
            bool freeWorkplacesAvailable = CityRegistry.GetFreeWorkplacesNearResidentialZones();

            // Check for absence of industrial buildings near these zones
            bool noNearbyIndustries = CityRegistry.NoIndustriesNearResidentialZones();

            return highSatisfaction && freeWorkplacesAvailable && noNearbyIndustries;
        }



        private void CitizenshipManipulation()
        {
            if (AreNewCitizensEligible())
            {
                for(int i = 0; i < CitizenLifecycle.StartingNrCitizens; i++)
                {
                    CitizenLifecycle.CreateYoungCitizen(this);
                }
            }
            
            //ეს ლოგიკა როცა ახალი ხალხი მოდის რადგან მაღალია სეთისფექშენ, დასამატებელია ლოგიკა როცა
            //იმდენად დაბალია რომ
            //პირიქით, ხალხი ტოვებს ქალაქს!! აქვე დაამატე
        }





        

        public void UpdateCitySatisfaction()
        {
            Statistics.CalculateCitySatisfaction(this);
        }

        private void CitizenshipEducationUpdate()
        {
            foreach (Citizen citizen in CityRegistry.GetAllCitizens())
            {
                if (citizen.EducationLevel == EducationLevel.Primary)
                {
                    citizen.EducationLevel = (CitizenLifecycle.GetEducationLevel(this, citizen.LivingPlace));
                }
            }
        }

        private void UpdateCityBalance()
        {
            double revenue = Statistics.Budget.ComputeRevenue(this);
            double spend = Statistics.Budget.MaintenanceFeeForEverything;
            Statistics.Budget.Balance += revenue;
            Statistics.Budget.Balance -= spend;
        }

        /// <summary>
        /// Function to be called for when a new day arrives.
        /// </summary>
        public void UpdateCityState()
        {
            // Placeholder for all update functions
            CitizenshipManipulation();
            CitizenshipEducationUpdate();
            UpdateZoneBuildingStatus();
            UpdateCitySatisfaction();
            GenerateDisaster();
            GameTime = GameTime.AddDays(1);

            UpdateCityBalance(); // --> PROBABLY THIS SHOULD BE RUN IN ONCE A YEAR???? IDK
            // სხვა აფდეითები და თამაშის წაგების ლოგიკა აქ დაემატება!
        }

        public void Step()
        {
            // Advance game time by one day every step
            //UpdateCityState();
        }

        public List<Zone> GetZonesThatAreStillBuilding()
        {
            List<Zone> stillBuilding = new List<Zone>();
            foreach (Zone zone in GetAllZones())
            {
                if (!zone.IsBuilt)
                {
                    stillBuilding.Add(zone);
                }
            }
            return stillBuilding;
        }

        private void UpdateZoneBuildingStatus()
        {
            DateTime gameCurrentTime = GameModel.GetInstance().GameTime;

            foreach (Zone zone in GetZonesThatAreStillBuilding())
            {
                TimeSpan dateDifference = gameCurrentTime - zone.DayOfBuildStart;
                int daysDifference = dateDifference.Days;

                if (daysDifference >= zone.TimeToBuild)
                {
                    zone.finishBuilding();
                }
            }
        }
        /// <summary>
        /// random disaster is generated at random location
        /// if the zone is in the range, entire zone will get damage
        /// disaster chance is 4% (1/25)
        /// </summary>
        public void GenerateDisaster() {
            int _X, _Y;
            Random rnd = new Random();
            int chance = rnd.Next(25);
            // 4 % chance of dissaster every day
            if(chance == 1)
            {
                _X = rnd.Next(20);
                _Y = rnd.Next(20);
                Disaster dis = new Disaster(5,new Vector2(_X, _Y));
                dis.ApplyEffects(this);
            }
        }



        public void PauseGame()
        {
            Timer.Instance.StopTimer();
            // Additional logic to display pause menu - if we will need it
        }

        public void ResumeGame()
        {
            Timer.Instance.StartTimer();
            // Additional logic to hide pause menu and return to the game 
        }

        //For persistence feature:
        public void SaveGame()
        {
            Timer.Instance.StopTimer();
            // Logic to serialize and save the game state
            Timer.Instance.StartTimer(); // restart timer if we want the game to resume immediately after saving
        }

        public void LoadGame()
        {
            Timer.Instance.StopTimer();
            // Logic to deserialize and load the game state
            Timer.Instance.StartTimer(); // Restart timer after loading is complete
        }

        public Buildable[] GetAdj(int i, int j)
        {
            Buildable[] arr = new Buildable[4];

            if (i % 2 == 1)
            {
                arr[0] = map[i - 1, j];
                arr[1] = map[i - 1, j + 1];
                arr[2] = map[i + 1, j + 1];
                arr[3] = map[i + 1, j];
            }
            else
            {
                arr[0] = map[i - 1, j - 1];
                arr[1] = map[i - 1, j];
                arr[2] = map[i + 1, j];
                arr[3] = map[i + 1, j - 1];
            }

            return arr;
        }

        public Point GetCoordAt(byte direction, Point coord)
        {
            return GetCoordAt(direction, coord.X, coord.Y);
        }

        public Point GetCoordAt(byte direction, int i, int j)
        {
            Point[] dir = new Point[4];
            if (i % 2 == 1)
            {
                dir[0] = new Point (i - 1, j);
                dir[1] = new Point (i - 1, j + 1);
                dir[2] = new Point (i + 1, j + 1);
                dir[3] = new Point (i + 1, j);
            }
            else
            {
                dir[0] = new Point (i - 1, j - 1);
                dir[1] = new Point (i - 1, j);
                dir[2] = new Point (i + 1, j);
                dir[3] = new Point (i + 1, j - 1);
            }

            switch (direction)
            {
                case 0b_1000: return dir[0];
                case 0b_0100: return dir[1];
                case 0b_0010: return dir[2];
                case 0b_0001: return dir[3];
            }

            throw new ArgumentOutOfRangeException();
        }

        public bool AreFree(params Point[] points)
        {
            //return points.All((point) => map[point.X, point.Y].Type == EBuildable.None);
            foreach (Point point in points)
            {
                if (map[point.X, point.Y].Type != EBuildable.None)
                {
                    Debug.WriteLine($"P: {point}");
                    return false;
                }
            }

            return true;
        }

        public int VerticalDistance(Point p1, Point p2)
        {
            return VerticalDistance(p1.X, p1.Y, p2.X, p2.Y);
        }

        public int VerticalDistance(int i1, int j1, int i2, int j2)
        {
            return Math.Abs(i2 - i1) - 1;
        }

        public int HorizontalDistance(Point p1, Point p2)
        {
            return HorizontalDistance(p1.X, p1.Y, p2.X, p2.Y);
        }

        public int HorizontalDistance(int i1, int j1, int i2, int j2)
        {
            int val = j2 - j1;

            if (val > 0)
            {
                return (val * 2) + (i2 % 2) - (i1 % 2) - 1;
            }
            else
            {
                return (-val * 2) - (i2 % 2) + (i1 % 2) - 1;
            }
        }
    }
}
