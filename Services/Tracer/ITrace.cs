using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RutotecaWeb.Services
{
    public interface ITrace
    {
        int GetAcceso(HttpRequest request);
        void SetTraza(string mensaje, int acceso = -1, Nivel nivel = 0, SubNivel subNivel = 0, int elemento = 0, SubElemento subElemento = 0, Terciario terciario = 0, Accion accion = 0, SubAccion subAccion = 0);
        void InsertException(Exception ex, int acceso = -1, int elemento = 0, SubElemento subElemento = 0, Terciario terciario = 0, Accion accion = 0, SubAccion subAccion = 0);
        void InsertEntrada(int? acceso, string origen, string destino);
    }
}
