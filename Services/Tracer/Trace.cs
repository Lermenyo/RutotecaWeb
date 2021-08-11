using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using System;

namespace RutotecaWeb.Services
{
    public enum Nivel:int
    {
        Error = -9000,
        Warning = -1000,
        Info = 0
    }

    public enum SubNivel:int
    {
        Base = 0,
        UriFormatException = -10000,
        DataException = -101,
    }

    public enum SubElemento : int
    {
        Base = 0,
        Ruta = 1,
        Localidad = 2,
        Vertice = 3,
        Tag = 4,
    }
    public enum Terciario : int
    {
        Base = 0

    }
    public enum Accion : int
    {
        Error = -1,
        Base = 0,
        GetByPermalink = 1,
        GetAltimetrias = 2,
        LoadCercanos = 3,
        LugaresCercanos = 4,
        Archivos = 5,
        Imagenes = 6,
        Tracks = 7,
        PuntoElemento = 8,
        RutasCercanas = 9,
        Relacionados = 10,
        RutasEnTag = 11,
        Mapa = 12,
        MapaTracks = 13,
        GPX = 14,
        TracksLugar = 15,
        MeteoblueMapa = 16,
        Meteoblue3h = 17
    }


    public enum SubAccion : int
    {
        Base = 0
    }
    public class Trace:ITrace
    {
        private readonly IConfiguration _config;
        private string _connectionString = "LogEntities";
        public class TrazaException
        {
            private static int MAX_DEEP = 5;
            public string Type;
            public string Source;
            public string StackTrace;
            public string TargetSite;
            public string Data;
            public string Message;
            public TrazaException InnerException;

            public TrazaException(Exception ex, int deep = 0)
            {
                if (ex != null)
                {
                    this.Type = ex.GetType().ToString();
                    this.Source = ex.Source;
                    this.StackTrace = ex.StackTrace;
                    this.TargetSite = ex.TargetSite.ToString();
                    this.Data = JsonConvert.SerializeObject(ex.Data);
                    this.Message = ex.Message;
                    if (ex.InnerException != null && deep < MAX_DEEP)
                        this.InnerException = new TrazaException(ex.InnerException, deep++);
                }
            }
        }
        public Trace(IConfiguration config)
        {
            _config = config;
        }
        public int GetAcceso(HttpRequest request) {
            var ip = request.HttpContext.Connection.RemoteIpAddress;
            var agente = request.Headers["User-Agent"].ToString() ?? String.Empty;
            var accept = request.Headers["Accept"].ToString() ?? String.Empty;
            var encoding = request.Headers["Accept-Encoding"].ToString() ?? String.Empty;
            var languaje = request.Headers["Accept-Language"].ToString() ?? String.Empty;
            var id = SearchAcceso(ip.ToString(),  encoding, languaje, agente, accept);
            if (id < 0)
            {
                InsertAcceso(ip.ToString(),  encoding, languaje, agente, accept);
                id = SearchAcceso(ip.ToString(),  encoding, languaje, agente, accept);
            }
            return id;
        }

        public void SetTraza(string mensaje, int acceso = -1, Nivel nivel = 0, SubNivel subNivel = 0, int elemento = 0, SubElemento  subElemento = 0, Terciario terciario = 0, Accion accion = 0, SubAccion subAccion = 0) {
            InsertTraza(mensaje, acceso, nivel, subNivel, elemento, subElemento, terciario, accion, subAccion);
        }

        private void InsertAcceso(string HTTP_CLIENT_IP,  string HTTP_ACCEPT_ENCODING, string HTTP_ACCEPT_LANGUAGE, string HTTP_USER_AGENT, string HTTP_ACCEPT) {

            using (var connection = new MySql.Data.MySqlClient.MySqlConnection(_config.GetConnectionString(_connectionString)))
            using (var command = connection.CreateCommand())
            {
                command.CommandText = "INSERT INTO identificadoracceso(HTTP_CLIENT_IP,HTTP_ACCEPT_ENCODING,HTTP_ACCEPT_LANGUAGE,HTTP_USER_AGENT,HTTP_ACCEPT)" +
                    "VALUES(@HTTP_CLIENT_IP,  @HTTP_ACCEPT_ENCODING, @HTTP_ACCEPT_LANGUAGE, @HTTP_USER_AGENT, @HTTP_ACCEPT); ";

                command.Parameters.AddWithValue("@HTTP_CLIENT_IP" , LimitSize(HTTP_CLIENT_IP, 50));
                command.Parameters.AddWithValue("@HTTP_ACCEPT_ENCODING" , LimitSize(HTTP_ACCEPT_ENCODING, 150));
                command.Parameters.AddWithValue("@HTTP_ACCEPT_LANGUAGE" , LimitSize(HTTP_ACCEPT_LANGUAGE, 50));
                command.Parameters.AddWithValue("@HTTP_USER_AGENT" , LimitSize(HTTP_USER_AGENT,200));
                command.Parameters.AddWithValue("@HTTP_ACCEPT" , LimitSize(HTTP_ACCEPT, 200));

                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();
            }
        }

