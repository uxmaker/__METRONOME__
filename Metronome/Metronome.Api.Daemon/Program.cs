using System;
using System.Threading.Tasks;
using Metronome.Api.Daemon.Lib;
using Metronome.Api.DAL;
using Metronome.Api.DAL.Navitia;

namespace Metronome.Api.Daemon
{
    class Program
    {
        static void Main(string[] args)
        {
            DaemonManager manager = new DaemonManager();

            DaemonOptions stationOpt = new DaemonOptions()
            {
                ConnectionString = "Server=DESKTOP-ER1MHPU\\SQLEXPRESS;Database=METRONOME;Trusted_Connection=True",
                API_BaseURL = "https://api.navitia.io/v1/coverage/fr-idf/",
                API_TokenAuth = "b0091e2f-0480-4216-8f99-ebafb067c539"
                // <OR> "48f07b3d-b3e5-4c89-b7ac-768148e8da72"
            };

            manager.Add(new Navitia_Daemon(stationOpt, new LineGateway(stationOpt.ConnectionString), new StopAreaGateway(stationOpt.ConnectionString), new JointureGateway(stationOpt.ConnectionString)));

            manager.Add(new Template_Daemon(5000));
            
            manager.Run();
        }
    }
}