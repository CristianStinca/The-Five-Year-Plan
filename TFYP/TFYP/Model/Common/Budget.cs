using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TFYP.Model.Facilities;
using TFYP.Model.City;
using TFYP.Model.Zones;
using ProtoBuf;

namespace TFYP.Model.Common
{
    [ProtoContract]
    [Serializable]
    public class Budget
    {


        [ProtoMember(1)]
        public double Balance { get; set; }
        [ProtoMember(2)]
        public int CurrentTax { get; set; }
        [ProtoMember(3)]
        public float CurrentTaxRate { get; set; }
        [ProtoMember(4)]
        public double MaintenanceFeeForEverything { get; set; }
        [ProtoMember(5)]
        public DateTime DateOfStartingLoan { get; set; }

        public Budget()
        {
            Balance = Constants.InitialBalance;
            CurrentTax = Constants.CityBaseTax;
            CurrentTaxRate = Constants.CityBaseTaxRate;
            MaintenanceFeeForEverything = 0;
            DateOfStartingLoan = DateTime.MinValue;
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
            if(DateOfStartingLoan != DateTime.MinValue && gameTime > DateOfStartingLoan)
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
            double revenue = 0;

            foreach (var residentialZone in gm.CityRegistry.Zones.Where(z => z.Type == EBuildable.Residential))
            {
                revenue += residentialZone.citizens.Where(citizen => citizen.IsActive).Sum(citizen => citizen.TaxAmount(this));
            }
            return revenue;
        
        }


        // spend not necessary as exta method, just --> MaintenanceFeeForEverything
        //და მოკლედ ყოველ ჯერზე როცა ახალ ფესილიტის დაამატებ AddToMaintenanceFee უნდა გამოიძახო
        //და როცა მოაშორებ RemoveFromMaintenanceFee

    }
}
