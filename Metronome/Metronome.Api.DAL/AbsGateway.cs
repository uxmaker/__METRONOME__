using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using Dapper;
using System.Threading.Tasks;

namespace Metronome.Api.DAL
{
    public abstract class AbsGateway
    {
        readonly string _connectionString;

        public AbsGateway(string connectionString)
        {
            _connectionString = connectionString;
        }

        public SqlConnection GetSqlConnection()
        {
            return new SqlConnection(_connectionString);
        }

    }
}
