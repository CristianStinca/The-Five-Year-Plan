using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TFYP.Model.Common;
using TFYP.Model.City;
using TFYP.Model.Zones;
using System.Drawing;

namespace TFYP.Model.Facilities
{
    public class Road : Facility
    {
        public bool IsConnected {  get; private set; }

        public List<Buildable> connected;
        public Road(List<Vector2> _coor, EBuildable _type) : base(_coor, _type)
        {
            IsConnected = false;
            MaintenanceCost = Constants.RoadMaintenanceFee;
            ConstructionCost = Constants.RoadBuildCost;
            connected=new List<Buildable>();
            // ConstructionTime = Constants.; // Must be added in Constants.cs
        }
        public void checkForZones() {
            GameModel gm = GameModel.GetInstance();
            connected.Clear();
            if ((int)this.Coor[0].X % 2 == 0)
            {
                connected.Add(gm.map[(int)this.Coor[0].X - 1, (int)Coor[0].Y-1]);
                connected.Add(gm.map[(int)this.Coor[0].X + 1, (int)Coor[0].Y - 1]);
                connected.Add(gm.map[(int)Coor[0].X-1 , (int)Coor[0].Y]);
                connected.Add(gm.map[(int)Coor[0].X+1, (int)Coor[0].Y]);
            }
            else
            {
                connected.Add(gm.map[(int)this.Coor[0].X - 1, (int)Coor[0].Y+1]);
                connected.Add(gm.map[(int)this.Coor[0].X + 1, (int)Coor[0].Y + 1]);
                connected.Add(gm.map[(int)Coor[0].X + 1, (int)Coor[0].Y]);
                connected.Add(gm.map[(int)Coor[0].X - 1, (int)Coor[0].Y]);
            }
        }
        public bool isConnected(Buildable elem)
        { 
            return this.connected.Contains(elem);
        }

        public bool connection(Buildable dest) {
            HashSet<Buildable> visited = new HashSet<Buildable>();
            Queue<Buildable> queue = new Queue<Buildable>();

            visited.Add(this);
            queue.Enqueue(this);

            while (queue.Count > 0)
            {
                Buildable currentBuildable = queue.Dequeue();

                
                if (currentBuildable == dest)
                {
                    return true;
                }

                
                foreach (Buildable neighbor in ((Road)currentBuildable).connected)
                {
                    if (!visited.Contains(neighbor))
                    {
                        visited.Add(neighbor);
                        queue.Enqueue(neighbor);
                    }
                }
            }

            
            return false;
        }


    }
}
