using Dapper;
using Microsoft.EntityFrameworkCore;
using RutotecaWeb.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace RutotecaWeb.DataContext
{
    public class RutaQueries : DbContext
    {
        string _connectionString;

        public RutaQueries() { }
        public RutaQueries(DbContextOptions<AppContext> options) : base(options) { }

        //public RutaQueries()
        //{
        //    ConfigurationManager.ConnectionStrings.Add(new ConnectionStringSettings("RutotecaBase", "data source=localhost;initial catalog=rutoteca;integrated security=True;MultipleActiveResultSets=True;App=EntityFramework"));
        //    var connectionString = ConfigurationManager.ConnectionStrings["RutotecaBase"].ConnectionString;
        //    _connectionString = "data source=localhost;initial catalog=rutoteca;integrated security=True;MultipleActiveResultSets=True;App=EntityFramework";
        //}

        public RutaDTO GetByPermalink(string permalink)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
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
