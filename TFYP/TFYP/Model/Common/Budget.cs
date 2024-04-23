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

        public double Balance { get; private set; }
        public double CurrentTaxRate { get; private set; }
        public double MaintenanceFeeForEverything { get; private set; }
        public DateTime DateOfStartingLoan { get; private set; }

        public Budget(double balance = 0, double taxRate = 0)
        {
            Balance = balance;
            CurrentTaxRate = taxRate;
            MaintenanceFeeForEverything = 0;
        }

        public void SetCurrentTaxRate(double taxRate)
        {
            CurrentTaxRate = taxRate;
        }

        public void UpdateBalance(double amount)
        {
            DateTime now = DateTime.Now;
            Balance += amount;
            if (Balance < 0 && DateOfStartingLoan == DateTime.MinValue)
            {
                DateOfStartingLoan = now;
            }
        }

        public int YearsOfBeingInLoan(DateTime now)
        {
            if (DateOfStartingLoan != DateTime.MinValue && now > DateOfStartingLoan)
            {
                return now.Year - DateOfStartingLoan.Year - (now.DayOfYear < DateOfStartingLoan.DayOfYear ? 1 : 0);
            }
            return 0;
        }

        public void AddToMaintenanceFee(double maintenanceFee)
        {
            MaintenanceFeeForEverything += maintenanceFee;
        }

        public void RemoveFromMaintenanceFee(double maintenanceFee)
        {
            MaintenanceFeeForEverything -= maintenanceFee;
        }

        public double ComputeRevenue(GameModel gm)
        {
            // Sum the TaxAmount for only active citizens
            return gm.CityRegistry.Citizens.Where(citizen => citizen.IsActive).Sum(citizen => citizen.TaxAmount(this));
        }


        // spend not necessary as exta method, just --> MaintenanceFeeForEverything
        //და მოკლედ ყოველ ჯერზე როცა ახალ ფესილიტის დაამატებ AddToMaintenanceFee უნდა გამოიძახო
        //და როცა მოაშორებ RemoveFromMaintenanceFee

    }
}
