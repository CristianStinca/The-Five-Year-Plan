using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TFYP.Utils;
using TFYP.View.Renders;
using TFYP.View.Windows;

namespace TFYP.View
{
    internal class View
    {
        public Window CurrentWindow { get; set; }

        //public Dictionary<string, string> objectsFileNames { get; set; }

        public View() 
        {
            this.CurrentWindow = GameWindow.Instance; // to be changed to MenuWindow();
        }

        public void Update()
        {
            this.CurrentWindow.Update(); // 
        }

        public void Draw(IRenderer renderer)
        {
            this.CurrentWindow.Draw(renderer);
        }
    }
}
