using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RutotecaWeb.Models
{
    public class MapaDTO
    {
        public int IdTrack            { get; set; }
        public int IdRuta             {get; set;}
        public string Puntos          {get; set;}
        public float IntermedioLat    {get; set;}
        public float IntermedioLon    {get; set;}
        public float DiferenciaMayor  {get; set;}
        public float DiferenciaMenor  {get; set;}
        public bool LongitudMayor     {get; set;}

    }
}
