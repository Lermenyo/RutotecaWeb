using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
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
                var _back = new BackEnd.Domain.Ruta();
                var _ruta = _back.GetByPermalink(id);
                return View("Index", _ruta);
            }
            else
                return View();
        }

        public IActionResult Index()
        {
            var _back = new BackEnd.Domain.Ruta();
            var _ruta = _back.GetByPermalink("SL-AN-0099-Barranco-de-la-Lana-(El-Ronquillo)");
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