        private void InsertTraza(string mensaje, int acceso = -1, Nivel nivel = 0, SubNivel subNivel = 0, int elemento = 0, SubElemento subElemento = 0, Terciario terciario = 0, Accion accion = 0, SubAccion subAccion = 0)
        {
            using (var connection = new MySql.Data.MySqlClient.MySqlConnection(_config.GetConnectionString(_connectionString)))
            using (var command = connection.CreateCommand())
            {
                command.CommandText = "INSERT INTO log (IdNivel,IdSubNivel,IdentificadorAcceso,IdElemento,IdSubElemento,IdTerciario,IdAccion,IdSubAccion,Texto) " +
                    " VALUES ( @IdNivel,@IdSubNivel,@IdentificadorAcceso,@IdElemento,@IdSubElemento,@IdTerciario,@IdAccion,@IdSubAccion,@Texto);";

                command.Parameters.Add("@IdNivel", MySqlDbType.Int16);
                command.Parameters.Add("@IdSubNivel", MySqlDbType.Int16);
                command.Parameters.Add("@IdentificadorAcceso", MySqlDbType.Int16);
                command.Parameters.Add("@IdElemento", MySqlDbType.Int16);
                command.Parameters.Add("@IdSubElemento", MySqlDbType.Int16);
                command.Parameters.Add("@IdTerciario", MySqlDbType.Int16);
                command.Parameters.Add("@IdAccion", MySqlDbType.Int16);
                command.Parameters.Add("@IdSubAccion", MySqlDbType.Int16);
                command.Parameters.Add("@Texto", MySqlDbType.String);

                command.Parameters["@IdNivel"].Value = (int) nivel;
                command.Parameters["@IdSubNivel"].Value = (int)subNivel;
                command.Parameters["@IdentificadorAcceso"].Value = acceso;
                command.Parameters["@IdElemento"].Value = elemento;
                command.Parameters["@IdSubElemento"].Value = (int)subElemento;
                command.Parameters["@IdTerciario"].Value = (int)terciario;
                command.Parameters["@IdAccion"].Value = (int)accion;
                command.Parameters["@IdSubAccion"].Value = (int)subAccion;
                command.Parameters["@Texto"].Value = LimitSize(mensaje, 5000);

                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();
            }
        }

        public void InsertEntrada(int? acceso, string origen, string destino)
        {
            using (var connection = new MySql.Data.MySqlClient.MySqlConnection(_config.GetConnectionString(_connectionString)))
            using (var command = connection.CreateCommand())
            {
                command.CommandText = "INSERT INTO movimientos (idAcceso, procedencia, destino) " +
                    " VALUES ( @idAcceso,@procedencia,@destino);";

                command.Parameters.Add("@idAcceso", MySqlDbType.Int16);
                command.Parameters.Add("@procedencia", MySqlDbType.String);
                command.Parameters.Add("@destino", MySqlDbType.String);

                command.Parameters["@idAcceso"].Value = acceso??-1;
                command.Parameters["@procedencia"].Value = origen;
                command.Parameters["@destino"].Value = destino;

                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();
            }
        }

