using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TFYP.Model.Facilities;
using TFYP.Model.Common;
using TFYP.Model.Zones;

namespace TFYP.Model.City
{
    internal class Statistics
    {
        private int population;
        private int satisfaction;
        private int capacity;

        // Volatile is used for concurrency
        public int Population
        {
            get => Volatile.Read(ref population);
            set => Volatile.Write(ref population, value);
        }

        public int Capacity
        {
            get => Volatile.Read(ref capacity);
            set => Volatile.Write(ref capacity, value);
        }

        public int Satisfaction
        {
            get => Volatile.Read(ref satisfaction);
            set => Volatile.Write(ref satisfaction, value);
        }
    }
}
