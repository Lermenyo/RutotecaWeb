namespace RutotecaWeb.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Tracks
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Tracks()
        {
            this.GpxMetadata = new List<GpxMetadata>();
            this.GpxPoint = new List<GpxPoint>();
            this.GpxTrackSegment = new List<GpxTrackSegment>();
            this.GpxWayPoint = new List<GpxWayPoint>();
        }
    
        public int Id { get; set; }
        public string NombreFichero { get; set; }
        public System.DateTime Fecha { get; set; }
        public string Name { get; set; }
        public string Comment { get; set; }
        public string Description { get; set; }
        public string Source { get; set; }
        public Nullable<int> Number { get; set; }
        public string Type { get; set; }
        public string Link_Href { get; set; }
        public string Link_Text { get; set; }
        public string Link_Uri { get; set; }
        public string Observaciones { get; set; }
    
        public virtual List<GpxMetadata> GpxMetadata { get; set; }
        public virtual List<GpxPoint> GpxPoint { get; set; }
        public virtual List<GpxTrackSegment> GpxTrackSegment { get; set; }
        public virtual List<GpxWayPoint> GpxWayPoint { get; set; }

        public static string SELECT_COMPLETA =
                    "SELECT" +
                    "[Id]," +
                    "[NombreFichero]," +
                    "[Fecha]," +
                    "[Name]," +
                    "[Comment]," +
                    "[Description]," +
                    "[Number]," +
                    "[Type]," +
                    "[Link_Href]," +
                    "[Link_Text]," +
                    "[Link_Uri] ," +
                    "[Observaciones] " +
                    "FROM[rutoteca].[dbo].[Tracks]";
    }
}
