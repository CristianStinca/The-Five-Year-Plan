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
using System.Text.Json.Serialization;
//using TYFP.Persistence;
using ProtoBuf;
using System.IO;
using System.Text.Json;
using System.Runtime.CompilerServices;
using Newtonsoft.Json.Linq;
using System.Reflection;

namespace TFYP.Model
{
   
    [Serializable]
    public class GameModel
    {
        public static GameModel instance = null;
        public static int _mapH=20, _mapW=20;
        public Buildable[,] map;

        public DateTime GameTime { get;  set; }

        public DateTime CreationDate { get; set; }
        public Statistics Statistics { get;  set; }
        public CityRegistry CityRegistry { get; set; }
        public List<Road> Roads { get; set; }

        public int MaxDistance { get; set; }
        public int MaxTax { get; set; }
        public Disaster latestDisaster { get; set; }
        public List<Disaster> currentDisasters = new List<Disaster>();

        public event EventHandler GameOver;

        protected virtual void OnGameOver()
        {
            GameOver?.Invoke(this, EventArgs.Empty);
        }

        private GameModel(int _mapH, int _mapW)
        {
            MAP_H = _mapH;
            MAP_W = _mapW;
            map = new Buildable[_mapH, _mapW];
            Budget budget = new Budget();
            Statistics = new Statistics(budget);
            CityRegistry = new CityRegistry(Statistics);
            CreationDate = new DateTime(1923, 1, 1);
            GameTime = new DateTime(1923, 1, 1);
            Roads = new List<Road>();

            MaxTax = 1000;
            MaxDistance = 40;
            InitializeMap();
        }

        public GameModel() {
            map = new Buildable[20, 20];
            InitializeMap();

        }
        public void ChangeGameModel(GameModel model)
        {
            instance = model;
        }

        /// <summary>
        /// Initializes the map with default buildable entities, marking boundaries as inaccessible and other areas as empty.
        /// </summary>
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


        public static GameModel GetInstance()
        {
            if (instance == null)
            {
                instance = new GameModel(MAP_H, MAP_W);
            }
            return instance;
        }

