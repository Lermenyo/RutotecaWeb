using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RutotecaWeb.DataContext;
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

        public async Task<IList<AtimetriaDTO>> LoadAltimetriasAsync(int id)
        {
            return await Task.FromResult(_dapper.GetAll<AtimetriaDTO>($"select * FROM vwAltimetria where idRuta = {id} ORDER BY idTrack, Orden", null, commandType: CommandType.Text));
        }

        public async Task<IList<CercanoDTO>> LoadCercanosAsync(int id)
        { 
            return await Task.FromResult(_dapper.GetAll<CercanoDTO>($"select * FROM vwElementosCercanos where ID ={id}", null, commandType: CommandType.Text));
        }


        [HttpGet]
        public ActionResult GetPerfil(int id)
        {
            //var _altimetrias = _dapper.GetAll<AtimetriaDTO>($"select * FROM vwAltimetria where idRuta = {id} ORDER BY idTrack, Orden", null, commandType: CommandType.Text);
            //return PartialView("_PerfilDeRuta", _altimetrias);
            return PartialView("_PerfilDeRuta");
        }
        
        [HttpGet]
        public ActionResult GetCercanos (int id)
        {
            var _cecanos = _dapper.GetAll<CercanoDTO>($"select * FROM vwElementosCercanos where ID ={id}", null, commandType: CommandType.Text);
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
