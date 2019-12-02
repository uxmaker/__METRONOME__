using System;
using System.Threading.Tasks;
using Metronome.Api.Daemon.Lib;

namespace Metronome.Api.Daemon
{
    class Program
    {
        static void Main(string[] args)
        {
            DaemonManager manager = new DaemonManager();
            manager.Add(new Template_Daemon(5000));
            manager.Run();

        }
    }
}
