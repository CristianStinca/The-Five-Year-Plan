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
        Uneducated,
        School,
        University
    }
    internal class Citizen
    {
        public int Satisfaction { get; private set; }
        public int Age { get; private set; }
        public bool IsActive { get; private set; } = true; // citizens are active when created
        public bool IsWorking { get; private set; }
        public EducationLevel EducationLevel { get; private set; }
        public Zone Living { get; private set; }
        public Zone Working { get; private set; }

        public Citizen(Zone living, Zone working)
        {
            Satisfaction = 50; // if we assume it is from 1-100
            EducationLevel = EducationLevel.Uneducated;
            Living = living;
            Working = working;
        }

        public void SetSatisfaction(GameModel gm)
        {
            
        }
        public int PayTax(int taxRate)
        {
            return taxRate;
        }

        public void SetWorking(GameModel gm, Zone workPlace)
        {
            Working = workPlace;
            IsWorking = true;
            //workPlace.AddCitizen(this, gm);
        }
        public void Retire()
        {
            IsWorking = false;
        }
        public void IncAge()
        {
            Age++;
        }
        public int GetSatisfaction()
        {
            return Satisfaction;
        }
        public void LeaveCity()
        {
            IsActive = false;
        }

        public override string ToString()
        {
            return "Citizen{" +
                    "  levelOfEducation=" + EducationLevel +
                    ", livingPlace=" + Living +
                    ", workplace=" + Working +
                    ", age=" + Age +
                    ", isWorking=" + IsWorking +
                    '}';
        }

    }
}
