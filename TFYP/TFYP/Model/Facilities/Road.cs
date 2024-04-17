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
        public Road(Vector2 _coor, EBuildable _type) : base(_coor, _type)
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
            if ((int)this.coor.X % 2 == 0)
            {
                connected.Add(gm.map[(int)this.coor.X - 1, (int)coor.Y-1]);
                connected.Add(gm.map[(int)this.coor.X - 1, (int)coor.Y + 1]);
                connected.Add(gm.map[(int)coor.X + 1, (int)coor.Y-1]);
                connected.Add(gm.map[(int)coor.X + 1, (int)coor.Y + 1]);
            }
            else
            {
                connected.Add(gm.map[(int)this.coor.X - 1, (int)coor.Y]);
                connected.Add(gm.map[(int)this.coor.X - 1, (int)coor.Y + 1]);
                connected.Add(gm.map[(int)coor.X + 1, (int)coor.Y]);
                connected.Add(gm.map[(int)coor.X + 1, (int)coor.Y + 1]);
            }
        }
        public bool isConnected(Buildable elem)
        { 
            return this.connected.Contains(elem);
        }


    }
}
