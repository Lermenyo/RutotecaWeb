using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RutotecaWeb.Models
{
    public class CercanoDTO
    {
        public int ID { get; set; }
        public int IdElementoRuta { get; set; }
        public string Nombre { get; set; }
        public string Permalink { get; set; }
        public string DescripcionCorta { get; set; }
        public int IdTipoElemento { get; set; }
        public string TipoElemento { get; set; }
        public int Metros { get; set; }
        public int ImportanciaIntrinseca { get; set; }
    }
}
