using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Metronome.Api.Daemon
{

    public enum DaemonStatus
    {
        None,
        NotReady,
        Ready
    }

    internal class DaemonManager
    {
        readonly object _lock;
        List<(Daemon, Task, DaemonStatus)> _daemons; 


        public DaemonManager()
        {
            _lock = new object();
            _daemons = new List<(Daemon, Task, DaemonStatus)>();
        }
        
        public void Run()
        {
            var daemons_initialized = new List<(Daemon, Task, DaemonStatus)>();
            var daemons_delayed = new List<(Daemon, Task, DaemonStatus)>();

            while (true)
            {
                lock(_lock)
                {
                    foreach (var daemon_nr in _daemons)
                    {
                        if (daemon_nr.Item3 == DaemonStatus.NotReady)
                        {
                            if (daemon_nr.Item2.IsCompleted) daemons_initialized.Add(daemon_nr);
                        }
                    }

                    foreach(var daemon_i in daemons_initialized)
                    {
                        _daemons.Remove(daemon_i);
                        _daemons.Add((daemon_i.Item1, daemon_i.Item1.Run(), DaemonStatus.Ready));
                    }

                    daemons_initialized.Clear();

                    foreach(var daemon_r in _daemons)
                    {
                        if(daemon_r.Item2.IsCompleted)
                        {
                            if(daemon_r.Item1.Delayed)
                            {
                                daemons_delayed.Add(daemon_r);
                            }
                        }
                    }

                    foreach(var daemon_d in daemons_delayed)
                    {
                        _daemons.Remove(daemon_d);
                        daemon_d.Item1.Delayed = false;
                        _daemons.Add((daemon_d.Item1, daemon_d.Item1.Run(), daemon_d.Item3));
                    }

                    daemons_delayed.Clear();
                }
            }
        }

        public void Add(Daemon daemon)
        {
            lock(_lock) { _daemons.Add((daemon, daemon.Init(), DaemonStatus.NotReady)); }
        }

        public void Remove(string guid)
        {
            (Daemon, Task, DaemonStatus) daemon_result = (null, null, DaemonStatus.None);

            lock (_lock)
            {
                foreach(var daemon in _daemons){ if (daemon.Item1.Guid == guid) daemon_result = daemon; }
                if(daemon_result.Item1 != null) _daemons.Remove(daemon_result);
            }
        }

    }
}
