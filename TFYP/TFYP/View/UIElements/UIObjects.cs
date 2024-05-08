using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using TFYP.Controller;
using TFYP.Model.Facilities;
using TFYP.Utils;
using static System.Net.Mime.MediaTypeNames;
using static System.Net.WebRequestMethods;

namespace TFYP.View.UIElements
{
    internal class UIObjects : IUIElements
    {
        #region OBJECTS_DECLARATIONS

        private List<Sprite> _emptyTiles = new();
        public Sprite EmptyTile { get; set; }
        public Sprite StadiumTile { get; set; }
        public Sprite SchoolTile { get; set; }
        public Sprite SchoolTile_r { get; set; }
        public Sprite UniTile { get; set; }
        public Sprite PoliceTile { get; set; }
        public Sprite ResidentialTile { get; set; }
        public Sprite ResidentialTile11 { get; set; }
        public Sprite ResidentialTile12 { get; set; }
        public Sprite ResidentialTile13 { get; set; }
        public Sprite ResidentialTile2 { get; set; }
        public Sprite ResidentialTile3 { get; set; }
        public Sprite IndustrialTile { get; set; }
        public Sprite IndustrialTile11 { get; set; }
        public Sprite IndustrialTile12 { get; set; }
        public Sprite IndustrialTile13 { get; set; }
        public Sprite IndustrialTile2 { get; set; }
        public Sprite IndustrialTile3 { get; set; }
        public Sprite ServiceTile { get; set; }
        public Sprite ServiceTile11 { get; set; }
        public Sprite ServiceTile12 { get; set; }
        public Sprite ServiceTile13 { get; set; }
        public Sprite ServiceTile2 { get; set; }
        public Sprite ServiceTile3 { get; set; }

        public Sprite[] RoadTiles = new Sprite[16];
        public Sprite DoneResidentialTile { get; set; }
        public Sprite Inaccessible { get; set; }
        public Sprite ResidentialTile_b { get; set; }
        public Sprite IndustrialTile_b { get; set; }
        public Sprite ServiceTile_b { get; set; }

        #endregion

        private OpenSimplexNoise noise;

        public UIObjects()
        {
            noise = new OpenSimplexNoise();

            EmptyTile = CreateSprite("Grasses/grass1");
            _emptyTiles.Add(CreateSprite("Grasses/grass3"));
            _emptyTiles.Add(CreateSprite("Grasses/grass1"));
            _emptyTiles.Add(CreateSprite("Grasses/grass2"));
            _emptyTiles.Add(CreateSprite("Grasses/grass5"));

            //StadiumTile = CreateSprite("Tiles/stadium_tile");
            StadiumTile = CreateSprite("StadiumParts/stadium_full");
            SchoolTile = CreateSprite("Tiles/school_double");
            SchoolTile_r = CreateSprite("Tiles/school_double_rotated");
            UniTile = CreateSprite("Tiles/university");
            PoliceTile = CreateSprite("Tiles/police_station");
            ResidentialTile = CreateSprite("Zones/residential_empty");
            ResidentialTile11 = CreateSprite("Zones/residential11");
            ResidentialTile12 = CreateSprite("Zones/residential12");
            ResidentialTile13 = CreateSprite("Zones/residential13");
            ResidentialTile2 = CreateSprite("Zones/residential2");
            ResidentialTile3 = CreateSprite("Zones/residential3");
            IndustrialTile = CreateSprite("Zones/industrial_empty");
            IndustrialTile11 = CreateSprite("Zones/industrial11");
            IndustrialTile12 = CreateSprite("Zones/industrial12");
            IndustrialTile13 = CreateSprite("Zones/industrial13");
            IndustrialTile2 = CreateSprite("Zones/industrial2");
            IndustrialTile3 = CreateSprite("Zones/industrial3");
            ServiceTile = CreateSprite("Zones/service_empty");
            ServiceTile11 = CreateSprite("Zones/service11");
            ServiceTile12 = CreateSprite("Zones/service12");
            ServiceTile13 = CreateSprite("Zones/service13");
            ServiceTile2 = CreateSprite("Zones/service2");
            ServiceTile3 = CreateSprite("Zones/service3");
            DoneResidentialTile = CreateSprite("Tiles/police_station");
            //Inaccessible = CreateSprite("Tiles/innac");
            Inaccessible = CreateSprite("Grasses/grass4");
            IndustrialTile_b = CreateSprite("Zones/industrial_building");
            ResidentialTile_b = CreateSprite("Zones/residential_building");
            ServiceTile_b = CreateSprite("Zones/service_building");

            RoadTiles[0b_0000] = CreateSprite("Roads/eroad");
            RoadTiles[0b_0001] = CreateSprite("Roads/deadend4");
            RoadTiles[0b_0010] = CreateSprite("Roads/deadend1");
            RoadTiles[0b_0011] = CreateSprite("Roads/curve1");
            RoadTiles[0b_0100] = CreateSprite("Roads/deadend2");
            RoadTiles[0b_0101] = CreateSprite("Roads/road1");
            RoadTiles[0b_0110] = CreateSprite("Roads/curve4");
            RoadTiles[0b_0111] = CreateSprite("Roads/triple2");
            RoadTiles[0b_1000] = CreateSprite("Roads/deadend3");
            RoadTiles[0b_1001] = CreateSprite("Roads/curve2");
            RoadTiles[0b_1010] = CreateSprite("Roads/road2");
            RoadTiles[0b_1011] = CreateSprite("Roads/triple3");
            RoadTiles[0b_1100] = CreateSprite("Roads/curve3");
            RoadTiles[0b_1101] = CreateSprite("Roads/triple4");
            RoadTiles[0b_1110] = CreateSprite("Roads/triple1");
            RoadTiles[0b_1111] = CreateSprite("Roads/quadruple");
        }

        public Sprite getGrass(int x, int y)
        {
            double level = noise.Evaluate(x, y) + 1;
            int nr = _emptyTiles.Count;

            if (nr == 0)
                throw new ArgumentNullException();

            for (int i = 1; i <= nr; i++)
            {
                if (level <= (i/(float)nr) * 2f)
                {
                    return _emptyTiles[i-1];
                }
            }

            throw new ArithmeticException();
        }

        private T ifLoaded<T>(T val)
        {
            if (!(val is object))
            {
                throw new InvalidCastException(nameof(val));
            }

            if (val == null)
            {
                throw new ArgumentNullException(nameof(val));
            }

            return val;
        }

        /// <summary>
        /// Creates a sprite from the given file.
        /// </summary>
        /// <param name="fileName">The string of the file name</param>
        /// <returns>The Sprite object.</returns>
        private static Sprite CreateSprite(string fileName)
        {
            var texture = Globals.Content.Load<Texture2D>(fileName);
            Sprite sprite = new Sprite(texture);
            return sprite;
        }

        #region SINGLETON_DECLARATION

        //private static readonly Lazy<UIObjects> lazy = new Lazy<UIObjects>(() => new UIObjects());
        //public static UIObjects Instance
        //{
        //    get
        //    {
        //        return lazy.Value;
        //    }
        //}

        #endregion
    }
}
