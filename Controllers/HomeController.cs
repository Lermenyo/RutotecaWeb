using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RutotecaWeb.Data;
using RutotecaWeb.Models;

namespace RutotecaWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        //public IActionResult Index()
        //{
        //    return View();
        //}
        [Route("{id}")]
        public IActionResult GetByPermalink(string id)
        {
            if (!String.IsNullOrEmpty(id))
            {
                var _ruta = new SqlServerRutaQueries().GetRutasByPermalink(id);
                return View("Ruta", _ruta);
            }
            else
                return View();
        }

        public IActionResult Index()
        {
            var _ruta = new SqlServerRutaQueries().GetRutasByPermalink("PR-AS-0001-1-hoces-del-esva-i");
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
