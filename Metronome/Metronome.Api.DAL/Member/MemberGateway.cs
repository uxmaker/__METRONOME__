using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Threading.Tasks;
using Dapper;


namespace Metronome.Api.DAL.Member
{
    public class MemberGateway : AbsGateway
    {

        static MemberData NullMember() => new MemberData() { Email = null, Id = -1, Password = null };

        public MemberGateway(string connectionString)
            :base(connectionString){}
        
        public async Task<MemberData> FindById(int id)
        {
            using (var c = GetSqlConnection())
            {
                try
                {
                    return await c.QueryFirstAsync<MemberData>(
                        "select m.Id, m.Email, m.Password from MTN.Member m where m.Id = @Id",
                        new { Id = id }
                        );
                }
                catch(Exception e)
                {
                    Console.WriteLine(e);
                    return NullMember();
                }
            }
        }

        public async Task<MemberData> FindByEmail(string email)
        {
            using (var c = GetSqlConnection())
            {
                try
                {
                    return await c.QueryFirstAsync<MemberData>(
                        "select m.Id, m.Email, m.Password from MTN.Member m where m.Email = @Email",
                        new { Email = email }
                        );
                }
                catch(Exception e)
                {
                    Console.WriteLine(e);
                    return NullMember();
                }
            }
        }

        public async Task<int> Create(string email, byte[] password)
        {
            using(var c = GetSqlConnection())
            {
                var sql_params = new DynamicParameters();
                sql_params.Add("@Email", email);
                sql_params.Add("@Password", password);
                sql_params.Add("@Id", dbType: DbType.Int32, direction: ParameterDirection.Output);
                sql_params.Add("@Status", dbType: DbType.Int32, direction: ParameterDirection.ReturnValue);

                await c.ExecuteAsync("MTN.sCreateMember", sql_params, commandType: CommandType.StoredProcedure);
                int status = sql_params.Get<int>("@Status");
                if (status != 0) { return -1; }
                return sql_params.Get<int>("@Id");
            }
        }

        public async Task UpdatePassword(int id, byte[] password)
        {
            using(var c = GetSqlConnection())
            {
                var sql_params = new DynamicParameters();
                sql_params.Add("@Id", id);
                sql_params.Add("@Password", password);
                await c.ExecuteAsync("MTN.sUpdateMember", sql_params, commandType: CommandType.StoredProcedure);
            }
        }

        public async Task Delete(int id)
        {
            using(var c = GetSqlConnection())
            {
                await c.ExecuteAsync("MTN.sDeleteMember", new { Id = id }, commandType: CommandType.StoredProcedure);
            }
        }
    }
}
