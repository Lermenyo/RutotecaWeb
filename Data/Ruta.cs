using Dapper;
using RutotecaWeb.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace RutotecaWeb.Data
{
    public class SqlServerRutaQueries 
    {
        public RutaDTO GetRutasByPermalink(string permalink)
        {
            var connectionString = "data source=localhost;initial catalog=rutoteca;integrated security=True;MultipleActiveResultSets=True;App=EntityFramework";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                var ruta = connection.QueryFirst<RutaDTO>("select * FROM [rutoteca].[dbo].vwRutas where [Permalink]= '" + permalink + "'");
                if (ruta == null)
                {
                    return null;
                }
                return ruta;
            }
        }
    }
}
