using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TFYP.Model.Common;
using TFYP.Model.City;
using TFYP.Model.Zones;

namespace TFYP.Model.Facilities
{
    public abstract class Facility : Buildable
    {
        public int CurrentCapacity { get; set; }

        // did not remove this just in case...
        public Facility(Vector2 _coor, EBuildable _type) : base(_coor, _type)
        {
            CurrentCapacity = 0;

        }

        public void IncreaseCapacity(int amount)
        {
            if (amount < 0)
                throw new ArgumentException("Amount cannot be negative.", nameof(amount));

            CurrentCapacity = Math.Min(CurrentCapacity + amount, Capacity);
        }
        public void DecreaseCapacity(int amount)
        {
            if (amount < 0)
                throw new ArgumentException("Amount cannot be negative.", nameof(amount));

            CurrentCapacity = Math.Max(CurrentCapacity - amount, 0);
        }

        public void SetCapacity(int newCapacity)
        {
            if (newCapacity < 0 || newCapacity > Capacity)
                throw new ArgumentOutOfRangeException(nameof(newCapacity), "New capacity must be within valid range.");

            CurrentCapacity = newCapacity;
        }
    }
}
