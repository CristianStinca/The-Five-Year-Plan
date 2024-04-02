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
        public EducationLevel Education { get; private set; }
        public bool IsActive { get; private set; } = true; // citizens are active when created

        public Citizen()
        {
            Satisfaction = 50; // if we assume it is from 1-100
            Education = EducationLevel.Uneducated;
        }
        public int PayTax(int taxRate)
        {
            Satisfaction -= taxRate; // Simplified example of tax rate changing satisfaction

            return taxRate;
        }
        public int GetSatisfaction()
        {
            return Satisfaction;
        }
        public void LeaveCity()
        {
            IsActive = false;
        }

    }
}
