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
    public class TransportGateway
    {
        readonly string _connectionString;

        public TransportGateway(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<IEnumerable<TransportGateway>> GetAll()
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                return await con.QueryAsync<TransportGateway>(
                    @"select * from metromind.Transports;");
            }
        }

        public async Task<Result<TransportGateway>> FindById(int TransportID)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                TransportGateway c = await con.QueryFirstOrDefaultAsync<TransportGateway>(
                    @"select
                            T.[name],
                            T.X,
                            T.Y,
                            T.lineID,
                        from metromind.Transports T
                        where T.id = @id;",
                    new { id = TransportID });

                if (c == null) return Result.Failure<TransportGateway>(Status.NotFound, "Transport not found.");
                return Result.Success(Status.Ok, c);
            }
        }
    }
}
