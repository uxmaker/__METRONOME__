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
    public class WayGateway
    {
        readonly string _connectionString;

        public WayGateway(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<IEnumerable<WayData>> GetAll()
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                return await con.QueryAsync<WayData>(
                    @"select * from metromind.Ways;");
            }
        }

        public async Task<Result<WayData>> FindById(int WayID)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                WayData c = await con.QueryFirstOrDefaultAsync<WayData>(
                    @"select L.api_id,
                            L.[name],
                            L.color,
                            L.opening_time,
                            L.closing_time,
                            L.code,
                        from metromind.Ways W
                        where W.id = @id;",
                    new { id = WayID });

                if (c == null) return Result.Failure<WayData>(Status.NotFound, "Line not found.");
                return Result.Success(Status.Ok, c);
            }
        }
    }
}
