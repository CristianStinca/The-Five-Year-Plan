﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TFYP.Controller.WindowsControllers;
using TFYP.Utils;
using TFYP.View.Renders;
using TFYP.View.UIElements;
using TFYP.View.Windows;

namespace TFYP.View
{
    internal class View
    {
        public Window CurrentWindow { get; set; }

        protected IUIElements _UIElements;

        public View(IUIElements UIElements) 
        {
            //this.CurrentWindow = new GameWindow(UIElements); // to be changed to MenuWindow.Instance;
            this._UIElements = UIElements;
        }

        public void changeToGameWindow()
        {
            this.CurrentWindow = new GameWindow(_UIElements);
        }

        public void changeToMenuWindow()
        {
            this.CurrentWindow = new MenuWindow(_UIElements);
        }

        public void Update()
        {
            this.CurrentWindow.Update();
        }

        public void Draw(IRenderer renderer)
        {
            this.CurrentWindow.Draw(renderer);
        }
    }
}
