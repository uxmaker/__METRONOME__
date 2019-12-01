using System;
using System.Threading.Tasks;
using NUnit.Framework;
using Metronome.Api.DAL.Member;

namespace Metronome.Api.DAL.Tests
{
    [TestFixture]
    public class Tests_MemberGateway
    {
        public static MemberGateway GetMemberGateway() => new MemberGateway(Tests_Helper.ConnectionString);
        public static string GetRandomEmail() => string.Format("user_{0}@test.com", Guid.NewGuid().ToString().Substring(8));
        public static byte[] GetRandomPassword() => Guid.NewGuid().ToByteArray();

        [Test]
        public async Task Can_Create_Find_Match_Delete()
        {
            var gateway = GetMemberGateway();
            string member_email = GetRandomEmail();
            byte[] member_password = GetRandomPassword();
            
            // -- CREATE
            int member_id = await gateway.Create(member_email, member_password);

            // -- FIND BY ID
            MemberData member = await gateway.FindById(member_id);
            Assert.That(member.Id, Is.EqualTo(member_id));
            Assert.That(member.Email, Is.EqualTo(member_email));
            Assert.That(member.Password, Is.EqualTo(member_password));

            // -- FIND BY EMAIL
            member = await gateway.FindByEmail(member_email);
            Assert.That(member.Id, Is.EqualTo(member_id));
            Assert.That(member.Email, Is.EqualTo(member_email));
            Assert.That(member.Password, Is.EqualTo(member_password));

            // -- MATCH
            var member_matched = await gateway.Match(member_email, member_password);
            Console.WriteLine(member_matched.Id);
            Console.WriteLine(member_id);
            Assert.That(member_matched.Id, Is.EqualTo(member_id));

            // -- DELETE
            await gateway.Delete(member_id);
            MemberData empty_member = await gateway.FindById(member_id);
            Assert.That(empty_member.Id, Is.EqualTo(-1));
        }

        [Test]
        public async Task Can_Update()
        {
            var gateway = GetMemberGateway();
            string member_email = GetRandomEmail();
            byte[] member_password = GetRandomPassword();

            // -- CREATE
            int member_id = await gateway.Create(member_email, member_password);

            // -- UPDATE PASSWORD
            member_password = GetRandomPassword();
            await gateway.UpdatePassword(member_id, member_password);
            var member = await gateway.FindById(member_id);
            Assert.That(member.Password, Is.EqualTo(member_password));

            // -- DELETE
            await gateway.Delete(member_id);
        }
    }
}