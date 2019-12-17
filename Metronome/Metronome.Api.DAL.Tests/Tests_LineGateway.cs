using System;
using System.Threading.Tasks;
using NUnit.Framework;
using Metronome.Api.DAL.Navitia;

namespace Metronome.Api.DAL.Tests
{
    [TestFixture]
    public class Tests_LineGateway
    {
        public static LineGateway GetLineGateway() => new LineGateway(Tests_Helper.ConnectionString);

        [Test]
        public async Task Can_Create_Find_Delete()
        {
            Console.WriteLine("START");
            var gateway = GetLineGateway();
            var opening = DateTime.UtcNow;
            var closing = opening.AddMinutes(1);
            var name = Guid.NewGuid().ToString();
            var apiId = String.Concat(name, 32);
            var color = "00000000";
            var code = "CODE-TEST";

            // -- CREATE
            int line_id = await gateway.Create(apiId, name, color, code, opening, closing);
            Console.WriteLine("CREATE");


            // -- FIND BY ID
            LineData line = await gateway.FindById(line_id);
            Assert.That(line.Id, Is.EqualTo(line_id));
            Assert.That(line.Name, Is.EqualTo(name));
            Assert.That(line.Color, Is.EqualTo(color));
            Assert.That(line.API_ID, Is.EqualTo(apiId));
            Assert.That(line.Code, Is.EqualTo(code));
            // -- Assert.That(line.Opening_time, Is.EqualTo(opening));
            // -- Assert.That(line.Closing_time, Is.EqualTo(closing));
            Console.WriteLine("FIND BY ID");

            // -- FIND BY API_ID
            line = await gateway.FindByApiId(apiId);
            Assert.That(line.Id, Is.EqualTo(line_id));
            Assert.That(line.Name, Is.EqualTo(name));
            Assert.That(line.Color, Is.EqualTo(color));
            Assert.That(line.API_ID, Is.EqualTo(apiId));
            Assert.That(line.Code, Is.EqualTo(code));
            // -- Assert.That(line.Opening_time, Is.EqualTo(opening));
            // -- Assert.That(line.Closing_time, Is.EqualTo(closing));
            Console.WriteLine("FIND BY API_ID");

            // -- DELETE
            await gateway.Delete(line_id);
            Console.WriteLine("DELETE");

        }
    }
}
