using Business.Concrete;
using DataAccess.Concrete;
using Entity.Concrete;
using Microsoft.AspNetCore.Mvc;

namespace Frontend1.Controllers
{
    public class DerslikController : Controller
    {
        DerslikManager _derslikManager = new DerslikManager(new DerslikDal());
        [HttpGet]
        [Route("Derslik/index")]
        [Route("Derslik/Yonetim")]
        [Route("Derslik/DerslikYonetim")]
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        [Route("/Derslik/Add")]
        public IActionResult Add([FromBody] Derslik derslik)
        {
            _derslikManager.Add(derslik);
            return Ok();
        }



        [HttpPost]
        public IActionResult Delete([FromBody] Derslik derslik)
        {
            try
            {
                // Bolum nesnesini sil
                _derslikManager.Delete(derslik);
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
            var derslik = _derslikManager.GetById(id);
            if (derslik != null)
            {
                return Ok(derslik); // Derslik nesnesini JSON olarak döner
            }
            return NotFound();
        }
        [HttpPost]
        public IActionResult Update([FromBody] Derslik derslik)
        {
            if (derslik == null)
            {
                return BadRequest("Ders verisi eksik veya geçersiz.");
            }

            try
            {
                _derslikManager.Update(derslik);
                return Ok(); // Başarılı yanıt
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Hata oluştu: " + ex.Message });
            }
        }
    }
}
