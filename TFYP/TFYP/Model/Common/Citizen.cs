using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TFYP.Model.Facilities;
using TFYP.Model.City;
using TFYP.Model.Zones;
using TFYP.Utils;
using System.Diagnostics;

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
        public static int GetEducationValue(this EducationLevel level)
        {
            switch (level)
            {
                case EducationLevel.Primary:
                    return 50;
                case EducationLevel.School:
                    return 75;
                case EducationLevel.University:
                    return 100;
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
        public int Satisfaction { get; private set; }
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
            Satisfaction = 50;
        }


        public void SetWorking(Zone workPlace)
        {
            WorkPlace = workPlace;
            IsWorking = true;
            workPlace.AddCitizen(this);
        }

        public void IncAge()
        {
            Age++;
        }

        public void LeaveCity()
        {
            IsActive = false;

            if (LivingPlace != null)
            {
                LivingPlace.RemoveCitizen(this);
            }
            if (WorkPlace != null)
            {
                WorkPlace.RemoveCitizen(this);
            }

        }


        public void CalculateSatisfaction(GameModel gm, Budget budget)
        {
            int educationSatisfaction = EducationExtensions.GetEducationValue(EducationLevel);
            
            int livingPlaceSatisfaction = (int)LivingPlace.GetZoneSatisfaction(gm);

            int workPlaceSatisfaction = WorkPlace != null ? (int)WorkPlace.GetZoneSatisfaction(gm) : 0;
            int distanceSatisfaction = WorkPlace != null ? 100 - ((GetDistanceLiveWork(gm, LivingPlace, WorkPlace) * 100) / gm.MaxDistance) : 100;

            Satisfaction = (educationSatisfaction + livingPlaceSatisfaction + workPlaceSatisfaction + distanceSatisfaction) / 4;

            // Ensure satisfaction is within 0-100 range
            //Satisfaction = Math.Clamp(Satisfaction, 0, 100);
        }


        // Method to calculate how much tax should a citizen pay tax based on current factors
        public float TaxAmount(Budget budget)
        {
            // Calculating tax based on current tax rate, and an additional value based on education

            float tax = (float)((budget.CurrentTax * budget.CurrentTaxRate) + (budget.CurrentTaxRate+1)*EducationLevel.GetEducationValue());
            TaxPaidThisYear = tax; // Assuming tax is paid annually
            return tax;

            //taxRate is base in beginning for city and then after time it should be updated in Budges class!!
        }


        public static int GetDistanceLiveWork(GameModel gm, Zone livingPlace, Zone workplace)
        {
            return gm.CalculateDistanceBetweenTwo(livingPlace, workplace);
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

