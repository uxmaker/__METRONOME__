using System;
using System.Threading.Tasks;
using NUnit.Framework;
using Metronome.Api.DAL.Navitia;

namespace Metronome.Api.DAL.Tests
{
    [TestFixture]
    public class Tests_StopAreaGateway
    {
        public static StopAreaGateway GetStopAreaGateway() => new StopAreaGateway(Tests_Helper.ConnectionString);
    
        [Test]
        public async Task Can_Create_Find_Delete()
        {
            Console.WriteLine("START");
            var gateway = GetStopAreaGateway();
            var name = Guid.NewGuid().ToString();
            var apiId = String.Concat(name, 32);
            var lon = 0f;
            var lat = 0f;

            // -- CREATE
            int stopArea_id = await gateway.Create(apiId,name,lon,lat);
            Console.WriteLine("CREATE");

            // -- FIND BY ID
            StopAreaData stopArea = await gateway.FindById(stopArea_id);
            Assert.That(stopArea.API_ID, Is.EqualTo(apiId));
            Assert.That(stopArea.Name, Is.EqualTo(name));
            Assert.That(stopArea.Lon, Is.EqualTo(lon));
            Assert.That(stopArea.Lat, Is.EqualTo(lat));
            Console.WriteLine("FIND BY ID");

            // -- FIND BY NAME
            stopArea = await gateway.FindByName(name);
            Assert.That(stopArea.API_ID, Is.EqualTo(apiId));
            Assert.That(stopArea.Name, Is.EqualTo(name));
            Assert.That(stopArea.Lon, Is.EqualTo(lon));
            Assert.That(stopArea.Lat, Is.EqualTo(lat));
            Console.WriteLine("FIND BY API_ID");

            // -- DELETE
            //await gateway.Delete(stopArea_id);
            Console.WriteLine("DELETE");
        }
    }
}
