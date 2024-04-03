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
        public CityRegistry cityRegistry;
        public Statistics statistics;
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

        
        public void Build(int _x, int _y, EBuildable _type)
        {
            switch (_type)
            {
                case EBuildable.Stadium:
                    map[_x, _y] = new Stadium(new Vector2(_x, _y), _type);
                    break;

                case EBuildable.None:
                    map[_x, _y] = new Buildable(new Vector2(_x, _y), _type);
                    break;
            }
        }




        //Adds the zone to the city
        /*
        public void AddZone(Zone zone)
        {
            
            AddToMap(zone);
            cityRegistry.AddZone(zone);
            cityRegistry.UpdateBalance(-zone.GetOneTimeCost(), GetCurrentDate());
        }
        */

        
        //private void AddToMap(Zone zone) { /* Implementation */ }


        //private DateTime GetCurrentDate() { return DateTime.Now; /* Implementation might differ */ }







    }
}
