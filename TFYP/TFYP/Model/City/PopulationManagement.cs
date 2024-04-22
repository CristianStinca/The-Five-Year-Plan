using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TFYP.Model.Facilities;
using TFYP.Model.Zones;
using TFYP.Model.Common;

namespace TFYP.Model.City
{
    [Serializable]
    internal class PopulationManagement
    {
        /*
        private CityRegistry cityRegistry;

        public PopulationManagement(CityRegistry cityRegistry)
        {
            this.cityRegistry = cityRegistry;
        }

        // Performs actions related to the aging and retiring of citizens.
        public void Census(GameModel gm)
        {
            foreach (var c in cityRegistry.GetAllCitizens())
            {
                c.IncAge();
            }

            foreach (var retire in GetListOfRetired())
            {
                if (ProbabilitySelector.Decision(retire.Age / 100.0))
                {
                    try
                    {
                        Console.WriteLine("One citizen died");
                        Die(retire, gm);
                    }
                    catch (NullReferenceException) // Changed from NullPointerException
                    {
                        // Ignored
                    }
                    CitizenLifecycle.CreateYoungCitizen(gm);
                }
            }

            foreach (var worker in GetListOfWorkForce())
            {
                if (worker.GetSatisfaction(gm) < Constants.CITIZEN_LEAVING_SATISFACTION)
                {
                    Console.WriteLine("One citizen left");
                    try
                    {
                        Die(worker, gm);
                    }
                    catch (NullReferenceException) // Changed from NullPointerException
                    {
                        // Ignored
                    }
                }
            }
        }

        private List<Citizen> GetListOfRetired()
        {
            return cityRegistry.GetAllCitizens().Where(citizen => citizen.Age >= 65).ToList();
        }

        private List<Citizen> GetListOfWorkForce()
        {
            return cityRegistry.GetAllCitizens().Where(citizen => citizen.Age < 65).ToList();
        }

        private void Die(Citizen dead, GameModel gm)
        {
            dead.LivingPlace.RemoveCitizen(dead, gm);
            dead.Workplace?.RemoveCitizen(dead, gm); // Using the null-conditional operator for safety
        }

        // Calculates the pension expenses.
        public int PayPension()
        {
            int total = GetListOfRetired().Sum(retire => retire.Pension);
            Console.WriteLine($"Social Security: Paid {total} pension to {GetListOfRetired().Count} retirees");
            return total;
        }

        // Calculates the total tax revenue.
        public double CollectTax(double taxRate)
        {
            double total = GetListOfWorkForce().Sum(c => c.PayTax(taxRate));
            Console.WriteLine($"Social Security: Collected tax {total} from {GetListOfWorkForce().Count} workers");
            return total;
        }
    }
        */
    }
}
