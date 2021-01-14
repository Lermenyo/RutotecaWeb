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
                var _ruta = await Task.FromResult(_dapper.Get<RutaDTO>($"select * FROM vwRutas where Permalink='{id}'", null, commandType: CommandType.Text));
                return View("Ruta", _ruta);
            }
            else
                return View();
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
