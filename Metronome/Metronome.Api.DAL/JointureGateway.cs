using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Dapper;

namespace Metronome.Api.DAL
{
    public class JointureGateway : AbsGateway
    {
        public JointureGateway(string connectionString) : base(connectionString) { }


        public async Task<int> Create(string code, string name1, string name2)
        {
            using (var c = GetSqlConnection())
            {
                var sql_params = new DynamicParameters();
                sql_params.Add("@LigneCode", code);
                sql_params.Add("@StopAreaName1", name1);
                sql_params.Add("@StopAreaName2", name2);
                sql_params.Add("@Id", dbType: DbType.Int32, direction: ParameterDirection.Output);
                sql_params.Add("@Status", dbType: DbType.Int32, direction: ParameterDirection.ReturnValue);
                await c.ExecuteAsync("MTN.sCreateJointure", sql_params, commandType: CommandType.StoredProcedure);
                int status = sql_params.Get<int>("@Status");
                if (status != 0) { return -1; }
                return sql_params.Get<int>("@Id");
            }
        }


    }
}
