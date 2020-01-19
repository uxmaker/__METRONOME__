using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Dapper;

namespace Metronome.Api.DAL.Navitia
{
    public class DisruptionGateway : AbsGateway
    {
        //static DisruptionData NullDisruption() => new DisruptionData() { Id = -1, API_ID = null, TextMessage = null, Status = null  };

        public DisruptionGateway(string connectionString) : base(connectionString) { }

        public async Task<int> Create(string statusDisruption, string textMessage, string apiid)
        {
            using (var c = GetSqlConnection())
            {
                var sql_params = new DynamicParameters();
                sql_params.Add("@Status", statusDisruption);
                sql_params.Add("@TextMessage", textMessage);
                sql_params.Add("@API_ID", apiid);
                sql_params.Add("@Id", dbType: DbType.Int32, direction: ParameterDirection.Output);
                sql_params.Add("@Statu", dbType: DbType.Int32, direction: ParameterDirection.ReturnValue);

                await c.ExecuteAsync("MTN.sCreateDisruption", sql_params, commandType: CommandType.StoredProcedure);
                int status = sql_params.Get<int>("@Statu");
                if (status != 0) { return -1; }
                //sawait CreateImpactedLines(sql_params.Get<int>("@Id"),)
                return sql_params.Get<int>("@Id");


            }
        }
        //Obligatoire apres un CreateDisruption
        public async Task<int> CreateImpactedLines(int disruptionID, int lineId)
        {
            using (var c = GetSqlConnection())
            {
                var sql_params = new DynamicParameters();
                sql_params.Add("@DisruptionID", disruptionID);
                sql_params.Add("@LineId", lineId);
                sql_params.Add("@Id", dbType: DbType.Int32, direction: ParameterDirection.Output);
                sql_params.Add("@Status", dbType: DbType.Int32, direction: ParameterDirection.ReturnValue);

                await c.ExecuteAsync("MTN.sCreateImpactedLines", sql_params, commandType: CommandType.StoredProcedure);
                int status = sql_params.Get<int>("@Status");
                if (status != 0) { return -1; }
                return sql_params.Get<int>("@Id");
            }
        }
    }
}
