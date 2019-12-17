using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Dapper;

namespace Metronome.Api.DAL.Navitia
{
    public class LineGateway : AbsGateway
    {
        static LineData NullLine() => new LineData() { Id = -1, API_ID = null, Color = null, Code = null, Name = null, Opening_time = null, Closing_time = null };

        public LineGateway(string connectionString) : base(connectionString) { }

        public async Task<IEnumerable<LineData>> GetAll()
        {
            using (var c = GetSqlConnection())
            {
                return await c.QueryAsync<LineData>(@"select Id, [name], ApiID, HEX from MTN.Line");
            }
        }

        public async Task<LineData> FindById(int id)
        {
            using (var c = GetSqlConnection())
            {
                try
                {
                    return await c.QueryFirstAsync<LineData>(
                            @"select    m.Id,
                                        m.API_ID,
                                        m.Name,
                                        m.Code,
                                        m.Color,
                                        m.Opening_time,
                                        m.Closing_time
                                        from MTN.Line m
                                        where m.Id = @Id",
                            new { Id = id }
                        );
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    return NullLine();
                }
            }
        }

        public async Task<LineData> FindByApiId(string apiId)
        {
            using (var c = GetSqlConnection())
            {
                try
                {
                    return await c.QueryFirstAsync<LineData>(
                            "select m.Id, m.API_ID, m.Name, m.Code, m.Color, m.Opening_time, m.Closing_time from MTN.Line m where m.API_ID = @API_ID",
                            new { API_ID = apiId }
                        );
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    return NullLine();
                }
            }
        }

        public async Task<int> Create(string apiId, string name, string color, string code, DateTime opening, DateTime closing)
        {
            using (var c = GetSqlConnection())
            {
                var sql_params = new DynamicParameters();
                sql_params.Add("@API_ID", apiId);
                sql_params.Add("@Name", name);
                sql_params.Add("@Color", color);
                sql_params.Add("@Code", code);
                sql_params.Add("@Opening_time", opening);
                sql_params.Add("@Closing_time", closing);
                sql_params.Add("@Id", dbType: DbType.Int32, direction: ParameterDirection.Output);
                sql_params.Add("@Status", dbType: DbType.Int32, direction: ParameterDirection.ReturnValue);

                await c.ExecuteAsync("MTN.sCreateLine", sql_params, commandType: CommandType.StoredProcedure);
                int status = sql_params.Get<int>("@Status");
                if(status != 0) { return -1; }
                return sql_params.Get<int>("@Id");
            }
        }

        public async Task Delete(int id)
        {
            using (var c = GetSqlConnection())
            {
                await c.ExecuteAsync("MTN.sDeleteLine", new { Id = id }, commandType: CommandType.StoredProcedure);
            }
        }

    }
}
