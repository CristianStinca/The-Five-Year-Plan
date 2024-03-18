using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TFYP.Model.GameObjects;
using TFYP.Model.GameObjects.Facilities;

namespace TFYP.Model
{
    internal class GameModel
    {
        public Buildable[,] map {  get; private set; }

        public GameModel()
        {
            map = new Buildable[20, 20];

            for (int i = 0; i < map.GetLength(0); i++)
                for (int j = 0; j < map.GetLength(1); j++)
                    map[i,j] = new Buildable(new Coordinate(i, j));
        }

        public void Build(int _x, int _y, EBuildable _type)
        {
            switch (_type)
            {
                case EBuildable.Stadium:
                    map[_x, _y] = new Stadium(new Coordinate(_x, _y), Dimension.DEFAULT, _type);
                    break;
            }
        }
    }
}