        public void InsertException(Exception ex, int acceso = -1, int elemento = 0, SubElemento subElemento = 0, Terciario terciario = 0, Accion accion = 0, SubAccion subAccion = 0)
        {
            SubNivel subNivel = SubNivel.Base;
            TrazaException traza = new TrazaException(ex);

            if (ex is UriFormatException)
                subNivel = SubNivel.UriFormatException;
            if (ex is System.Data.DataException)
                subNivel = SubNivel.DataException;

            using (var connection = new MySql.Data.MySqlClient.MySqlConnection(_config.GetConnectionString(_connectionString)))
            using (var command = connection.CreateCommand())
            {
                command.CommandText = "INSERT INTO log (IdNivel,IdSubNivel,IdentificadorAcceso,IdElemento,IdSubElemento,IdTerciario,IdAccion,IdSubAccion,Texto) " +
                    " VALUES ( @IdNivel,@IdSubNivel,@IdentificadorAcceso,@IdElemento,@IdSubElemento,@IdTerciario,@IdAccion,@IdSubAccion,@Texto);";

                command.Parameters.Add("@IdNivel", MySqlDbType.Int16);
                command.Parameters.Add("@IdSubNivel", MySqlDbType.Int16);
                command.Parameters.Add("@IdentificadorAcceso", MySqlDbType.Int16);
                command.Parameters.Add("@IdElemento", MySqlDbType.Int16);
                command.Parameters.Add("@IdSubElemento", MySqlDbType.Int16);
                command.Parameters.Add("@IdTerciario", MySqlDbType.Int16);
                command.Parameters.Add("@IdAccion", MySqlDbType.Int16);
                command.Parameters.Add("@IdSubAccion", MySqlDbType.Int16);
                command.Parameters.Add("@Texto", MySqlDbType.String);

                command.Parameters["@IdNivel"].Value = (int)Nivel.Error;
                command.Parameters["@IdSubNivel"].Value = (int)subNivel;
                command.Parameters["@IdentificadorAcceso"].Value = acceso;
                command.Parameters["@IdElemento"].Value = elemento;
                command.Parameters["@IdSubElemento"].Value = (int)subElemento;
                command.Parameters["@IdTerciario"].Value = (int)terciario;
                command.Parameters["@IdAccion"].Value = (int)accion;
                command.Parameters["@IdSubAccion"].Value = (int)subAccion;
                command.Parameters["@Texto"].Value = LimitSize(JsonConvert.SerializeObject(traza), 5000);

                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();
            }
        }
        private int SearchAcceso(string HTTP_CLIENT_IP, string HTTP_ACCEPT_ENCODING, string HTTP_ACCEPT_LANGUAGE, string HTTP_USER_AGENT, string HTTP_ACCEPT)
        {
            var id = -1;
            using (var connection = new MySql.Data.MySqlClient.MySqlConnection(_config.GetConnectionString(_connectionString)))
            using (var command = connection.CreateCommand())
            {
                command.CommandText = "SELECT MIN(idIdentificadorAcceso) as id from identificadoracceso" +
                    " WHERE HTTP_CLIENT_IP = @HTTP_CLIENT_IP" +
                    " AND HTTP_ACCEPT_ENCODING = @HTTP_ACCEPT_ENCODING" +
                    " AND HTTP_ACCEPT_LANGUAGE = @HTTP_ACCEPT_LANGUAGE" +
                    " AND HTTP_USER_AGENT = @HTTP_USER_AGENT";
                    if (HTTP_ACCEPT != "*/*")
                        command.CommandText +=  " AND HTTP_ACCEPT = @HTTP_ACCEPT";

                command.Parameters.AddWithValue("@HTTP_CLIENT_IP", LimitSize(HTTP_CLIENT_IP, 50));
                command.Parameters.AddWithValue("@HTTP_ACCEPT_ENCODING", LimitSize(HTTP_ACCEPT_ENCODING, 150));
                command.Parameters.AddWithValue("@HTTP_ACCEPT_LANGUAGE", LimitSize(HTTP_ACCEPT_LANGUAGE, 50));
                command.Parameters.AddWithValue("@HTTP_USER_AGENT", LimitSize(HTTP_USER_AGENT, 200));
                if (HTTP_ACCEPT != "*/*")
                    command.Parameters.AddWithValue("@HTTP_ACCEPT", LimitSize(HTTP_ACCEPT, 200));

                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        if (reader["id"] != DBNull.Value)
                            id = ((int?)reader["id"])??-1;
                    }
                }
                connection.Close();
                return id;
            }
        }

        private string LimitSize(string strIn, int maxLength = 150)
        {
            if (strIn.Length > maxLength)
                strIn = strIn.Substring(0, maxLength);
            return strIn;
        }
    }

}