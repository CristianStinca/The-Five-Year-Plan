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


namespace TFYP.Model
{
    /* Made this class serializable to save the current state of the game, including player progress, game settings, and the world state, so that it can be paused and resumed */
    
    [Serializable]
    public class GameModel
    {        
        private static int _mapH, _mapW;
        public Buildable[,] map;
        public List<Citizen> Citizens { get; private set; }
        public List<Zone> Zones { get; private set; }
        public DateTime CreationDate; // DateTime built-in in c#
        public Statistics Statistics { get; private set; }
        public CityRegistry CityRegistry { get; private set; }
        public List<Road> Roads { get; private set; }

        public GameModel(int _mapH, int _mapW)
        {
            MAP_H = _mapH;
            MAP_W = _mapW;
            map = new Buildable[_mapH, _mapW];
            Budget budget = new Budget(Constants.InitialBalance, Constants.TaxRate);
            Statistics = new Statistics(budget);
            CityRegistry = new CityRegistry(Statistics);
            Citizens = new List<Citizen>();
            Zones = new List<Zone>();
            CreationDate = DateTime.Now; // Year, Month, Day - we will change date later
            Roads = new List<Road>();

            for (int i = 0; i < map.GetLength(0); i++)
                for (int j = 0; j < map.GetLength(1); j++)
                    map[i, j] = new Buildable(new Vector2(i, j));
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
        
        //Adds the zone to the city
        public void AddZone(int _x, int _y, EBuildable zone)
        {
            // TO DO: after adding a zone, roads should be checked, where is it connected now, and what effect did building of this zone cause
            
            AddToMap(_x, _y, zone);
            //cityRegistry.AddZone(zone);
            //cityRegistry.UpdateBalance(-zone.GetOneTimeCost(), GetCurrentDate());
        }
        private void AddToMap(int _x, int _y, EBuildable zone) {

            //left as x and y for now, can be changed to coordinate later

            //map[_x, _y] = new Buildable(new Vector2(_x, _y), zone.type);

            switch (zone)
            {
                case EBuildable.Stadium:
                    map[_x, _y] = new Stadium(new Vector2(_x, _y), zone);
                    break;

                case EBuildable.None:
                    map[_x, _y] = new Buildable(new Vector2(_x, _y), zone);
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
            foreach (var zone in Zones)
            {
                ApplyDisasterToZone(disaster, zone);
            }
            UpdateAfterDisaster();
        }
        // we might also need a method to update the game world after the disaster effects
        public void UpdateAfterDisaster()
        {
            // update game world state here, like repairing buildings, updating citizen satisfaction, and so on
        }
    }
}
