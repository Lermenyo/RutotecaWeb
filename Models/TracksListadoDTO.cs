namespace RutotecaWeb.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class TracksListadoDTO
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public TracksListadoDTO()
        {
        }
    
        public int Id               { get; set; }
        public Decimal Distancia    { get; set; }
        public int IBP              { get; set; }
        public Decimal Ascenso      { get; set; }
        public Decimal Descenso     { get; set; }
        public Decimal KmSubida     { get; set; }
        public TimeSpan Tiempo      { get; set; }
        public String Color         { get; set; }

        public static string SELECT_COMPLETA =
            "SELECT T1.[Id]     AS ID,	        " +
            "T3.totlengthkm     AS Distancia,	" +
            "T3.IBPCam	        AS IBP,         " +
            "T3.accuclimb       AS ASCENSO,	    " +
            "T3.accudescent     AS DESCENSO,	" +
            "T3.totclimbdistkm  AS KMSUBIDA,	" +
            "T3.[totaltime]     AS TIEMPO    " +
            "FROM[rutoteca].[dbo].[Tracks] T1 " +
            "INNER JOIN[dbo].[TrackRuta] T2 ON T1.[Id] = IDTRACK " +
            "LEFT JOIN IBPINDEX T3 ON T3.[idTrack] = T1.ID ";
        //WHERE T2.IDRUTA = 8203 
    }
}
