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
using ProtoBuf;

namespace TFYP.Model.Common
{
    [ProtoContract]
    public enum EducationLevel
    {
        Primary,
        School,
        University
    }
    [ProtoContract]
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

    [ProtoContract]
    [Serializable]
    public class Citizen
    {
        [ProtoMember(1)]
        public string FirstName { get; set; }
        [ProtoMember(2)]
        public string LastName { get; set; }
        [ProtoMember(3)]
        public Zone WorkPlace { get; set; }
        [ProtoMember(4)]
        public Zone LivingPlace { get;   set; }
        [ProtoMember(5)]
        public int Age { get; set; }
        [ProtoMember(6)]
        public bool IsWorking { get; set; }
        [ProtoMember(7)]
        public EducationLevel EducationLevel { get; set; }
        [ProtoMember(8)]
        public float TaxPaidThisYear { get; set; }
        [ProtoMember(9)]
        public int Satisfaction { get; set; }
        [ProtoMember(10)]
        public bool IsActive { get;     set; } = true;
        [ProtoMember(11)]
        public static Random random = new Random();
        [ProtoMember(12)]
        public static RandomName nameGenerator = new RandomName(random);

        public Citizen() { }
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


        /// <summary>
        /// Simulates the citizen leaving the city by setting IsActive to false and removing the citizen from their living and work places.
        /// </summary>

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

        /// <summary>
        /// Calculates the satisfaction level of a citizen based on education, living place, work place, and distance to work.
        /// </summary>
        /// <param name="gm">The GameModel instance containing city information.</param>
        /// <param name="budget">The Budget instance containing financial information.</param>

        public void CalculateSatisfaction(GameModel gm, Budget budget)
        {
            int educationSatisfaction = EducationExtensions.GetEducationValue(EducationLevel);
            
            int livingPlaceSatisfaction = (int)LivingPlace.GetZoneSatisfaction(gm);

            int workPlaceSatisfaction = WorkPlace != null ? (int)WorkPlace.GetZoneSatisfaction(gm) : 0;
            int distanceSatisfaction = WorkPlace != null ? 100 - ((GetDistanceLiveWork(gm, LivingPlace, WorkPlace) * 100) / gm.MaxDistance) : 100;

            Satisfaction = (educationSatisfaction + livingPlaceSatisfaction + workPlaceSatisfaction + distanceSatisfaction) / 4;

            Satisfaction = Math.Clamp(Satisfaction, 0, 100);
        }


        /// <summary>
        /// Calculates the amount of tax a citizen should pay based on the current tax rate and education level.
        /// </summary>
        /// <param name="budget">The Budget instance containing tax rate information.</param>
        /// <returns>The amount of tax to be paid by the citizen.</returns>

        public float TaxAmount(Budget budget)
        {
            // Calculating tax based on current tax rate, and an additional value based on 
            float tax = (float)((budget.CurrentTax * budget.CurrentTaxRate) + (budget.CurrentTaxRate+1)*EducationLevel.GetEducationValue());
            TaxPaidThisYear = tax; // Tax is paid annually
            return tax;

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

