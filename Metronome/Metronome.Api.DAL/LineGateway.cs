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
    public class LineGateway
    {

        readonly string _connectionString;

        public LineGateway(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<IEnumerable<LineData>> GetAll()
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                return await con.QueryAsync<LineData>(
                    @"select * from metromind.Lines;");
            }
        }

        public async Task<Result<LineData>> FindById(int LineID)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                LineData c = await con.QueryFirstOrDefaultAsync<LineData>(
                    @"select L.api_id,
                            L.[name],
                            L.color,
                            L.opening_time,
                            L.closing_time,
                            L.code,
                        from metromind.Lines L
                        where L.id = @id;",
                    new { id = LineID });

                if (c == null) return Result.Failure<LineData>(Status.NotFound, "Line not found.");
                return Result.Success(Status.Ok, c);
            }
        }
    }
}
