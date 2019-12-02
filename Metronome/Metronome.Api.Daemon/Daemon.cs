using System;
using System.Threading.Tasks;
using System.Timers;

namespace Metronome.Api.Daemon
{

    public abstract class Daemon
    {
        public string Guid { get; }

        readonly Timer _timer;

        public bool Delayed { get; set; }

        // -- PUBLIC METHODS

        public Daemon(int interval)
        {
            Guid = System.Guid.NewGuid().ToString();
            _timer = new Timer();
            Delayed = false;
            ConfigureTimer(interval);
            _timer.Start();
        }

        // -- VIRTUAL METHODS

        public virtual async Task Init()
        {
            throw new NotImplementedException();
        }

        public virtual async Task Run()
        {
            throw new NotImplementedException();
        }

        // -- PRIVATE METHODS

        private void IsDelayed(object sender, ElapsedEventArgs args)
        {
            if (!Delayed) Delayed = true;
        }

        private void ConfigureTimer(int interval)
        {
            _timer.Interval = interval;
            _timer.AutoReset = true;
            _timer.Elapsed += IsDelayed;
            _timer.Enabled = true;
        }

    }
}
