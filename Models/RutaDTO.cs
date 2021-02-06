using RutotecaWeb.Services;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace RutotecaWeb.Models
{
    public class RutaDTO
    {
        public int Id               { get; set; }
        public int IdElemento { get; set; }
        public string Permalink     { get; set; }
        public string Codigo        { get; set; }
        public string Nombre        { get; set; }
        public decimal? Longitud    { get; set; }
        public int? IBP           { get; set; }
        public bool? Circular      { get; set; }
        public string Dificultad    { get; set; }
        public string Duracion      { get; set; }
        public int? Ascenso       { get; set; }
        public int? Descenso      { get; set; }
        public string CodigoTipo    { get; set; }
        public int? Numero        { get; set; }
        public string Version       { get; set; }
        public string SubVersion    { get; set; }
        public string Etapa         { get; set; }
        public string CodigoLugar   { get; set; }
        public bool? Homologado    { get; set; }
        public DateTime? FechaRevision { get; set; }
        public string Descripcion   { get; set; }
        public string Informacion   { get; set; }

        public IList<CercanoDTO> Cecanos { get; set; }

        public TablaExistenciasDTO TablaExistencias { get; set; }

        public RutaDTO()
        {
            Cecanos = new List<CercanoDTO>();
        }

    }
}
