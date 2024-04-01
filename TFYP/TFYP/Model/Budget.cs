using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TFYP.Model
{
    internal class Budget
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
