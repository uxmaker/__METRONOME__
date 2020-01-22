using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Dapper;

namespace Metronome.Api.DAL.Navitia
{
    public class HorrairesGateway : AbsGateway
    {

        public HorrairesGateway(string connectionString) : base(connectionString) { }

        public async Task<int> Create(string ArrivalTime, string StopName, int LigneId, string Direction)
        {
            using (var c = GetSqlConnection())
            {
                var sql_params = new DynamicParameters();
                sql_params.Add("@Arrival_time", ArrivalTime);
                sql_params.Add("@StopName", StopName);
                sql_params.Add("@LineId", LigneId);
                sql_params.Add("@Direction", Direction);
                sql_params.Add("@Id", dbType: DbType.Int32, direction: ParameterDirection.Output);
                sql_params.Add("@Status", dbType: DbType.Int32, direction: ParameterDirection.ReturnValue);

                await c.ExecuteAsync("MTN.sCreateHorraires", sql_params, commandType: CommandType.StoredProcedure);
                int status = sql_params.Get<int>("@Status");
                if (status != 0) { return -1; }
                return sql_params.Get<int>("@Id");


            }
        }
        public async Task<IEnumerable<HorrairesData>> GetHorraireByStopArea(int stoparea, int ligne)
        {
            using (var c = GetSqlConnection())
            {
                return await c.QueryAsync<HorrairesData>("select * from MTN.Horraires h where h.LineId = @Ligne and h.StopAreaId = @StopArea",
                    new { Ligne = ligne, StopArea = stoparea });
            }
        }

        public async Task<IEnumerable<HorrairesData>> FindByStopArea(string StopArea)
        {
            using (var c = GetSqlConnection())
            {

                return await c.QueryAsync<HorrairesData>("select * from MTN.Horraires h where h.StopAreaId = (select id from MTN.StopArea s where s.Name =@StopAreaName  ", new { StopAreaName = StopArea });
            }
        }
        public async Task<int> DeleteByLineId(int lineId)
        {

            using (var c = GetSqlConnection())
            {
                try
                {
                    await c.QueryAsync("delete MTN.Horraires where LineId = @LineId", new { LineId = lineId });
                    return 0;

                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    return 1;
                }
            }
        }

    }
}
