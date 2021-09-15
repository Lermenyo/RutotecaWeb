namespace RutotecaWeb.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Track
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Track()
        {
            this.GpxPoint = new List<GpxPoint>();
        }
    
        public int IdTrack  { get; set; }
    
        public virtual List<GpxPoint> GpxPoint { get; set; }
    }
}
