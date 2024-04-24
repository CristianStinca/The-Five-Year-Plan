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
    /// timer setup will call GameModel.GetInstance().Step() every interval
    /// </summary>
    /// 

    public class Timer
    {
        private static Timer _instance;
        private System.Timers.Timer _timer;

        public static Timer Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new Timer();
                return _instance;
            }
        }

        private int _gameSpeed = 1;  // Default speed level

        public int GameSpeed
        {
            get => _gameSpeed;
            set
            {
                if (value != _gameSpeed && value > 0)
                {
                    _gameSpeed = value;
                    _timer.Interval = 10000 / _gameSpeed;
                }
            }
        }

        /// <summary>
        /// 
        /// GameSpeed Setter: The setter now checks if the new speed is different and updates the timer interval 
        /// accordingly. This allows for real-time speed adjustments in your game.
        /// Timer Interval: It is set to adjust according to the game speed.For instance, 
        /// if GameSpeed is set to 2, then 10000 / 2 = 5000, which means the timer will tick every 5 seconds.
        /// 
        /// </summary>
        private Timer()
        {
            _timer = new System.Timers.Timer(10000 / GameSpeed); //will update every 10 seconds, we can change it
            _timer.Elapsed += (sender, e) => GameModel.GetInstance().Step();
            _timer.AutoReset = true;
            _timer.Enabled = true;
        }

        /// <summary>
        /// These two functions will be used for pausing, resuming, saving, or loading the game
        /// </summary>
        public void StartTimer()
        {
            _timer.Start();
        }

        public void StopTimer()
        {
            _timer.Stop();
        }

        //we need to have 3 possible speed options for game, the three speed logic will be implemented here:
        public void ChangeSpeed(int newSpeedLevel)
        {
            GameSpeed = newSpeedLevel;
            _timer.Interval = 10000 / GameSpeed;
        }
    }

    


}
