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
    public class StationGateway
    {
        readonly string _connectionString;

        public StationGateway(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<IEnumerable<StationData>> GetAll()
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                return await con.QueryAsync<StationData>(
                    @"select * from metromind.Stations;");
            }
        }

        public async Task<Result<StationData>> FindById(int StationID)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                StationData c = await con.QueryFirstOrDefaultAsync<StationData>(
                    @"select
                            S.api_id,
                            S.[name],
                            S.X,
                            S.Y,
                        from metromind.Stations S
                        where S.id = @id;",
                    new { id = StationID });

                if (c == null) return Result.Failure<StationData>(Status.NotFound, "Station not found.");
                return Result.Success(Status.Ok, c);
            }
        }
    }
}
