using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TFYP.Model.Facilities;
using TFYP.Model.City;
using TFYP.Model.Zones;

namespace TFYP.Model.Common
{
    public class Budget
    {
        public int Balance { get; private set; }

        public Budget()
        {
            Balance = 0;
        }

        public void UpdateBalance(int amount)
        {
            Balance += amount;
        }
    }
}
