using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RutotecaWeb.Models
{
    public class VerticeDTO
    {
            public int IdElemento { get; set; }
            public string Nombre { get; set; }
            public string DescripcionCorta { get; set; }
            public IList<CercanoDTO> Cecanos { get; set; }
            public VerticeDTO()
            {
                Cecanos = new List<CercanoDTO>();
            }
    }
}
