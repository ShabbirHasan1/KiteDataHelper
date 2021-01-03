using System;
using System.Collections.Generic;
using System.Text;
using System.Timers;

namespace KiteDataHelper.Models
{
    public class FifteenMinuteTimer
    {
        private readonly Timer _timer;

        public FifteenMinuteTimer()
        {
            _timer = new Timer(900001);
            _timer.Enabled = false;
            _timer.AutoReset = true;
        }

        public Timer FifteenMinTimer 
        {
            get
            {
                return _timer;
            }

            private set
            {
            }
        }
    }
}
