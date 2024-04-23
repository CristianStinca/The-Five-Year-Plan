using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TFYP.Model.Facilities;
using TFYP.Model.City;
using TFYP.Model.Zones;
using TFYP.Utils;

namespace TFYP.Model.Common
{
    public enum EducationLevel
    {
        Primary,
        School,
        University
    }

    public static class EducationExtensions
    {
        public static float GetEducationValue(this EducationLevel level)
        {
            switch (level)
            {
                case EducationLevel.Primary:
                    return 50.0f;
                case EducationLevel.School:
                    return 100.0f;
                case EducationLevel.University:
                    return 150.0f;
                default:
                    throw new ArgumentOutOfRangeException(nameof(level), level, null);
            }
        }
    }


    public class Citizen
    {

        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public Zone WorkPlace { get; private set; }
        public Zone LivingPlace { get; private set; }
        public int Age { get; private set; }
        public bool IsWorking { get; private set; }
        public EducationLevel EducationLevel { get; set; }
        public float TaxPaidThisYear { get; private set; }
        public float Satisfaction { get; private set; }
        public bool IsActive { get; private set; } = true;

        private static Random random = new Random();
        private static RandomName nameGenerator = new RandomName(random);


        public Citizen(Zone livingPlace, Zone workPlace, EducationLevel educationLevel)
        {
            EducationLevel = educationLevel;
            LivingPlace = livingPlace;
            WorkPlace = workPlace;
            Age = random.Next(0, 101);
            IsWorking = WorkPlace != null;

            Sex sex = (random.Next(2) == 0) ? Sex.Male : Sex.Female; 
            string fullName = nameGenerator.Generate(sex);
            var names = fullName.Split(' ');
            FirstName = names[0];
            LastName = names[^1]; 
        }


        public void SetWorking(GameModel gm, Zone workPlace)
        {
            WorkPlace = workPlace;
            IsWorking = true;
            workPlace.AddCitizen(this, gm);
        }

        public void IncAge()
        {
            Age++;
        }

        public void LeaveCity()//სად გამოვიყენებთ?
        {
            IsActive = false;
        }


        public void CalculateSatisfaction(GameModel gm, Budget budget)
        {
            float educationSatisfaction = (float)(EducationExtensions.GetEducationValue(EducationLevel) / 150f);
            float employmentSatisfaction = IsWorking ? 1f : 0f;
            float distanceSatisfaction = 1f - (float)(GetDistanceLiveWork(gm, LivingPlace, WorkPlace) / gm.MaxDistance);
            float taxSatisfaction = 1f - (float)(TaxAmount(budget) / gm.MaxTax);

            float industrialProximityEffect = CalculateIndustrialProximityEffect(LivingPlace, gm);
            float cityFinancialHealthEffect = CalculateCityFinancialHealthEffect(budget);

            float livingPlaceSatisfaction = (float)LivingPlace.GetZoneSatisfaction(gm) - industrialProximityEffect;
            float workPlaceSatisfaction = WorkPlace != null ? (float)WorkPlace.GetZoneSatisfaction(gm) : 1f;

            // Calculate overall satisfaction
            Satisfaction = (educationSatisfaction * Constants.EducationWeight) +
                           (employmentSatisfaction * Constants.EmploymentWeight) +
                           (distanceSatisfaction * Constants.DistanceWeight) +
                           (taxSatisfaction * Constants.TaxWeight) +
                           ((livingPlaceSatisfaction + workPlaceSatisfaction) / 2 * Constants.ZoneSatisfactionWeight) +
                           cityFinancialHealthEffect;


            Satisfaction = Math.Clamp(Satisfaction * 100, 0f, 100f);

        }

        private float CalculateIndustrialProximityEffect(Zone livingPlace, GameModel gm)
        {
            if (livingPlace.Coor.Count > 0) 
            {
                double distanceToNearestIndustrial = gm.GetDistanceToNearestIndustrialArea(livingPlace.Coor[0]);
                return (float)(-0.5f * (1f - (distanceToNearestIndustrial / gm.MaxDistance)));
            }
            return 0; // if no coordinates are available
        }



        private float CalculateCityFinancialHealthEffect(Budget budget)
        {
            if (budget.Balance < 0)
            {
                // More negative impact as the loan size increases and the duration of the negative budget extends
                int yearsNegative = budget.YearsOfBeingInLoan(DateTime.Now);
                return (float)(-0.1 * yearsNegative * (budget.Balance / 10000));
            }
            return 0;
        }




        // Method to calculate how much tax should a citizen pay tax based on current factors
        public float TaxAmount(Budget budget)
        {
            // Calculating tax based on some constant base tax, the current tax rate, and an additional value based on education

            float tax = (float)(Constants.CityBaseTax * budget.CurrentTaxRate + EducationLevel.GetEducationValue());
            TaxPaidThisYear = tax; // Assuming tax is paid annually
            return tax;

            //taxRate is base in beginning for city and then after time it should be updated in Budges class!!
        }


        public static int GetDistanceLiveWork(GameModel gm, Zone livingPlace, Zone workplace)
        {
            return gm.CalculateDistanceBetweenZones(livingPlace, workplace);
        }



        public override string ToString()
        {
            return $"Citizen{{FirstName={FirstName}, LastName={LastName}, WorkPlace={WorkPlace}, " +
                   $"LivingPlace={LivingPlace}, Age={Age}, IsWorking={IsWorking}, " +
                   $"EducationLevel={EducationLevel}, TaxPaidThisYear={TaxPaidThisYear:F2}, " +
                   $"Satisfaction={Satisfaction:F2}, IsActive={IsActive}}}";
        }

    }


}

