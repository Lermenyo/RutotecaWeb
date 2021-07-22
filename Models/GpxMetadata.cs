namespace RutotecaWeb.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class GpxMetadata
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Author_Name { get; set; }
        public string Author_Email { get; set; }
        public string Author_Link { get; set; }
        public string Copyright_Author { get; set; }
        public Nullable<int> Copyright_Year { get; set; }
        public string Copyright_Licence { get; set; }
        public string Link_Href { get; set; }
        public string Link_Text { get; set; }
        public string MimeType { get; set; }
        public string Uri { get; set; }
        public Nullable<int> IdTrack { get; set; }
        public Nullable<decimal> minlat { get; set; }
        public Nullable<decimal> minlon { get; set; }
        public Nullable<decimal> maxlat { get; set; }
        public Nullable<decimal> maxlon { get; set; }
    
        public virtual Tracks Tracks { get; set; }


        public static string SELECT_COMPLETA =
                "SELECT [Id]                       " +
                "      ,[Name]                     " +
                "      ,[Description]              " +
                "      ,[Author_Name]              " +
                "      ,[Author_Email]             " +
                "      ,[Author_Link]              " +
                "      ,[Copyright_Author]         " +
                "      ,[Copyright_Year]           " +
                "      ,[Copyright_Licence]        " +
                "      ,[Link_Href]                " +
                "      ,[Link_Text]                " +
                "      ,[MimeType]                 " +
                "      ,[Uri]                      " +
                "      ,[IdTrack]                  " +
                "      ,[minlat]                   " +
                "      ,[minlon]                   " +
                "      ,[maxlat]                   " +
                "      ,[maxlon]                   " +
                "FROM[rutoteca].[dbo].[GpxMetadata]";
    }
}
