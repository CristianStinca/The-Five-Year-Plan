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
    internal class Budget
    {
        public double Balance { get; private set; }
        public double TaxRate { get; private set; }
        public double TotalMaintenanceFee { get; private set; }

        public Budget(double balance = 0, double taxRate = 0)
        {
            Balance = balance;
            TaxRate = taxRate;
            TotalMaintenanceFee = 0;
        }

        public void SetTaxRate(double taxRate)
        {
            TaxRate = taxRate;
        }

        public void UpdateBalance(double amount)
        {
            Balance += amount;
        }

        public void UpdateMaintenanceFee(double maintenanceFee)
        {
            TotalMaintenanceFee += maintenanceFee;
        }

    }
}
