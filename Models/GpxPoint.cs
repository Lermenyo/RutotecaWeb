namespace RutotecaWeb.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class GpxPoint
    {
        public GpxPoint()
        {
            this.GpxWayPoint = new HashSet<GpxWayPoint>();
        }
    
        public long Id { get; set; }
        public Nullable<decimal> lat { get; set; }
        public Nullable<decimal> lon { get; set; }
        public Nullable<decimal> ele { get; set; }
        public Nullable<System.DateTime> time { get; set; }
        public Nullable<int> IdTrack { get; set; }
        public Nullable<int> Orden { get; set; }
        public string Name { get; set; }
        public string Comment { get; set; }
        public string Description { get; set; }
        public string Source { get; set; }
        public string Symbol { get; set; }
        public Nullable<int> IdSegmento { get; set; }
    
        public virtual GpxTrackSegment GpxTrackSegment { get; set; }
        public virtual ICollection<GpxWayPoint> GpxWayPoint { get; set; }
        public virtual Tracks Tracks { get; set; }

        public static string SELECT_COMPLETA =
                "SELECT [Id],   " +
                "[lat]      ," +
                "[lon]      ," +
                "[ele]      ," +
                "[time]      ," +
                "[IdTrack]     ," +
                "[Orden]      ," +
                "[Name]      ," +
                "[Comment]      ," +
                "[Description]      ," +
                "[Source]      ," +
                "[Symbol]      ," +
                "[IdSegmento]   " +
                "FROM [rutoteca].[dbo].[GpxPoint]";
    }
}