        public static GameModel CleanGameModel()
        {
            instance = new GameModel(MAP_H, MAP_W);
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

        /// <summary>
        /// Heals a zone at the specified coordinates if it is of type Residential, Industrial, or Service.
        /// </summary>
        /// <param name="_x">The x-coordinate of the zone to heal.</param>
        /// <param name="_y">The y-coordinate of the zone to heal.</param>
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

        /// <summary>
        /// Counts the number of zones of type Residential in the city registry.
        /// </summary>
        /// <returns>The count of residential zones.</returns>
        public int CheckResidentialCount()
        {
            int count = 0;
            foreach(var z in CityRegistry.Zones)
            {
                if (z.Type.Equals(EBuildable.Residential))
                {
                    count++;
                }
            }
            return count;
        }

        /// <summary>
        /// Counts the number of zones of type Industrial in the city registry.
        /// </summary>
        /// <returns>The count of industrial zones.</returns>
        public int CheckIndustrialCount()
        {
            int count = 0;
            foreach (var z in CityRegistry.Zones)
            {
                if (z.Type.Equals(EBuildable.Industrial))
                {
                    count++;
                }
            }
            return count;
        }

        /// <summary>
        /// Counts the number of zones of type Service in the city registry.
        /// </summary>
        /// <returns>The count of service zones.</returns>
        public int CheckServiceCount()
        {
            int count = 0;
            foreach (var z in CityRegistry.Zones)
            {
                if (z.Type.Equals(EBuildable.Service))
                {
                    count++;
                }
            }
            return count;
        }

        /// <summary>
        /// Adds a zone to the map at the specified coordinates with the specified properties.
        /// </summary>
        /// <param name="_x">The x-coordinate of the position to add the zone.</param>
        /// <param name="_y">The y-coordinate of the position to add the zone.</param>
        /// <param name="zone">The type of zone to add.</param>
        /// <param name="rotate">Specifies if rotation is required for placement.</param>
        public void AddZone(int _x, int _y, EBuildable zone, bool rotate)
        {
            
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

        /// <summary>
        /// Adds a buildable entity to the map at the specified coordinates with the specified properties.
        /// </summary>
        /// <param name="_x">The x-coordinate of the position to add the buildable entity.</param>
        /// <param name="_y">The y-coordinate of the position to add the buildable entity.</param>
        /// <param name="zone">The type of buildable entity to add to the map.</param>
        /// <param name="rotate">Specifies if rotation is required for placement.</param>
        private void AddToMap(int _x, int _y, EBuildable zone, bool rotate) {

            List<Vector2> t = new List<Vector2>();
            t.Add((new Vector2(_x, _y)));


            switch (zone)
            {
                case EBuildable.Stadium:

                   

                    Point[] points = new Point[4];
                    points[3] = new Point(_x, _y);
                    points[2] = GetCoordAt(0b_0100, _x, _y);
                    points[1] = GetCoordAt(0b_1000, _x, _y);
                    points[0] = GetCoordAt(0b_1100, _x, _y);

              

                    if (!AreFree(points))
                    {
                        throw new Exception("second tile was already filled!");
                    }
                    else if(CityRegistry.StadiumCount == Constants.MaxStadiumCount)
                    {
                        throw new Exception("Stadium count limit reached");
                    }


                    CityRegistry.AddFacility(new Stadium(t, zone));
                    Statistics.Budget.UpdateBalance(-Constants.StadiumBuildCost, GameTime);
                    CityRegistry.Statistics.Budget.AddToMaintenanceFee(Constants.StadiumMaintenanceFee);
                    CityRegistry.IncStadiumCount();

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
                    if(!map[_x, _y].Type.Equals(EBuildable.None) || CityRegistry.PoliceCount == Constants.MaxPoliceCount)
                    {
                        break;
                    }
                    PoliceStation s = new PoliceStation(t, zone);
                    CityRegistry.AddFacility(s);
                    Statistics.Budget.UpdateBalance(-Constants.PoliceStationBuildCost, GameTime);
                    CityRegistry.Statistics.Budget.AddToMaintenanceFee(Constants.PoliceStationMaintenanceFee);
                    map[_x, _y] = s;
                    CityRegistry.IncPoliceCount();
                    break;

                case EBuildable.Residential:
                    if (!map[_x, _y].Type.Equals(EBuildable.None) || CheckResidentialCount() == Constants.MaxResidentialCount)
                    {
                        break;
                    }
                    Zone z = new Zone(EBuildable.Residential, t, Constants.ResidentialEffectRadius, Constants.ResidentialZoneBuildTime, Constants.ResidentialZoneCapacity, Constants.ResidentialZoneMaintenanceCost, Constants.ResidentialZoneBuildCost, GameTime);
                    CityRegistry.AddZone(z);
                    Statistics.Budget.UpdateBalance(-Constants.ResidentialZoneBuildCost, GameTime);
                    CityRegistry.Statistics.Budget.AddToMaintenanceFee(Constants.ResidentialZoneMaintenanceCost);
                    map[_x, _y] = z;
                    
                    break;

                case EBuildable.Service:
                    if (!map[_x, _y].Type.Equals(EBuildable.None) || CheckServiceCount() == Constants.MaxServiceCount)
                    {
                        break;
                    }
                    Zone z1 = new Zone(EBuildable.Service, t, Constants.ServiceEffectRadius, Constants.ServiceZoneBuildTime, Constants.ServiceZoneCapacity, Constants.ServiceZoneMaintenanceCost, Constants.ServiceZoneBuildCost, GameTime);
                    CityRegistry.AddZone(z1);
                    Statistics.Budget.UpdateBalance(-Constants.ServiceZoneBuildCost, GameTime);
                    CityRegistry.Statistics.Budget.AddToMaintenanceFee(Constants.ServiceZoneMaintenanceCost);
                    map[_x, _y] = z1;
                    break;

                case EBuildable.Industrial:
                    if (!map[_x, _y].Type.Equals(EBuildable.None) || CheckIndustrialCount() == Constants.MaxIndustrialCount)
                    {
                        break;
                    }
                    Zone z2 = new Zone(EBuildable.Industrial, t, Constants.IndustrialEffectRadius, Constants.IndustrialBuildTime, Constants.IndustrialZoneCapacity, Constants.IndustrialZoneMaintenanceCost, Constants.IndustrialZoneBuildCost, GameTime);
                    CityRegistry.AddZone(z2);
                    Statistics.Budget.UpdateBalance(-Constants.IndustrialZoneBuildCost, GameTime);
                    CityRegistry.Statistics.Budget.AddToMaintenanceFee(Constants.IndustrialZoneMaintenanceCost);
                    map[_x, _y] = z2;
                    break;

                case EBuildable.Road:
                    if (map[_x, _y].Type.Equals(EBuildable.None) && CityRegistry.RoadCount < Constants.MaxRoadCount)
                    {
                        Road r = new Road(t, EBuildable.Road);
                        map[_x, _y] = r;
                        Roads.Add(r);
                        this.Roads = this.Roads.Distinct().ToList();
                        Statistics.Budget.UpdateBalance(-Constants.RoadBuildCost, GameTime);
                        CityRegistry.Statistics.Budget.AddToMaintenanceFee(Constants.RoadMaintenanceFee);
                        CityRegistry.IncRoadCount();
                    }
                    break;
                    
                case EBuildable.University:

                    Point[] pts = new Point[4];
                    pts[3] = new Point(_x, _y);
                    pts[2] = GetCoordAt(0b_0100, _x, _y);
                    pts[1] = GetCoordAt(0b_1000, _x, _y);
                    pts[0] = GetCoordAt(0b_1100, _x, _y);



                    if (!AreFree(pts) || CityRegistry.UniversityCount == Constants.MaxUniversityCount)
                    {
                        throw new Exception("second tile was already filled!");
                    }

                    CityRegistry.AddFacility(new University(t));

                    Statistics.Budget.UpdateBalance(-Constants.UniversityBuildCost, GameTime);
                    CityRegistry.Statistics.Budget.AddToMaintenanceFee(Constants.UniversityMaintenanceFee);
                    CityRegistry.IncUniversityCount();

                    University uni;

                    t.Add(pts[0].ToVector2());
                    t.Add(pts[1].ToVector2());
                    t.Add(pts[2].ToVector2());

                    uni = new University(t);
                    map[pts[0].X, pts[0].Y] = uni;
                    map[pts[1].X, pts[1].Y] = uni;
                    map[pts[2].X, pts[2].Y] = uni;
                    map[pts[3].X, pts[3].Y] = uni;
                    
                    break;

                case EBuildable.School:


                    points = new Point[2];
                    points[1] = new Point(_x, _y);
                    if (rotate)
                        points[0] = GetCoordAt(0b_0100, points[1]);
                    else
                        points[0] = GetCoordAt(0b_1000, points[1]);

                    if (!AreFree(points) || CityRegistry.SchoolCount == Constants.MaxSchoolCount)
                    {
                        throw new Exception("second tile was already filled!");
                    }
                    CityRegistry.AddFacility(new School(t));
                    Statistics.Budget.UpdateBalance(-Constants.SchoolBuildCost, GameTime);
                    CityRegistry.Statistics.Budget.AddToMaintenanceFee(Constants.SchoolMaintenanceFee);
                    CityRegistry.IncSchoolCount();
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
                CityRegistry.DecRoadCount();
                this.RemoveRoad(_x, _y);
            }
            else if (obj.GetType().Equals(typeof(Zone)))
            {
                RemoveZone(_x, _y);
            }
            else if (!(obj.Type.Equals(EBuildable.None)))
            {
                if (obj.Type.Equals(EBuildable.University))
                {
                    CityRegistry.DecUniversityCount();
                }
                else if (obj.Type.Equals(EBuildable.School))
                {
                    CityRegistry.DecSchoolCount();
                }
                else if(obj.Type.Equals(EBuildable.PoliceStation))
                {
                    CityRegistry.DecPoliceCount();
                }
                else if(obj.Type.Equals(EBuildable.Stadium))
                {
                    CityRegistry.DecStadiumCount();
                }

                RemoveFacility(_x, _y);
                CityRegistry.Statistics.Budget.RemoveFromMaintenanceFee(Constants.FacilityMaintenanceCost);
                Statistics.Budget.UpdateBalance(Constants.FacilityReimbursement, GameTime);
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
        public void RemoveZone(int _x, int _y) {

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
            // Adjusting balance after removing zone
            CityRegistry.Statistics.Budget.RemoveFromMaintenanceFee(Constants.ZoneMaintenanceCost);
            Statistics.Budget.UpdateBalance(Constants.ZoneReimbursement, GameTime);

        }


        /// <summary>
        /// checking if after the removal of the road the connected zones will be connected or the connection will be severed 
        /// making decision to procceed with the removal according to the result
        /// </summary>
        /// <param name="_x"></param>
        /// <param name="_y"></param>
        public void RemoveRoad(int _x, int _y) {
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
                // Adjusting balance after removing road
                CityRegistry.Statistics.Budget.RemoveFromMaintenanceFee(Constants.RoadMaintenanceCost);
                Statistics.Budget.UpdateBalance(Constants.RoadReimbursement, GameTime);
            }
        }
        private void RemoveFacility(int _x, int _y) {
            var obj = map[_x, _y];
            obj.Coor.ForEach(c => map[(int)c.X, (int)c.Y] = new Buildable(new List<Vector2> { c }, EBuildable.None));

        }

        /// <summary>
        /// Calculates the minimum distance between two buildable entities.
        /// </summary>
        /// <param name="b1">The first buildable entity.</param>
        /// <param name="b2">The second buildable entity.</param>
        /// <returns>The minimum distance between the two buildable entities.</returns>
        public int CalculateDistanceBetweenTwo(Buildable b1, Buildable b2)
        {
            if (b1 == null || b2 == null)
            {
                throw new ArgumentNullException("Both places must be non-null to calculate distance.");
            }

            int minDistance = int.MaxValue;

            foreach (Vector2 coor1 in b1.Coor)
            {
                foreach (Vector2 coor2 in b2.Coor)
                {
                    int distance = Distance(coor1, coor2);
                    if (distance < minDistance)
                    {
                        minDistance = distance;
                    }
                }
            }

            return minDistance+1;
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

        /// <summary>
        /// Upgrades the zone at the specified coordinates, adjusting maintenance fees and city balance accordingly.
        /// </summary>
        /// <param name="x">The x-coordinate of the zone to upgrade.</param>
        /// <param name="y">The y-coordinate of the zone to upgrade.</param>
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

        /// <summary>
        /// Checks if new citizens are eligible to arrive based on various criteria.
        /// </summary>
        /// <returns>True if new citizens are eligible to arrive, otherwise false.</returns>
        public bool AreNewCitizensEligible()
        {
            // Check general satisfaction level
            bool highSatisfaction = Statistics.Satisfaction >= Constants.NewCitizenComingSatisfaction;

            // Check for free workplaces near available residential zones
            bool freeWorkplacesAvailable = CityRegistry.GetFreeWorkplacesNearResidentialZones(this);

            // Check for absence of industrial buildings near these zones
            bool noNearbyIndustries = CityRegistry.NoIndustriesNearResidentialZones(this);

            return highSatisfaction && freeWorkplacesAvailable && noNearbyIndustries;
        }

        public List<Zone> getEveryIndustrial()
        {
            List<Zone> industrials = CityRegistry.Zones.Where(z => z.Type == EBuildable.Industrial).ToList();
            return industrials;
        }

        public List<Facility> getEveryPolice()
        {
            List<Facility> polices = CityRegistry.Facilities.Where(f => f.Type == EBuildable.PoliceStation).ToList();
            return polices;
        }

        public List<Facility> getEveryStadium()
        {
            List<Facility> stadiums = CityRegistry.Facilities.Where(f => f.Type == EBuildable.Stadium).ToList();
            return stadiums;
        }

        /// <summary>
        /// Initializes the population of residential zones that have not been initially populated and are connected.
        /// </summary>
        public void initialPopulationOfZones()
        {
            List<Zone> allResidentialZones = new List<Zone>();
            foreach (var residentialZone in this.CityRegistry.Zones.Where(z => z.Type == EBuildable.Residential))
            {
                if (!residentialZone.isInitiallyPopulated && residentialZone.IsConnected)
                {
                    //residentialZone.startBuilding(this.GameTime);
                    for (int i = 0; i < CitizenLifecycle.StartingNrCitizens; i++)
                    {
                        CitizenLifecycle.populate(residentialZone, this);
                    }
                }
            }

        }


        /// <summary>
        /// Manages citizenship by creating new citizens based on eligibility criteria.
        /// </summary>
        /// <returns>The number of new citizens arrived on this day.</returns>
        public int CitizenshipManipulation()
        {
            int citizensArrivedOnThisDay = 0;

            if (AreNewCitizensEligible())
            {
                for(int i = 0; i < CitizenLifecycle.ImmigrantsCount; i++)
                {
                    CitizenLifecycle.CreateYoungCitizen(this);
                    citizensArrivedOnThisDay += 1;
                }
            }
            return citizensArrivedOnThisDay;
            
        }

        /// <summary>
        /// Updates the city satisfaction by evaluating citizen satisfaction and zone satisfaction.
        /// </summary>
        /// <returns>The number of citizens left the city on this day due to low satisfaction.</returns>
        public int UpdateCitySatisfaction()
        {
            int citizensLeftOnThisDay = 0;
            
            //update citizens satisfaction and they might leave city if it's too low
            foreach (Citizen citizen in CityRegistry.GetAllCitizens())
            {
                citizen.CalculateSatisfaction(this, Statistics.Budget);
                if(citizen.Satisfaction < Constants.CitizenLeavingSatisfaction)
                {
                    citizen.LeaveCity();
                    citizensLeftOnThisDay += 1;
                }
            }

            //update zones sat
            foreach (Zone zone in CityRegistry.Zones)
            {
                zone.GetZoneSatisfaction(this);

            }

            //update whole city sat
            int citySat = Statistics.CalculateCitySatisfaction(this, citizensLeftOnThisDay);
            if(citySat < Constants.GameOverSatisfaction)
            {
                //Game is over if city satisfaction became critically low
                OnGameOver();
            }

            return citizensLeftOnThisDay;
        }

        /// <summary>
        /// Updates the education level of citizens based on their living place.
        /// </summary>
        public void CitizenshipEducationUpdate()
        {
            foreach (Citizen citizen in CityRegistry.GetAllCitizens())
            {
                if (citizen.EducationLevel == EducationLevel.Primary)
                {
                    citizen.EducationLevel = (CitizenLifecycle.GetEducationLevel(this, citizen.LivingPlace));

                }
            }
        }

        /// <summary>
        /// Updates the city's financial balance by computing revenue, deducting maintenance fees, and updating the balance.
        /// </summary>
        public void UpdateCityBalance()
        {
            double revenue = Statistics.Budget.ComputeRevenue(this);
            double spend = Statistics.Budget.MaintenanceFeeForEverything;
            Statistics.Budget.UpdateBalance(revenue, GameTime);
            Statistics.Budget.UpdateBalance(-spend, GameTime);
            
        }

        /// <summary>
        /// Updates the city state when a new day arrives.
        /// </summary>
        public void UpdateCityState()
        {
            GameTime = GameTime.AddDays(1);
            initialPopulationOfZones();
            UpdateCitySatisfaction();
            CitizenshipManipulation();
            CitizenshipEducationUpdate();
            CheckForDisasters();
            // Check if it's the first day of the year to update city balance -->
            // collect taxes from citizens and pay maintainance fees once a year
            if (GameTime.Day == 1 && GameTime.Month == 1)
            {
                UpdateCityBalance();
            }
            UpdateZoneBuildingStatus();
            ConvertUnusedZonesToGeneral();
            GenerateDisaster();
        }

        /// <summary>
        /// Retrieves a list of zones that are currently under construction.
        /// </summary>
        /// <returns>A list of zones that are still building.</returns>
        public List<Zone> GetZonesThatAreStillBuilding()
        {
            List<Zone> stillBuilding = new List<Zone>();
            foreach (Zone zone in GetAllZones())
            {
                /*if(zone.Type == EBuildable.Service || zone.Type == EBuildable.Industrial)
                {
                    if (zone.IsConnected && zone.Status == ZoneStatus.Pending)
                    {
                        zone.startBuilding(this.GameTime);
                    }
                }*/

                if (zone.IsConnected && zone.Status == ZoneStatus.Pending)
                {
                    zone.startBuilding(this.GameTime);
                }

                if (zone.Status == ZoneStatus.Building)
                {
                    stillBuilding.Add(zone);
                }
            }
            return stillBuilding;
        }

        /// <summary>
        /// Updates the building status of zones that are still under construction.
        /// </summary>
        public void UpdateZoneBuildingStatus()
        {
            foreach (Zone zone in GetZonesThatAreStillBuilding())
            {
                TimeSpan dateDifference = GameTime - zone.DayOfBuildStart;
                int daysDifference = dateDifference.Days;
                if (daysDifference >= zone.TimeToBuild)
                {
                    zone.finishBuilding();
                }
            }
        }

        /// <summary>
        /// Converts unused zones, which have been pending for at least one year, to general zones.
        /// </summary>
        public void ConvertUnusedZonesToGeneral()
        {
            List<Zone> deactivatableZones = new List<Zone>();
            foreach (var zone in CityRegistry.Zones)
            {
                deactivatableZones.Add(zone);
            }
            foreach (var zone in deactivatableZones)
            {
                Zone currZone = (Zone)zone;
                TimeSpan timeDifference = GameTime - currZone.DayOfCreation;
                bool isOneYearPassed = Math.Abs(timeDifference.Days) == 365 || Math.Abs(timeDifference.Days) == 366;


                if (currZone.Status == ZoneStatus.Pending && isOneYearPassed)
                {
                    RemoveZone((int)currZone.Coor[0].X, (int)currZone.Coor[0].Y);
                }

            }
            deactivatableZones.Clear();
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
                _X = rnd.Next(2, MAP_H - 2);
                _Y = rnd.Next(1, MAP_W - 1);
                Disaster dis = new Disaster(5,new Vector2(_X, _Y));
                dis.ApplyEffects(this);
                latestDisaster = dis;
                currentDisasters.Add(dis);
            }
        }
        /// <summary>
        /// Generates a new disaster at a random position on the map when triggered by a button press.
        /// The generated disaster is applied to the game environment, and its effects are activated.
        /// </summary>
        public void GenerateDisasterByButton()
        {
            int _X, _Y;
            Random rnd = new Random();
            _X = rnd.Next(2, MAP_H - 2);
            _Y = rnd.Next(1, MAP_W - 1);
            Disaster dis = new Disaster(5, new Vector2(_X, _Y));
            dis.ApplyEffects(this);
            latestDisaster = dis;
            currentDisasters.Add(dis);
        }

        public List<Disaster> GetCurrentDisasters()
        {
            return currentDisasters;
        }

        /// <summary>
        /// Checks for active disasters in the current list of disasters and removes them if they are no longer active.
        /// </summary>
        public void CheckForDisasters()
        {
            List<Disaster> removableDisasters = new List<Disaster>();
            foreach(Disaster disaster in currentDisasters)
            {
                if (disaster.isActive)
                {
                    removableDisasters.Add(disaster);
                }
            }
            foreach(Disaster disaster in removableDisasters)
            {
                currentDisasters.Remove(disaster);
            }
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
                case 0b_1100: return GetCoordAt(0b_0100, dir[0]);
                case 0b_0110: return GetCoordAt(0b_0010, dir[1]);
                case 0b_0011: return GetCoordAt(0b_0001, dir[2]);
                case 0b_1001: return GetCoordAt(0b_1000, dir[3]);
            }

            throw new ArgumentOutOfRangeException("Invalid direction.");
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

        public int VerticalDistance(Vector2 p1, Vector2 p2)
        {
            return VerticalDistance(p1.ToPoint(), p2.ToPoint());
        }

        public int VerticalDistance(Point p1, Point p2)
        {
            return VerticalDistance(p1.X, p1.Y, p2.X, p2.Y);
        }

        public int VerticalDistance(int i1, int j1, int i2, int j2)
        {
            return Math.Abs(i2 - i1) - 1;
        }

        public int HorizontalDistance(Vector2 p1, Vector2 p2)
        {
            return HorizontalDistance(p1.ToPoint(), p2.ToPoint());
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

        public int Distance(Vector2 p1, Vector2 p2)
        {
            return Distance(p1.ToPoint(), p2.ToPoint());
        }

        public int Distance(Point p1, Point p2)
        {
            return Distance(p1.X, p1.Y, p2.X, p2.Y);
        }

        public int Distance(int i1, int j1, int i2, int j2)
        {
            return Math.Max(VerticalDistance(i1, j1, i2, j2), HorizontalDistance(i1, j1, i2, j2));
        }

        private static string GetSaveFilePath(int slot)
        {
            string baseDirectory = Path.GetFullPath(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"..\..\..\")); //@"C:\Users\nikol\Desktop\tfyp\TFYP\TFYP"; // Adjust as necessary
            string persistenceFolder = Path.Combine(baseDirectory, "Persistence");
            string filename = $"save{slot}.json";

            if (!Directory.Exists(persistenceFolder))
            {
                Directory.CreateDirectory(persistenceFolder);
            }

            return Path.Combine(persistenceFolder, filename);
        }

        /// <summary>
        /// Saves the current game instance to the specified save slot by serializing it to JSON format and writing to a file.
        /// </summary>
        /// <param name="slot">The save slot where the game data will be stored.</param>
        public static void Save(int slot)
        {
            string filePath = GetSaveFilePath(slot);
            Debug.WriteLine($"saving to {filePath}");
            try
            {
                string jsonString = JsonSerializer.Serialize(instance, new JsonSerializerOptions { WriteIndented = true });
                JObject obj = JObject.Parse(jsonString);
                for (int i = 0; i < instance.CityRegistry.Zones.Count; i++)
                {
                    var o = obj["CityRegistry"]["Zones"][i]["Coor"];
                    string val = "";
                    foreach (var t in instance.CityRegistry.Zones[i].Coor) {
                        val += t.X.ToString() + "," + t.Y.ToString()+";";
                    }
                    obj["CityRegistry"]["Zones"][i]["Coor"]=val;



                }
                for (int i = 0; i < instance.Roads.Count; i++)
                {
                    var o = obj["Roads"][i]["Coor"];
                    string val = "";
                    foreach (var t in instance.Roads[i].Coor)
                    {
                        val += t.X.ToString() + "," + t.Y.ToString() + ";";
                    }
                    obj["Roads"][i]["Coor"] = val;



                }
                for (int i = 0; i < instance.CityRegistry.Facilities.Count; i++)
                {
                    var o = obj["CityRegistry"]["Facilities"][i]["Coor"];
                    string val = "";
                    foreach (var t in instance.CityRegistry.Facilities[i].Coor)
                    {
                        val += t.X.ToString() + "," + t.Y.ToString() + ";";
                    }
                    obj["CityRegistry"]["Facilities"][i]["Coor"] = val;



                }
                jsonString = obj.ToString();


                File.WriteAllText(filePath, jsonString);
                Debug.WriteLine($"Game saved successfully in slot {slot}.");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Failed to save game model in slot {slot}: " + ex.Message);
            }
        }

        /// <summary>
        /// Reads game data from the specified save slot, deserializes it, and updates the current game instance with the loaded data.
        /// </summary>
        /// <param name="slot">The save slot from which to read the game data.</param>
        public static void Read(int slot)
        {
            string filePath = GetSaveFilePath(slot);
            Debug.WriteLine($"loading from {filePath}");
            try
            {
                if (File.Exists(filePath))
                {
                    string jsonString = File.ReadAllText(filePath);
                    JObject obj = JObject.Parse(jsonString);
                    List<List<Vector2>> l1 = new List<List<Vector2>>();
                    List<List<Vector2>> l2 = new List<List<Vector2>>();
                    List<List<Vector2>> l3 = new List<List<Vector2>>();
                    for (int i = 0; i < obj["CityRegistry"]["Zones"].Count(); i++)
                    {
                        var o = obj["CityRegistry"]["Zones"][i]["Coor"];
                        string s = o.ToString();
                        string[] m = s.Split(';');
                        List<Vector2> p = new List<Vector2>();
                        foreach (var t in m)
                        {
                            if (t.Length > 1)
                            {
                                p.Add(new Vector2(int.Parse(t.Split(',')[0]), int.Parse(t.Split(',')[1])));
                            }
                        }
                        l1.Add(p);

                        obj["CityRegistry"]["Zones"][i]["Coor"]= JArray.Parse("[]");


                    }
                    

                    for (int i = 0; i < obj["CityRegistry"]["Facilities"].Count(); i++)
                    {
                        var o = obj["CityRegistry"]["Facilities"][i]["Coor"];
                        string s = o.ToString();
                        string[] m = s.Split(';');
                        List<Vector2> p = new List<Vector2>();
                        foreach (var t in m)
                        {
                            if (t.Length > 1)
                            {
                                p.Add(new Vector2(int.Parse(t.Split(',')[0]), int.Parse(t.Split(',')[1])));
                            }
                        }
                        l2.Add(p);

                        obj["CityRegistry"]["Facilities"][i]["Coor"] = JArray.Parse("[ {} ]");


                    }
                   

                    for (int i = 0; i < obj["Roads"].Count(); i++)
                    {
                        var o = obj["Roads"][i]["Coor"];
                        string s = o.ToString();
                        string[] m = s.Split(';');
                        List<Vector2> p = new List<Vector2>();
                        foreach (var t in m)
                        {
                            if (t.Length > 1)
                            {
                                p.Add(new Vector2(int.Parse(t.Split(',')[0]), int.Parse(t.Split(',')[1])));
                            }
                        }
                        l3.Add(p);

                        obj["Roads"][i]["Coor"] = JArray.Parse("[]");


                    }
                   
                  


                    jsonString = obj.ToString();
                    instance = JsonSerializer.Deserialize<GameModel>(jsonString)!;
                    var ins = GetInstance();

                    for (int i = 0; i < instance.CityRegistry.Zones.Count; i++)
                    {
                        instance.CityRegistry.Zones[i].Coor = l1[i];
                    }
                    for (int i = 0; i < instance.CityRegistry.Facilities.Count; i++)
                    {
                        instance.CityRegistry.Facilities[i].Coor = l2[i];
                    }
                    
                    for (int i = 0; i < instance.Roads.Count; i++)
                    {
                        instance.Roads[i].Coor = l3[i];
                    }



                    foreach (var z in instance.CityRegistry.Zones)
                    {
                        Vector2 v = z.Coor[0];
                        instance.map[(int)v.X, (int)v.Y] = z;
                    }
                    foreach (var r in instance.Roads)
                    {
                        Vector2 v = r.Coor[0];
                        instance.map[(int)v.X, (int)v.Y] = r;
                    }
                    foreach (var f in instance.CityRegistry.Facilities)
                    {
                        Vector2 v = f.Coor[0];
                        instance.map[(int)v.X, (int)v.Y] = f;
                    }


                }
                else
                {
                    Debug.WriteLine($"No saved game found in slot {slot}.");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"An error occurred while reading the game model from slot {slot}: " + ex.Message);
            }
        }
    }
}
