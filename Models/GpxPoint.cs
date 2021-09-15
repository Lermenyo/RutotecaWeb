namespace RutotecaWeb.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class GpxPoint
    {
        public GpxPoint()
        {

        }
    
        public long Id { get; set; }
        public long IdPunto { get; set; }
        public int IdSegmento { get; set; }
        public int IdCuadricula { get; set; }
        public int IdTrack { get; set; }
        public Nullable<decimal> Lat { get; set; }
        public Nullable<decimal> Lon { get; set; }
        public Nullable<int> Ele { get; set; }
        public Nullable<System.DateTime> Time { get; set; }

        public static string SELECT_COMPLETA =
                "SELECT T3.Id, T3.IdPunto, T3.IdSegmento, T2.IdCuadricula, T3.IdTrack, T2.Ele, T2.Lat, T2.Lon, T3.Time " +
                "FROM Cuadricula T1                             " +
                "INNER JOIN Punto T2 ON T1.Id = T2.IdCuadricula " +
                "INNER JOIN TrackPoint T3 ON T3.IdPunto = T2.Id ";
    }
}
