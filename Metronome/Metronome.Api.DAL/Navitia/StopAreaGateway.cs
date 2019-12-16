using System;
using System.Threading.Tasks;
using System.Data;
using Dapper;
using System.Collections.Generic;

namespace Metronome.Api.DAL.Navitia
{
    public class StopAreaGateway : AbsGateway
    {
        static StopAreaData NullStopArea() => new StopAreaData(){ Id = -1, API_ID = null, Coord = null, Lat = 0f, Lon = 0f, Name = null };

        public StopAreaGateway(string connectionString) : base(connectionString) { }

        public async Task<List<StopAreaData>> GetAll()
        {
            using (var c = GetSqlConnection())
            {
                try
                {
                    var r =  await c.QueryAsync<StopAreaData>(
                            @"select    m.Id,
                                        m.API_ID,
                                        m.Name,
                                        m.Lon,
                                        m.Lat
                                        from MTN.StopArea m" );

                    return r.AsList<StopAreaData>();
                }
                catch(Exception e)
                {
                    Console.WriteLine(e);
                }

                return new List<StopAreaData>();
            }
        }

        public async Task<StopAreaData> FindById(int id)
        {
            using (var c = GetSqlConnection())
            {
                try
                {
                    return await c.QueryFirstAsync<StopAreaData>(
                            @"select    m.Id,
                                        m.API_ID,
                                        m.Name,
                                        m.Lon,
                                        m.Lat
                                        from MTN.StopArea m
                                        where m.Id = @Id",
                        new { Id = id }
                    );
                }
                catch(Exception e)
                {
                    Console.WriteLine(e);
                    return NullStopArea();
                }
            }
        }

        public async Task<StopAreaData> FindByName(string name)
        {
            using (var c = GetSqlConnection())
            {
                try
                {
                    return await c.QueryFirstAsync<StopAreaData>(
                            "select m.Id, m.API_ID, m.Name, m.Lon, m.Lat from MTN.StopArea m where m.Name = @Name",
                            new { Name = name }
                        );
                }
                catch(Exception e)
                {
                    Console.WriteLine(e);
                    return NullStopArea();
                }
            }
        }

        public async Task<int> Create(string apiId, string name, float lon, float lat)
        {
            using (var c = GetSqlConnection())
            {
                var sql_params = new DynamicParameters();
                sql_params.Add("@API_ID", apiId);
                sql_params.Add("@Name", name);
                sql_params.Add("@Lon", lon);
                sql_params.Add("@Lat", lat);
                sql_params.Add("@Id", dbType: DbType.Int32, direction: ParameterDirection.Output);
                sql_params.Add("@Status", dbType: DbType.Int32, direction: ParameterDirection.ReturnValue);

                await c.ExecuteAsync("MTN.sCreateStopArea", sql_params, commandType: CommandType.StoredProcedure);
                int status = sql_params.Get<int>("@Status");
                if (status != 0) { return -1; }
                return sql_params.Get<int>("@Id");
            }
        }

        public async Task Delete(int id)
        {
            using (var c = GetSqlConnection())
            {
                await c.ExecuteAsync("MTN.sDeleteStopArea", new { Id = id }, commandType: CommandType.StoredProcedure);
            }
        }

    }
}
