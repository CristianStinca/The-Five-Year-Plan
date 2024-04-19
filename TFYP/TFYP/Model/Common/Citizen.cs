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
        //In which zones does citizen work:
        public Zone WorkPlace { get; private set; }
        public Zone LivingPlace { get; private set; }

        public int Age { get; private set; }
        public bool IsWorking { get; private set; }
        public EducationLevel EducationLevel { get; private set; }

        public float TaxPaidThisYear { get; private set; }

        private static Random random = new Random();//for age

        public float Satisfaction { get; private set; }
        public bool IsActive { get; private set; } = true; // citizens are active when created



        public Citizen(Zone livingPlace, Zone workPlace, EducationLevel educationLevel)
        {
            EducationLevel = educationLevel;
            LivingPlace = livingPlace;
            WorkPlace = workPlace;
            Age = random.Next(0, 101);
            IsWorking = WorkPlace != null;
        }


        public void SetWorking(GameModel gm, Zone workPlace)
        {
            WorkPlace = workPlace;
            IsWorking = true;
            workPlace.AddCitizen(this, gm);
        }

        public void IncAge()//სად გამოვიყენებთ?
        {
            Age++;
        }

        public void LeaveCity()//სად გამოვიყენებთ?
        {
            IsActive = false;
        }



        public void CalculateSatisfaction(GameModel gm, Budget budget)
        {
            float educationSatisfaction = (float)(EducationExtensions.GetEducationValue(EducationLevel) / 150f); // Cast if needed
            float employmentSatisfaction = IsWorking ? 1f : 0f;
            float distanceSatisfaction = 1f - (float)(GetDistanceLiveWork(gm, LivingPlace, WorkPlace) / gm.MaxDistance);
            float taxSatisfaction = 1f - (float)(TaxAmount(budget) / gm.MaxTax);

            float livingPlaceSatisfaction = (float)LivingPlace.GetZoneSatisfaction(gm);
            float workPlaceSatisfaction = WorkPlace != null ? (float)WorkPlace.GetZoneSatisfaction(gm) : 1f;

            Satisfaction = (educationSatisfaction * Constants.EducationWeight) +
                           (employmentSatisfaction * Constants.EmploymentWeight) +
                           (distanceSatisfaction * Constants.DistanceWeight) +
                           (taxSatisfaction * Constants.TaxWeight) +
                           ((livingPlaceSatisfaction + workPlaceSatisfaction) / 2 * Constants.ZoneSatisfactionWeight);

            Satisfaction = Math.Clamp(Satisfaction, 0f, 1f);
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
            return $"Citizen{{WorkPlace={WorkPlace}, " +
                $"LivingPlace={LivingPlace}, " +
                $"EducationLevel={EducationLevel}, " +
                $"Age={Age}, " +
                $"IsWorking={IsWorking}, " +
                $"TaxAmount={TaxPaidThisYear}}}";
        }

    }

}

