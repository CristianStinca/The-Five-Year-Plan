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
            StadiumTile = CreateSprite("StadiumParts/stadium_full", "2D/stadium2d");
            SchoolTile = CreateSprite("Tiles/school_double", "2D/school2d");
            SchoolTile_r = CreateSprite("Tiles/school_double_rotated", "2D/school2d_r");
            UniTile = CreateSprite("Tiles/university_quadruple", "2D/university2d");
            PoliceTile = CreateSprite("Tiles/police_station", "2D/police2d");
            ResidentialTile = CreateSprite("Zones/residential_empty", "2D/residential2d");
            ResidentialTile11 = CreateSprite("Zones/residential11", "2D/residential2d");
            ResidentialTile12 = CreateSprite("Zones/residential12", "2D/residential2d");
            ResidentialTile13 = CreateSprite("Zones/residential13", "2D/residential2d");
            ResidentialTile2 = CreateSprite("Zones/residential2", "2D/residential2d");
            ResidentialTile3 = CreateSprite("Zones/residential3", "2D/residential2d");
            IndustrialTile = CreateSprite("Zones/industrial_empty", "2D/industrial2d");
            IndustrialTile11 = CreateSprite("Zones/industrial11", "2D/industrial2d");
            IndustrialTile12 = CreateSprite("Zones/industrial12", "2D/industrial2d");
            IndustrialTile13 = CreateSprite("Zones/industrial13", "2D/industrial2d");
            IndustrialTile2 = CreateSprite("Zones/industrial2", "2D/industrial2d");
            IndustrialTile3 = CreateSprite("Zones/industrial3", "2D/industrial2d");
            ServiceTile = CreateSprite("Zones/service_empty", "2D/service2d");
            ServiceTile11 = CreateSprite("Zones/service11", "2D/service2d");
            ServiceTile12 = CreateSprite("Zones/service12", "2D/service2d");
            ServiceTile13 = CreateSprite("Zones/service13", "2D/service2d");
            ServiceTile2 = CreateSprite("Zones/service2", "2D/service2d");
            ServiceTile3 = CreateSprite("Zones/service3", "2D/service2d");
            DoneResidentialTile = CreateSprite("Tiles/police_station");
            //Inaccessible = CreateSprite("Tiles/innac");
            Inaccessible = CreateSprite("Grasses/grass4");
            IndustrialTile_b = CreateSprite("Zones/industrial_building", "2D/industrial2d");
            ResidentialTile_b = CreateSprite("Zones/residential_building", "2D/residential2d");
            ServiceTile_b = CreateSprite("Zones/service_building", "2D/service2d");

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
            return CreateSprite(fileName, fileName);
        }
        
        private static Sprite CreateSprite(string fileName1, string fileName2)
        {
            var texture1 = Globals.Content.Load<Texture2D>(fileName1);
            var texture2 = Globals.Content.Load<Texture2D>(fileName2);
            Sprite sprite = new Sprite(new[] { texture1, texture2 });
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
