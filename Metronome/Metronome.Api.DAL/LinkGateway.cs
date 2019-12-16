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
    public class LinkGateway
    {
        readonly string _connectionString;

        public LinkGateway(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<IEnumerable<LinkData>> GetAll()
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                return await con.QueryAsync<LinkData>(@"select * from metromind.Links");
            }
        }

        public async Task<Result<LinkData>> FindById(int LinkID)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                LinkData c = await con.QueryFirstOrDefaultAsync<LinkData>(
                    @"select
                            L.stationAID,
                            L.stationBID,
                            L.Tmoy,
                            L.[state],
                        from metromind.Links L
                        where L.id = @id;",
                    new { id = LinkID });

                if (c == null) return Result.Failure<LinkData>(Status.NotFound, "Link not found.");
                return Result.Success(Status.Ok, c);
            }
        }
    }
}
