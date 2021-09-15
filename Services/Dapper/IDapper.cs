using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;

namespace RutotecaWeb.Services
{
    public interface IDapper : IDisposable
    {
        DbConnection GetDbconnection(string conn);
        T Get<T>(string conn, string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure);
        List<T> GetAll<T>(string conn, string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure);
        int Execute(string conn, string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure);
        T Insert<T>(string conn, string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure);
        T Update<T>(string conn, string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure);
    }
}
