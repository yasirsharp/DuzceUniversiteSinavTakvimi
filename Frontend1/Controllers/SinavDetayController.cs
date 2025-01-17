using Microsoft.AspNetCore.Mvc;
using Business.Abstract;
using Entity.Concrete;

namespace Frontend1.Controllers
{
    public class SinavDetayController : Controller
    {
        private readonly ISinavDetayService _sinavDetayService;
        private readonly IDBAPService _dBAPService;
        private readonly IDerslikService _derslikService;

        public SinavDetayController(ISinavDetayService sinavDetayService, IDBAPService dBAPService, IDerslikService derslikService)
        {
            _sinavDetayService = sinavDetayService;
            _dBAPService = dBAPService;
            _derslikService = derslikService;
        }

        // Tüm sınav detaylarını listeleyen action
        public IActionResult Index()
        {
            ViewData["DBAP"]= _dBAPService.GetAll() ;
            ViewData["DBAPDetail"]= _dBAPService.GetAllDetails() ;
            ViewData["SinavDetay"] = _sinavDetayService.GetAllDetails() ;
            ViewData["Derslikler"] = _derslikService.GetList();
            var result = _sinavDetayService.GetAllDetails();
            if (result.Success)
            {
                return View(result);
            }
            return View("Error", result.Message);
        }

        // Yeni sınav detayı ekleme işlemi - POST
        [HttpPost]
        public IActionResult Add(SinavDetay sinavDetay)
        {
            var result = _sinavDetayService.Add(sinavDetay);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result.Message);
        }


        // Sınav detayını düzenleme işlemi - POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Update(SinavDetay sinavDetay)
        {
            var _sinavDetay = _sinavDetayService.GetById(sinavDetay.Id);
            if (!_sinavDetay.Success)
            {
                return BadRequest(_sinavDetay);
            }

            var result = _sinavDetayService.Update(sinavDetay);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        // Sınav detayını silme işlemi - GET
        [HttpDelete]
        [Route("/sinavdetay/delete/{id}")]
        public IActionResult Delete(int id)
        {
            var result = _sinavDetayService.GetById(id);
            if (result.Success)
            {
                return Ok(result.Data);
            }
            return BadRequest(result.Message);
        }
    }
}
