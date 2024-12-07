using Business.Concrete;
using DataAccess.Concrete;
using Entity.Concrete;
using Microsoft.AspNetCore.Mvc;

namespace Frontend1.Controllers
{
    public class DersController : Controller
    {
        DersManager _dersManager = new DersManager(new DersDal());
        [HttpGet]
        [Route("Ders/index")]
        [Route("Ders/Yonetim")]
        [Route("Ders/DersYonetim")]
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        [Route("/Ders/Add")]
        public IActionResult Add([FromBody] Ders ders)
        {
            _dersManager.Add(ders);
            return Ok();
        }



        [HttpPost]
        public IActionResult Delete([FromBody] Ders ders)
        {
            try
            {
                // Ders nesnesini sil
                _dersManager.Delete(ders);
                return Ok(); // Başarılı yanıt
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Hata oluştu: " + ex.Message });
            }
        }

        [HttpGet]
        public IActionResult GetById(int id)
        {
            var ders = _dersManager.GetById(id);
            if (ders != null)
            {
                return Ok(ders); // Ders nesnesini JSON olarak döner
            }
            return NotFound();
        }
        [HttpPost]
        public IActionResult Update([FromBody] Ders ders)
        {
            if (ders == null)
            {
                return BadRequest("Ders verisi eksik veya geçersiz.");
            }

            try
            {
                _dersManager.Update(ders);
                return Ok(); // Başarılı yanıt
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Hata oluştu: " + ex.Message });
            }
        }
    }
}
