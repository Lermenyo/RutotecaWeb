using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RutotecaWeb.Models;
using RutotecaWeb.Services;

namespace RutotecaWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IDapper _dapper;
        private readonly ITrace _traza;

        public HomeController(ILogger<HomeController> logger, IDapper dapper, ITrace trace)
        {
            _logger = logger;
            _dapper = dapper;
            _traza = trace;
        }

        private void TrazaAcceso() {
            if (ViewBag.IdAcceso == null)
                ViewBag.IdAcceso = _traza.GetAcceso(Request);
        }

        [Route("{id}")]
        public async Task<IActionResult> GetByPermalinkAsync(string id)
        {
            TrazaAcceso();

            try
            {
                _traza.SetTraza(String.Concat("Acceso: ", id), ViewBag.IdAcceso, Nivel.Info, accion: Accion.GetByPermalink);
                _traza.InsertEntrada(ViewBag.IdAcceso, Request.Headers["Referer"].ToString(), id);

                if (!String.IsNullOrEmpty(id))
                {
                    var _par = new Dapper.DynamicParameters();
                    var _elemento = await Task.FromResult(_dapper.Get<ElementoDTO>(Dapperr.BasDB, $"select * FROM vwPermalink where Permalink='{id}'", null, commandType: CommandType.Text));
                    switch (_elemento.IdTipoElemento)
                    {
                        case 1:  //Ruta
                            _traza.SetTraza("Acceso Ruta", ViewBag.IdAcceso,  Nivel.Info, elemento: _elemento.Id, subElemento: SubElemento.Ruta, accion: Accion.GetByPermalink);
                            return await SetRuta(_elemento.Id);
                        case 2:  //Localidad
                            _traza.SetTraza("Acceso Localidad", ViewBag.IdAcceso, Nivel.Info, elemento: _elemento.Id, subElemento: SubElemento.Localidad, accion: Accion.GetByPermalink);
                            return await SetLocalidad(_elemento.Id);
                        case 3:  //Vertice
                            _traza.SetTraza("Acceso Vertice", ViewBag.IdAcceso, Nivel.Info, elemento: _elemento.Id, subElemento: SubElemento.Vertice, accion: Accion.GetByPermalink);
                            return await SetVertice(_elemento.Id);
                        case 4:  //Monumento
                        case 5:  //Evento
                        case 10:  //Etiqueta
                            _traza.SetTraza("Acceso Etiqueta", ViewBag.IdAcceso, Nivel.Info, elemento: _elemento.Id, subElemento: SubElemento.Tag, accion: Accion.GetByPermalink);
                            return await SetTag(_elemento.Id);
                        case 20:  //Documento
                        default:
                            _traza.SetTraza(String.Concat("Elemento no implementado: ", _elemento.IdTipoElemento), ViewBag.IdAcceso, Nivel.Warning, accion: Accion.GetByPermalink);
                            return View("Index");
                    }

                }
                else
                {
                    _traza.SetTraza("id Vacio/Nulo => Index", ViewBag.IdAcceso, Nivel.Info, accion: Accion.GetByPermalink);
                    return View("Index");
                }
            }
            catch (Exception ex)
            {
                _traza.InsertException(ex, ViewBag.IdAcceso, accion: Accion.GetByPermalink);
                return View("Index");
            }
        }

        private async Task<IActionResult> SetRuta(int idElemento)
        {
            var _ruta = await Task.FromResult(_dapper.Get<RutaDTO>(Dapperr.BasDB, $"select * FROM vwRutas where IdElemento={idElemento}", null, commandType: CommandType.Text));
            _ruta.TablaExistencias = await Task.FromResult(_dapper.Get<TablaExistenciasDTO>(Dapperr.AuxDB, $"select * FROM AuxTablaExistencias where IdElemento={idElemento}", null, commandType: CommandType.Text));
            if (_ruta != null)
            {
                ViewData["Title"] = String.Format("{0} {1} {2} - {3}", _ruta.CodigoTipo, _ruta.CodigoLugar, _ruta.Numero, _ruta.Nombre);
                //ViewData["MetaDescription"] = String.Format("{0}", _ruta.Descripcion);
                return View("Ruta", _ruta);
            }
            else
                return View("Index");
        }

        private async Task<IActionResult> SetLocalidad(int idElemento)
        {
            var _localidad = await Task.FromResult(_dapper.Get<LocalidadDTO>(Dapperr.BasDB, $"select * FROM vwLocalidades where IdElemento={idElemento}", null, commandType: CommandType.Text));
            if (_localidad != null)
            {
                ViewData["Title"] = String.Format("{0}", _localidad.Nombre);
                ViewData["MetaDescription"] = String.Format("{0}", _localidad.DescripcionCorta);
                return View("Localidad", _localidad);
            }
            else
                return View("Index");
        }

        private async Task<IActionResult> SetVertice(int idElemento)
        {
            var _vertice = await Task.FromResult(_dapper.Get<VerticeDTO>(Dapperr.BasDB, $"select * FROM vwVertices where IdElemento={idElemento}", null, commandType: CommandType.Text));
            if (_vertice != null)
            {
                ViewData["Title"] = String.Format("{0} - {1}", _vertice.Nombre, _vertice.DescripcionCorta);
                ViewData["MetaDescription"] = String.Format("{0}", _vertice.DescripcionCorta);
                return View("Vertice", _vertice);
            }
            else
                return View("Index");
        }

        private async Task<IActionResult> SetTag(int idElemento)
        {
            var _tag = await Task.FromResult(_dapper.Get<TagDTO>(Dapperr.BasDB, $"select * FROM vwVertices where IdElemento={idElemento}", null, commandType: CommandType.Text));
            if (_tag != null)
            {
                ViewData["Title"] = String.Format("{0} - {1}", _tag.Nombre, _tag.DescripcionCorta);
                ViewData["MetaDescription"] = String.Format("{0}", _tag.DescripcionCorta);
                return View("Tag", _tag);
            }
            else
                return View("Index");
        }

        public JsonResult GetAltimetrias(int id)
        {
            TrazaAcceso();
            _traza.SetTraza(String.Concat("GetAltimetrias Id: ", id.ToString()), ViewBag.IdAcceso, Nivel.Info, accion: Accion.GetAltimetrias);

            try
            {
                var datos = _dapper.Get<AtimetriaDTO>(Dapperr.AuxDB, $"select * FROM AuxAltimetrias where idRuta = {id}", null, commandType: CommandType.Text);
                if (datos != null)
                    return Json(System.Text.Json.JsonSerializer.Deserialize<object>(datos.Json));
                else
                    return Json("NoData");
            }
            catch (Exception ex)
            {
                _traza.InsertException(ex , ViewBag.IdAcceso, accion: Accion.GetAltimetrias);
                return Json("NoData");
            }
        }

        public async Task<IList<CercanoDTO>> LoadCercanosAsync(int id)
        {
            TrazaAcceso();
            _traza.SetTraza(String.Concat("LoadCercanosAsync Id: ", id.ToString()), ViewBag.IdAcceso, Nivel.Info, accion: Accion.LoadCercanos);

            try
            {
                return await Task.FromResult(_dapper.GetAll<CercanoDTO>(Dapperr.BasDB, $"select * FROM vwElementosCercanos where ID ={id}", null, commandType: CommandType.Text));
            }
            catch (Exception ex)
            {
                _traza.InsertException(ex, ViewBag.IdAcceso, accion: Accion.LoadCercanos);
                return null;
            }
        }

        [HttpGet]
        public ActionResult GetLugaresCercanos (int id)
        {
            TrazaAcceso();
            _traza.SetTraza(String.Concat("GetLugaresCercanos Id: ", id.ToString()), ViewBag.IdAcceso, Nivel.Info, accion: Accion.LugaresCercanos);

            try
            {
                var _idsElemento = _dapper.GetAll<int>(Dapperr.AuxDB, $"select IdElemento2  FROM AuxCercania where IdElemento1 = {id}", null, commandType: CommandType.Text);
                var _cecanos = _dapper.GetAll<CercanoDTO>(Dapperr.BasDB, $"SELECT {id} as ID, T2.Nombre, T2.Permalink, T2.DescripcionCorta, T2.IdTipoElemento, T3.Nombre AS TipoElemento, T2.ImportanciaIntrinseca FROM dbo.Elementos T2 INNER JOIN dbo.TiposElemento AS T3 ON T2.IdTipoElemento = T3.Id where T2.Id IN (" + String.Join(',',_idsElemento) + ")", null, commandType: CommandType.Text);
                return PartialView("_ListaCercanos", _cecanos);
            }
            catch (Exception ex)
            {
                _traza.InsertException(ex, ViewBag.IdAcceso, accion: Accion.LugaresCercanos);
                return null;
            }
        }

        [HttpGet]
        public ActionResult GetArchivos(int id)
        {
            TrazaAcceso();
            _traza.SetTraza(String.Concat("GetArchivos Id: ", id.ToString()), ViewBag.IdAcceso, Nivel.Info, accion: Accion.Archivos);

            try
            {
                var _archivos = _dapper.GetAll<ArchivosDTO>(Dapperr.BasDB, $"select * FROM vwArchivos where IdElemento ={id}", null, commandType: CommandType.Text);
                return PartialView("_ListaArchivos", _archivos);
            }
            catch (Exception ex)
            {
                _traza.InsertException(ex, ViewBag.IdAcceso, accion: Accion.Archivos);
                return null;
            }
        }

        [HttpGet]
        public ActionResult GetImagenes(int id)
        {
            TrazaAcceso();
            _traza.SetTraza(String.Concat("GetImagenes Id: ", id.ToString()), ViewBag.IdAcceso, Nivel.Info, accion: Accion.Imagenes);

            try
            {
                //https://documentos.rutoteca.es/SL-CV-0168/perfil_302.jpg
                var _imagenes = _dapper.GetAll<ImagenesDTO>(Dapperr.BasDB, $"select * FROM vwImagenes where IdElemento ={id}", null, commandType: CommandType.Text);
                return PartialView("_CarruselImagenes", _imagenes);
            }
            catch (Exception ex)
            {
                _traza.InsertException(ex, ViewBag.IdAcceso, accion: Accion.Imagenes);
                return null;
            }
        }

        [HttpGet]
        public ActionResult GetTracks(int id)
        {
            TrazaAcceso();
            _traza.SetTraza(String.Concat("GetTracks Id: ", id.ToString()), ViewBag.IdAcceso, Nivel.Info, accion: Accion.Tracks);

            try
            {
                var colors = new List<string>() {
                  "#ff0000"//Rojo
                , "#0000ff"//Azul
                , "#003300"//Verde Oscuro
                , "#000000"//Negro
                , "#660066"//Violeta
                , "#ff00ff"//Fucsia
                , "#003366"//AzulMarino
                , "#663300"//Marron
                , "#ff3300"//Naranja
                , "#3399ff"//AzulClaro
                , "#ff9999"//RosaClaro
                , "#ffffff"//Blanco
                };

                ///home/getgpx/
                var _tracks = _dapper.GetAll<TracksListadoDTO>(Dapperr.GpxDB, TracksListadoDTO.SELECT_COMPLETA + $" WHERE T4.IDELEMENTO = {id}", null, commandType: CommandType.Text);
                _tracks.ForEach(t => { t.Color = colors[_tracks.IndexOf(t)]; });
                return PartialView("_ListaTracks", _tracks);
            }
            catch (Exception ex)
            {
                _traza.InsertException(ex, ViewBag.IdAcceso, accion: Accion.Tracks);
                return null;
            }
        }

        [HttpGet]
        public JsonResult GetPuntoElemento(int id)
        {
            TrazaAcceso();
            _traza.SetTraza(String.Concat("GetPuntoElemento Id: ", id.ToString()), ViewBag.IdAcceso, Nivel.Info, accion: Accion.PuntoElemento);

            try
            {
                return Json(GetPuntoByIdElemento(id));
            }
            catch (Exception ex)
            {
                _traza.InsertException(ex, ViewBag.IdAcceso,  accion: Accion.PuntoElemento);
                return null;
            }
        }

        public PuntoMapa GetPuntoByIdElemento(int id)
        {
            var select1 = "SELECT        dbo.Elementos.Id, dbo.Coordenadas.Latitud, dbo.Coordenadas.Longitud, 13 AS Zoom" +
                           "        FROM dbo.Elementos INNER JOIN" + 
                           "             dbo.Elementos_Lugar ON dbo.Elementos.Id = dbo.Elementos_Lugar.IdElemento INNER JOIN" +
                           "             dbo.Coordenadas ON dbo.Elementos_Lugar.IdCoordenada = dbo.Coordenadas.Id";
            var select2 = "SELECT        0 as IdElemento, T2.IntermedioLat AS Latitud, T2.IntermedioLon AS Longitud, 13 AS Zoom"+
                          "         FROM AuxMapaTrack";

            var datos = _dapper.Get<PuntoMapa>(Dapperr.BasDB, select1 + $"where ID ={id}", null, commandType: CommandType.Text);
            if (datos == null)
                datos = _dapper.Get<PuntoMapa>(Dapperr.AuxDB, select2 + $"where IdRuta ={id}", null, commandType: CommandType.Text);
            return datos;
        }

        [HttpGet]
        public ActionResult GetRutasCercanas(int id)
        {
            TrazaAcceso();
            _traza.SetTraza(String.Concat("GetRutasCercanas Id: ", id.ToString()), ViewBag.IdAcceso, Nivel.Info, accion: Accion.RutasCercanas);

            try
            {
                var select =
                $"SELECT        " +
                "{id} AS ID, " +
                "T2.Nombre, " +
                "T2.Permalink, " +
                "T2.DescripcionCorta, " +
                "T2.IdTipoElemento, " +
                "T3.Nombre AS TipoElemento, " +
                "T1.Metros, " +
                "T2.ImportanciaIntrinseca, " +
                "T1.IdElemento1 AS IdElementoRuta " +
                "FROM " +
                "      dbo.Elementos AS T2 " + 
                "      dbo.TiposElemento AS T3 ON T2.IdTipoElemento = T3.Id";
                var ids = _dapper.GetAll<int>(Dapperr.AuxDB, $"select IdElemento1 FROM AuxCercania where IdElemento2 = {id}", null, commandType: CommandType.Text);
                var _cecanos = _dapper.GetAll<CercanoDTO>(Dapperr.BasDB, select + " where ID in (" + String.Join(',',ids) + ")", null, commandType: CommandType.Text);
                return PartialView("_ListaCercanos", _cecanos);
            }
            catch (Exception ex)
            {
                _traza.InsertException(ex, ViewBag.IdAcceso, accion: Accion.RutasCercanas);
                return null;
            }
        }
        [HttpGet]
        public ActionResult GetRelacionados(int id)
        {
            TrazaAcceso();
            _traza.SetTraza(String.Concat("GetRelacionados Id: ", id.ToString()), ViewBag.IdAcceso, Nivel.Info, accion: Accion.Relacionados);

            try
            {
                var _cecanos = _dapper.GetAll<RelacionadoDTO>(Dapperr.AuxDB, $"select * FROM AuxVwTagsRuta where Id ={id}", null, commandType: CommandType.Text);
                return PartialView("_ListaRelacionados", _cecanos);
            }
            catch (Exception ex)
            {
                _traza.InsertException(ex, ViewBag.IdAcceso,  accion: Accion.Relacionados);
                return null;
            }
        }

        [HttpGet]
        public ActionResult GetRutasEnTag(int id)
        {
            TrazaAcceso();
            _traza.SetTraza(String.Concat("GetRutasEnTag Id: ", id.ToString()), ViewBag.IdAcceso, Nivel.Info, accion: Accion.RutasEnTag);

            try
            {
                var _cecanos = _dapper.GetAll<RelacionadoDTO>(Dapperr.AuxDB, $"select * FROM AuxVwRutasTag where Id ={id}", null, commandType: CommandType.Text);
                return PartialView("_ListaRelacionados", _cecanos);
            }
            catch (Exception ex)
            {
                _traza.InsertException(ex, ViewBag.IdAcceso,  accion: Accion.RutasEnTag);
                return null;
            }
        }

        [HttpGet]
        public List<MapaDTO> GetMapa(int id)
        {
            TrazaAcceso();
            _traza.SetTraza(String.Concat("GetMapa Id: ", id.ToString()), ViewBag.IdAcceso, Nivel.Info, accion: Accion.Mapa);

            try
            {
                var _datosMapa = _dapper.GetAll<MapaDTO>(Dapperr.AuxDB, $"select * FROM AuxMapaTrack where IdRuta ={id}", null, commandType: CommandType.Text);

                if (_datosMapa != null && _datosMapa.Count > 0)
                    return _datosMapa;
                else
                    return new List<MapaDTO>();
            }
            catch (Exception ex)
            {
                _traza.InsertException(ex, ViewBag.IdAcceso,  accion: Accion.Mapa);
                return null;
            }
        }

        [HttpGet]
        public List<MapaDTO> GetMapaTracks(int id)
        {
            TrazaAcceso();
            _traza.SetTraza(String.Concat("GetMapaTracks Id: ", id.ToString()), ViewBag.IdAcceso, Nivel.Info, accion: Accion.MapaTracks);

            try
            {
                var _datosMapa = _dapper.GetAll<MapaDTO>(Dapperr.AuxDB, $"select * FROM AuxMapaTrack where IdRuta ={id}", null, commandType: CommandType.Text);

                if (_datosMapa != null && _datosMapa.Count > 0)
                    return _datosMapa;
                else
                    return new List<MapaDTO>();
            }
            catch (Exception ex)
            {
                _traza.InsertException(ex, ViewBag.IdAcceso,  accion: Accion.MapaTracks);
                return null;
            }
        }

        //https://localhost:5001/home/getgpx/538
        [HttpGet]
        public ActionResult GetGPX(int id)
        {
            TrazaAcceso();
            _traza.SetTraza(String.Concat("GetGPX Id: ", id.ToString()), ViewBag.IdAcceso, Nivel.Info, accion: Accion.GPX);

            try
            {
                MemoryStream ms = new MemoryStream();
                var track = new Track();
                track.GpxPoint.AddRange(_dapper.GetAll<GpxPoint>(Dapperr.GpxDB, GpxPoint.SELECT_COMPLETA + $" WHERE IdTrack = {id} ORDER BY 1", null, commandType: CommandType.Text));
                var servicioGPX = new Services.GestorGpx(track);
                servicioGPX.SaveInStream(ms);
                return File(ms.ToArray(), "application/gpx+xml", "track.gpx");
            }
            catch (Exception ex)
            {
                _traza.InsertException(ex, ViewBag.IdAcceso,  accion: Accion.GPX);
                return null;
            }
        }

        [HttpGet]
        public List<TrackLugarDTO> GetTracksLugar(int id)
        {
            TrazaAcceso();
            _traza.SetTraza(String.Concat("GetTracksLugar Id: ", id.ToString()), ViewBag.IdAcceso, Nivel.Info, accion: Accion.TracksLugar);

            try
            {
                var _datosMapa = _dapper.GetAll<TrackLugarDTO>(Dapperr.AuxDB, $"select * FROM vwTracksLugar where IdLugar ={id}", null, commandType: CommandType.Text);

                if (_datosMapa != null && _datosMapa.Count > 0)
                    return _datosMapa;
                else
                    return new List<TrackLugarDTO>();
            }
            catch (Exception ex)
            {
                _traza.InsertException(ex, ViewBag.IdAcceso,  accion: Accion.TracksLugar);
                return null;
            }
        }


        [HttpGet]
        public ActionResult GetMeteoblueMapa(int id)
        {
            TrazaAcceso();
            _traza.SetTraza(String.Concat("GetMeteoblueMapa Id: ", id.ToString()), ViewBag.IdAcceso, Nivel.Info, accion: Accion.MeteoblueMapa);

            try
            {
                var punto = GetPuntoByIdElemento(id);
                var url = String.Format("https://www.meteoblue.com/es/tiempo/maps/widget/{0:0.000}{1}{2:0.000}{3}?windAnimation=0&windAnimation=1&gust=0&gust=1&satellite=0&satellite=1&coronaWeatherScore=0&coronaWeatherScore=1&geoloc=fixed&tempunit=C&windunit=km%252Fh&lengthunit=metric&zoom=10&autowidth=auto",
                   punto.Latitud,
                   (punto.Latitud > 0) ? 'N' : 'S',
                   punto.Longitud,
                   (punto.Longitud < 0) ? 'E' : 'O').Replace(",", ".");
                return PartialView("_MeteoBlueMapa", url);
            }
            catch (Exception ex)
            {
                _traza.InsertException(ex, ViewBag.IdAcceso, accion: Accion.MeteoblueMapa);
                return null;
            }
        }

        [HttpGet]
        public ActionResult GetMeteoblue3h(int id)
        {
            TrazaAcceso();
            _traza.SetTraza(String.Concat("GetMeteoblue3h Id: ", id.ToString()), ViewBag.IdAcceso, Nivel.Info, accion: Accion.Meteoblue3h);

            try
            {
                var punto = GetPuntoByIdElemento(id);
                var url = String.Format("https://www.meteoblue.com/es/tiempo/widget/three/{0:0.000}{1}{2:0.000}{3}?geoloc=fixed&nocurrent=0&noforecast=0&days=4&tempunit=CELSIUS&windunit=KILOMETER_PER_HOUR&layout=image",
                   punto.Latitud,
                   (punto.Latitud > 0) ? 'N' : 'S',
                   punto.Longitud,
                   (punto.Longitud < 0) ? 'E' : 'O').Replace(",", ".");
                return PartialView("_MeteoBlue3h", url);
            }
            catch (Exception ex)
            {
                _traza.InsertException(ex, ViewBag.IdAcceso, accion: Accion.Meteoblue3h);
                return null;
            }
        }

        public IActionResult Index()
        {
            TrazaAcceso();
            _traza.SetTraza("Index", ViewBag.IdAcceso, Nivel.Info, accion: Accion.Base);

            try
            {
                return View();
            }
            catch (Exception ex)
            {
                _traza.InsertException(ex, ViewBag.IdAcceso, accion: Accion.Base);
                return null;
            }
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            TrazaAcceso();
            _traza.SetTraza("Error", ViewBag.IdAcceso, Nivel.Error, accion: Accion.Error);
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
