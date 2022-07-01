using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;

namespace AquaCulture.Web.Helpers
{
    public class BlazorTimer
    {
        private System.Timers.Timer _timer;

        public void SetTimer(double interval)
        {
            _timer = new System.Timers.Timer(interval);
            _timer.Elapsed += NotifyTimerElapsed;
            _timer.Enabled = true;
        }

        public event Action OnElapsed;

        private void Dispose()
        {
            _timer.Stop();
            _timer.Dispose();
        }
        private void NotifyTimerElapsed(Object source, ElapsedEventArgs e)
        {
            OnElapsed?.Invoke();
            //_timer.Dispose();
        }
    }
}