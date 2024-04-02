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



namespace TFYP.Model
{
    /* Made this class serializable to save the current state of the game, including player progress, game settings, and the world state, so that it can be paused and resumed */
    
    [Serializable]
    public class GameModel
    {
        

        
        private static int _mapH, _mapW;
        public Buildable[,] map;
        //public CityRegistry cityRegistry;
        //public Statistics statistics;
        public DateTime dateOfWorld; // DateTime built-in in c#
      

        public GameModel(int _mapH, int _mapW)
        {
            _mapH = _mapH;
            _mapW = _mapW;
            map = new Buildable[_mapH, _mapW]; 
            //statistics = new Statistics(new Budget(Constants.InitialCityBalance, 0.3f));
            //cityRegistry = new CityRegistry(cityStatistics);
            dateOfWorld = new DateTime(2024, 1, 1); // Year, Month, Day - we will change date later


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
        
        
        public void AddZone(int _x, int _y, Zone zone)
        {
            // TO DO: after adding a zone, roads should be checked, where is it connected now, and what effect did building of this zone cause
            
            AddToMap(_x, _y, zone);
            //cityRegistry.AddZone(zone);
            //cityRegistry.UpdateBalance(-zone.GetOneTimeCost(), GetCurrentDate());
        }
        
        
        
        private void AddToMap(int _x, int _y, Zone zone) {

            //left as x and y for now, can be changed to coordinate later

            //map[_x, _y] = new Buildable(new Vector2(_x, _y), zone.type);

            switch (zone.type)
            {
                case EBuildable.Stadium:
                    map[_x, _y] = new Stadium(new Vector2(_x, _y), zone.type);
                    break;

                case EBuildable.None:
                    map[_x, _y] = new Buildable(new Vector2(_x, _y), zone.type);
                    break;
            }
        }

        







    }
}
