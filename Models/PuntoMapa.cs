using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RutotecaWeb.Models
{
    public class PuntoMapa
    {
        public int Id { get; set; }
        public float Latitud { get; set; }
        public float Longitud { get; set; }
        public int Zoom { get; set; }
    }
}
