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

        public double Balance { get; set; }
        public double CurrentTax { get; private set; }
        public double MaintenanceFeeForEverything { get; private set; }
        public DateTime DateOfStartingLoan { get; private set; }

        public Budget()
        {
            Balance = Constants.InitialBalance;
            CurrentTax = Constants.CityBaseTax;
            MaintenanceFeeForEverything = 0;
        }

        public void UpdateTax(double tax)
        {
            CurrentTax = tax;
        }

        public void UpdateBalance(double amount, DateTime gameTime)
        {
            Balance += amount;
            if (Balance < 0)
            {
                DateOfStartingLoan = gameTime;
            }
        }

        public int YearsOfBeingInLoan(DateTime gameTime)
        {
            if (gameTime > DateOfStartingLoan)
            {
                return gameTime.Year - DateOfStartingLoan.Year - (gameTime.DayOfYear < DateOfStartingLoan.DayOfYear ? 1 : 0);
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
