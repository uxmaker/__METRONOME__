using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Threading.Tasks;
using Dapper;

namespace Metronome.Api.DAL
{
    public class UserGateway
    {
        readonly string _connectionString;

        public UserGateway(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<IEnumerable<UserData>> GetAll()
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                return await con.QueryAsync<UserData>(
                    @"select * from metromind.Members;");
            }
        }

        public async Task<Result<UserData>> FindById(int UserID)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                UserData c = await con.QueryFirstOrDefaultAsync<UserData>(
                    @"select
                            M.email,
                            M.[password],
                        from metromind.Members M
                        where M.id = @id;",
                    new { id = UserID });

                if (c == null) return Result.Failure<UserData>(Status.NotFound, "User not found.");
                return Result.Success(Status.Ok, c);
            }
        }
    }
}
