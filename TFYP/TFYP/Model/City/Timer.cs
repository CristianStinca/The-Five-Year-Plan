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

    /// <summary>
    /// This class manages real-time intervals and calls the Step method on GameModel class. 
    /// It'll also manage different game speeds (three).
    /// </summary>
    public class Timer
    {
        private static Timer _instance;
        public static Timer Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new Timer();
                return _instance;
            }
        }

        private System.Timers.Timer _timer;
        public int GameSpeed { get; set; } = 1;  // Default speed level

        private Timer()
        {
            _timer = new System.Timers.Timer(10000 / GameSpeed);  // 10 seconds adjusted by game speed
            _timer.Elapsed += (sender, e) => GameModel.GetInstance().Step();
            _timer.AutoReset = true;
            _timer.Enabled = true;
        }

        public void ChangeSpeed(int newSpeedLevel)
        {
            GameSpeed = newSpeedLevel;
            _timer.Interval = 10000 / GameSpeed;
        }
    }


}
