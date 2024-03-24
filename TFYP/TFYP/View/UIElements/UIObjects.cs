﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TFYP.Controller;
using TFYP.Utils;

namespace TFYP.View.UIElements
{
    internal class UIObjects : IUIElements
    {
        #region OBJECTS_DECLARATIONS

        public Sprite EmptyTile { get; set; }//{ get => ifLoaded(EmptyTile); private set => EmptyTile = value; }
        public Sprite StadiumTile { get; set; } //{ get => ifLoaded(StadiumTile); private set => StadiumTile = value; }

        #endregion

        public UIObjects()
        {
            EmptyTile = CreateSprite("empty_tile");
            StadiumTile = CreateSprite("stadium_tile");
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

        private static Sprite CreateSprite(string fileName)
        {
            Debug.WriteLine("sprite initialised");
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
