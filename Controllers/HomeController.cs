using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
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

        public HomeController(ILogger<HomeController> logger, IDapper dapper)
        {
            _logger = logger;
            _dapper = dapper;
        }

        [Route("{id}")]
        public async Task<IActionResult> GetByPermalinkAsync(string id)
        {
            if (!String.IsNullOrEmpty(id))
            {
                var _par = new Dapper.DynamicParameters();
                var _elemento = await Task.FromResult(_dapper.Get<ElementoDTO>($"select * FROM vwPermalink where Permalink='{id}'", null, commandType: CommandType.Text));
                switch (_elemento.IdTipoElemento)
                {
                    case 1:  //Ruta
                        return await SetRuta(_elemento.Id);
                    case 2:  //Localidad
                        return await SetLocalidad(_elemento.Id);
                    case 3:  //Vertice
                        return await SetVertice(_elemento.Id);
                    case 4:  //Monumento
                    case 5:  //Evento
                    case 10:  //Etiqueta
                        return await SetTag(_elemento.Id);
                    case 20:  //Documento
                    default:
                        return View("Index");
                }

            }
            else
                return View("Index");
        }

        private async Task<IActionResult> SetRuta(int idElemento)
        {
            var _ruta = await Task.FromResult(_dapper.Get<RutaDTO>($"select * FROM vwRutas where IdElemento={idElemento}", null, commandType: CommandType.Text));
            _ruta.TablaExistencias = await Task.FromResult(_dapper.Get<TablaExistenciasDTO>($"select * FROM AuxTablaExistencias where IdElemento={idElemento}", null, commandType: CommandType.Text));
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
            var _localidad = await Task.FromResult(_dapper.Get<LocalidadDTO>($"select * FROM vwLocalidades where IdElemento={idElemento}", null, commandType: CommandType.Text));
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
            var _vertice = await Task.FromResult(_dapper.Get<VerticeDTO>($"select * FROM vwVertices where IdElemento={idElemento}", null, commandType: CommandType.Text));
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
            var _tag = await Task.FromResult(_dapper.Get<TagDTO>($"select * FROM vwVertices where IdElemento={idElemento}", null, commandType: CommandType.Text));
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
            var datos = _dapper.Get<AtimetriaDTO>($"select * FROM AuxAltimetrias where idRuta = {id}", null, commandType: CommandType.Text);
            if (datos != null)
                return Json(System.Text.Json.JsonSerializer.Deserialize<object>(datos.Json));
            else
                return Json("NoData");
        }

        public async Task<IList<CercanoDTO>> LoadCercanosAsync(int id)
        { 
            return await Task.FromResult(_dapper.GetAll<CercanoDTO>($"select * FROM vwElementosCercanos where ID ={id}", null, commandType: CommandType.Text));
        }

        [HttpGet]
        public ActionResult GetLugaresCercanos (int id)
        {
            var _cecanos = _dapper.GetAll<CercanoDTO>($"select * FROM vwLugaresEnRuta where ID ={id}", null, commandType: CommandType.Text);
            return PartialView("_ListaCercanos", _cecanos);
        }

        [HttpGet]
        public ActionResult GetArchivos(int id)
        {
            var _archivos = _dapper.GetAll<ArchivosDTO>($"select * FROM vwArchivos where IdElemento ={id}", null, commandType: CommandType.Text);
            return PartialView("_ListaArchivos", _archivos);
        }

        [HttpGet]
        public ActionResult GetImagenes(int id)
        {
            //https://documentos.rutoteca.es/SL-CV-0168/perfil_302.jpg
            var _imagenes = _dapper.GetAll<ImagenesDTO>($"select * FROM vwImagenes where IdElemento ={id}", null, commandType: CommandType.Text);
            return PartialView("_CarruselImagenes", _imagenes);
        }


        [HttpGet]
        public JsonResult GetPuntoElemento(int id)
        {
            return Json(GetPuntoByIdElemento(id));
        }

        
        public PuntoMapa GetPuntoByIdElemento(int id)
        {
            return _dapper.Get<PuntoMapa>($"select * FROM vwPuntoElemento where ID ={id}", null, commandType: CommandType.Text);
        }

        [HttpGet]
        public ActionResult GetRutasCercanas(int id)
        {
            var _cecanos = _dapper.GetAll<CercanoDTO>($"select * FROM vwRutasEnLugar where ID ={id}", null, commandType: CommandType.Text);
            return PartialView("_ListaCercanos", _cecanos);
        }
        [HttpGet]
        public ActionResult GetRelacionados(int id)
        {
            var _cecanos = _dapper.GetAll<RelacionadoDTO>($"select * FROM vwTagsRuta where Id ={id}", null, commandType: CommandType.Text);
            return PartialView("_ListaRelacionados", _cecanos);
        }

        [HttpGet]
        public ActionResult GetRutasEnTag(int id)
        {
            var _cecanos = _dapper.GetAll<RelacionadoDTO>($"select * FROM vwRutasTag where Id ={id}", null, commandType: CommandType.Text);
            return PartialView("_ListaRelacionados", _cecanos);
        }

        [HttpGet]
        public List<MapaDTO> GetMapa(int id)
        {
            var _datosMapa = _dapper.GetAll<MapaDTO>($"select * FROM AuxMapaTrack where IdRuta ={id}", null, commandType: CommandType.Text);

            if (_datosMapa != null && _datosMapa.Count > 0)
                return _datosMapa;
            else
                return new List<MapaDTO>();
        }
        [HttpGet]
        public List<TrackLugarDTO> GetTracksLugar(int id)
        {
            var _datosMapa = _dapper.GetAll<TrackLugarDTO>($"select * FROM vwTracksLugar where IdLugar ={id}", null, commandType: CommandType.Text);

            if (_datosMapa != null && _datosMapa.Count > 0)
                return _datosMapa;
            else
                return new List<TrackLugarDTO>();
        }
        

        [HttpGet]
        public ActionResult GetMeteoblueMapa(int id)
        {
            var punto = GetPuntoByIdElemento(id);
            var url = String.Format("https://www.meteoblue.com/es/tiempo/maps/widget/{0:0.000}{1}{2:0.000}{3}?windAnimation=0&windAnimation=1&gust=0&gust=1&satellite=0&satellite=1&coronaWeatherScore=0&coronaWeatherScore=1&geoloc=fixed&tempunit=C&windunit=km%252Fh&lengthunit=metric&zoom=10&autowidth=auto",
               punto.Latitud,
               (punto.Latitud > 0) ? 'N' : 'S',
               punto.Longitud,
               (punto.Longitud < 0) ? 'E' : 'O').Replace(",", ".");
            return PartialView("_MeteoBlueMapa", url);
        }

        [HttpGet]
        public ActionResult GetMeteoblue3h(int id)
        {
            var punto = GetPuntoByIdElemento(id);
            var url = String.Format("https://www.meteoblue.com/es/tiempo/widget/three/{0:0.000}{1}{2:0.000}{3}?geoloc=fixed&nocurrent=0&noforecast=0&days=4&tempunit=CELSIUS&windunit=KILOMETER_PER_HOUR&layout=image",
               punto.Latitud,
               (punto.Latitud > 0)?'N':'S',
               punto.Longitud,
               (punto.Longitud < 0) ? 'E' : 'O').Replace(",", ".");
            return PartialView("_MeteoBlue3h", url);
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
