using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RutotecaWeb.Models
{
    public class ImagenesDTO
    {
        public string Url { get; set; }
        public string NombreWeb { get; set; }
        public string Descripcion { get; set; }
        public string Propio { get; set; }
        public string Origen { get; set; }
        public int IdTipoDocumento { get; set; }
        public int IdElemento { get; set; }
    }
}
