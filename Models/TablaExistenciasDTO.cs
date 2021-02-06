using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RutotecaWeb.Models
{
    public class TablaExistenciasDTO
    {
        public bool IdElemento          { get; set; }
        public bool TieneMapa           {get; set;}
        public bool TieneAltimetrias    {get; set;}
        public bool TieneImagenes       {get; set;}
        public bool TieneArchivos       {get; set;}
        public bool TieneCercanos       {get; set;}
        public bool TieneTags           {get; set;}
        public bool TieneMeteo          {get; set;}
        public bool TieneTrack          {get; set;}
        public int  NumeroTracks        {get; set;}
        public bool TieneIBP            {get; set;}
        public bool TieneMide           {get; set;}
        public bool TieneEstadisticas   {get; set;}
    }
}
