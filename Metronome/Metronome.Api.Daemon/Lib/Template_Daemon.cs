using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Metronome.Api.Daemon.Lib
{
    public class Template_Daemon : Daemon
    {
        public Template_Daemon(int frequency)
            :base(frequency)
        {}

        public async override Task Init()
        {
            Console.WriteLine("... INITIALIZE ...");
            await Task.Delay(100);
        }

        public async override Task Run()
        {
            Console.WriteLine("... RUN ...");
            await Task.Delay(100);
        }

    }
}
