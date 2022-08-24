using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using TorneioLuta.Domain.DTO;
using TorneioLuta.Domain.Entities;
using TorneioLuta.Interfaces.Application;
using TorneioLuta.Models;
using TorneioLuta.CrossCutting;
using System.Linq;

namespace TorneioLuta.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private ILutadoresApp _lutadoresApp;
        private ITorneioApp _torneioApp;

        public HomeController(ILogger<HomeController> logger,
            ILutadoresApp lutadoresApp,
            ITorneioApp torneioApp)
        {
            _logger = logger;
            _lutadoresApp = lutadoresApp;
            _torneioApp = torneioApp;
        }

        public IActionResult Index()
        {
            List<Lutador> lutadores = new List<Lutador>();

            try
            {
                lutadores.AddRange(_lutadoresApp.GetAll());
            }
            catch (Exception)
            {
                lutadores = new List<Lutador>();
            }

            return View(lutadores);
        }

        public IActionResult Resultado(string ids)
        {
            List<GrupoDTO> resultadoTorneio = null;
            try
            {
                List<LutadoresSelectedDTO> lutadoresIds = JsonFunctionsCrossCutting.ConvertForDTO(ids);
                resultadoTorneio = _torneioApp.IniciarTorneio(lutadoresIds);
            }
            catch (Exception)
            {
                resultadoTorneio = new List<GrupoDTO>();
            }

            return View(resultadoTorneio);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
