using Business.Concrete;
using DataAccess.Concrete;
using Entity.Concrete;
using Frontend1.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Frontend1.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [HttpPost]
        [Route("/Home/YeniBolumEkle")]
        public IActionResult YeniBolumEkle([FromBody] string bolumAdi)
        {
            BolumManager bolumManager = new BolumManager(new BolumDal());
            Bolum bolum = new Bolum();
            bolum.BolumAdi = bolumAdi;
            bolumManager.Add(bolum);
            return Ok();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
