namespace RutotecaWeb.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class GpxWayPoint
    {
        public int Id { get; set; }
        public Nullable<int> IdElemento { get; set; }
        public Nullable<long> IdGpxPoint { get; set; }
        public Nullable<decimal> Proximity { get; set; }
        public Nullable<decimal> Temperature { get; set; }
        public Nullable<decimal> Depth { get; set; }
        public string DisplayMode { get; set; }
        public string Categories { get; set; }
        public string Address { get; set; }
        public Nullable<int> IdTrack { get; set; }
    
        public virtual GpxPoint GpxPoint { get; set; }
        public virtual Tracks Tracks { get; set; }
    }
}
