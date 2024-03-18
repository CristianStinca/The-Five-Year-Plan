using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TFYP.Utils;

namespace TFYP.View.Windows
{
    internal sealed class MenuWindow : Window
    {
        public MenuWindow() : base()
        {
            
        }

        private static readonly Lazy<MenuWindow> lazy = new Lazy<MenuWindow>(() => new MenuWindow());
        public static MenuWindow Instance
        {
            get
            {
                return lazy.Value;
            }
        }
    }
}
