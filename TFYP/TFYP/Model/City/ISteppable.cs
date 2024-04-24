using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TFYP.Model.Facilities;
using TFYP.Model.Common;
using TFYP.Model.Zones;

namespace TFYP.Model.City
{
    public interface ISteppable
    {
        /*The ISteppable interface will ensure that any part of the game that needs regular updates 
         * implements a Step method. This method is called by the Timer class.
         */

        void Step();  
    }

}
