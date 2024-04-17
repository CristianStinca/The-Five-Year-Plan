using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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

        public Sprite EmptyTile { get; set; }//{ get => ifLoaded(EmptyTile); private set => EmptyTile = value; }
        public Sprite StadiumTile { get; set; } //{ get => ifLoaded(StadiumTile); private set => StadiumTile = value; }
        public Sprite SchoolTile { get; set; }
        public Sprite PoliceTile { get; set; }
        public Sprite ResidentialTile { get; set; }
        public Sprite IndustrialTile { get; set; }
        public Sprite ServiceTile { get; set; }
        public Sprite RoadTile { get; set; }
        public Sprite DoneResidentialTile { get; set; }

        #endregion

        public UIObjects()
        {
            EmptyTile = CreateSprite("Tiles/empty_tile");
            StadiumTile = CreateSprite("Tiles/stadium_tile3");
            SchoolTile = CreateSprite("Tiles/school");
            PoliceTile = CreateSprite("Tiles/police_station");
            ResidentialTile = CreateSprite("Grasses/grass1"); // must be changed
            IndustrialTile = CreateSprite("Tiles/school"); // must be changed
            ServiceTile = CreateSprite("Tiles/empty_tile"); // must be changed
            RoadTile = CreateSprite("Roads/road2"); // must be changed this is just for testing
            DoneResidentialTile = CreateSprite("Tiles/police_station");

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

        //private static Button CreateButton(string buttonNormal, string buttonHover, Vector2 position)
        //{
        //    Texture2D startNormal = Globals.Content.Load<Texture2D>(buttonNormal);
        //    Texture2D startHover = Globals.Content.Load<Texture2D>(buttonHover);
        //    Sprite startSprite = new Sprite(startNormal, position);
        //    Button newButton = new Button(startSprite, startHover, startNormal);
        //    return newButton;
        //}

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
