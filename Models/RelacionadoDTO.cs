using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RutotecaWeb.Models
{
    public class RelacionadoDTO
    {
        public int ID { get; set; }
        public string Nombre { get; set; }
        public string Permalink { get; set; }
        public string DescripcionCorta { get; set; }
        public int ImportanciaIntrinseca { get; set; }
    }
}
