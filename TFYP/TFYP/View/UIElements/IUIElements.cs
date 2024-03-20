using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TFYP.View.UIElements
{
    internal interface IUIElements
    {
        public Sprite EmptyTile { get => ifLoaded(EmptyTile); private set => EmptyTile = value; }
        public Sprite StadiumTile { get => ifLoaded(StadiumTile); private set => StadiumTile = value; }

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
    }
}
