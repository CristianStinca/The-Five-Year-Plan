using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TFYP.Model.GameObjects;
using TFYP.Model.GameObjects.Facilities;
using Microsoft.Xna.Framework;

namespace TFYP.Model
{
    internal class GameModel
    {
        public static readonly int MAP_H = 20;
        public static readonly int MAP_W = 20;
        public Buildable[,] map {  get; private set; }

        public GameModel()
        {
            map = new Buildable[MAP_H, MAP_W];

            for (int i = 0; i < map.GetLength(0); i++)
                for (int j = 0; j < map.GetLength(1); j++)
                    map[i,j] = new Buildable(new Vector2(i, j));
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
    }
}
