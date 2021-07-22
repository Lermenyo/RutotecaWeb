namespace RutotecaWeb.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class GpxTrackSegment
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public GpxTrackSegment()
        {
            this.GpxPoint = new HashSet<GpxPoint>();
        }
    
        public int Id { get; set; }
        public int IdTrack { get; set; }
        public Nullable<int> Orden { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<GpxPoint> GpxPoint { get; set; }
        public virtual Tracks Tracks { get; set; }
    }
}
