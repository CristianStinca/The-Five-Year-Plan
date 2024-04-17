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
        public Sprite SchoolTile { get => ifLoaded(SchoolTile); private set => SchoolTile = value; }
        public Sprite PoliceTile { get => ifLoaded(PoliceTile); private set => PoliceTile = value; }
        public Sprite ResidentialTile { get => ifLoaded(ResidentialTile); private set => ResidentialTile = value; } // must be changed
        public Sprite IndustrialTile { get => ifLoaded(IndustrialTile); private set => IndustrialTile = value; } // must be changed
        public Sprite ServiceTile { get => ifLoaded(EmptyTile); private set => EmptyTile = value; } // must be changed
        public Sprite RoadTile { get => ifLoaded(RoadTile); private set => RoadTile = value; }
        public Sprite DoneResidentialTile { get => ifLoaded(DoneResidentialTile); private set => DoneResidentialTile = value; }

        /// <summary>
        /// Checks if the texture was loaded (not null).
        /// </summary>
        /// <typeparam name="T">The type to be checked.</typeparam>
        /// <param name="val">The value to check.</param>
        /// <returns>The given value without changes.</returns>
        /// <exception cref="InvalidCastException">Raised if the type is not an object.</exception>
        /// <exception cref="ArgumentNullException">Raised if the type is not set (null).</exception>
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
