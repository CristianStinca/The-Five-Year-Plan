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
using ProtoBuf;

namespace TFYP.Model.Facilities
{
    [ProtoContract]
    [Serializable]

    public class Road : Facility
    {
        [ProtoMember(1)]
        public bool IsConnected {  get;  set; }
        [ProtoMember(2)]
        public List<Buildable> connected;
        public Road() { }
        public Road(List<Vector2> _coor, EBuildable _type) : base(_coor, _type)
        {
            IsConnected = false;
            MaintenanceCost = Constants.RoadMaintenanceFee;
            ConstructionCost = Constants.RoadBuildCost;
            connected=new List<Buildable>();
        }
        /// <summary>
        /// Checks for neighboring zones connected to this zone and adds them to the list of connected zones.
        /// </summary>
        public void checkForZones() {
            GameModel gm = GameModel.GetInstance();
            connected = new List<Buildable>(); 
            if ((int)this.Coor[0].X % 2 == 0)
            {
                connected.Add(gm.map[(int)this.Coor[0].X - 1, (int)Coor[0].Y - 1]);
                connected.Add(gm.map[(int)this.Coor[0].X + 1, (int)Coor[0].Y - 1]);
                connected.Add(gm.map[(int)Coor[0].X - 1, (int)Coor[0].Y]);
                connected.Add(gm.map[(int)Coor[0].X + 1, (int)Coor[0].Y]);
            }
            else
            {
                connected.Add(gm.map[(int)this.Coor[0].X - 1, (int)Coor[0].Y + 1]);
                connected.Add(gm.map[(int)this.Coor[0].X + 1, (int)Coor[0].Y + 1]);
                connected.Add(gm.map[(int)Coor[0].X + 1, (int)Coor[0].Y]);
                connected.Add(gm.map[(int)Coor[0].X - 1, (int)Coor[0].Y]);
            }
        }
        public bool isConnected(Buildable elem)
        { 
            return this.connected.Exists(x => x.Coor.Contains(elem.Coor[0]));
        }
        /// <summary>
        /// Checks if there is a connection between this buildable and a destination buildable through roads.
        /// </summary>
        /// <param name="dest">The destination buildable to check for connection.</param>
        /// <returns>True if a connection exists, otherwise false.</returns>
        public bool connection(Buildable dest) {
            HashSet<Buildable> visited = new HashSet<Buildable>();
            Queue<Road> queue = new Queue<Road>();

            visited.Add(this);
            queue.Enqueue(this);

            while (queue.Count > 0)
            {
                Road currentRoad = queue.Dequeue();

                
                if (currentRoad.isConnected(dest))
                {
                    return true;
                }


                foreach (Buildable neighbor in currentRoad.connected) 
                {
                    if (!visited.Contains(neighbor) && neighbor.Type.Equals(EBuildable.Road))
                    {
                        visited.Add(neighbor);
                        queue.Enqueue((Road)neighbor);
                    }
                }
            }

            
            return false;
        }


    }
}
