using Business.Concrete;
using DataAccess.Concrete;
using Entity.Concrete;
using Microsoft.AspNetCore.Mvc;

namespace Frontend1.Controllers
{
    public class BolumController : Controller
    {
        BolumManager bolumManager = new BolumManager(new BolumDal());
        [HttpGet]
        [Route("Bolum/index")]
        [Route("Bolum/Yonetim")]
        [Route("Bolum/BolumYonetim")]
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        [Route("/Bolum/Add")]
        public IActionResult Add([FromBody] Bolum bolum)
        {
            bolumManager.Add(bolum);
            return Ok();
        }



        [HttpPost]
        public IActionResult Delete([FromBody] Bolum bolum)
        {
            try
            {
                // Bolum nesnesini sil
                bolumManager.Delete(bolum);
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
            var bolum = bolumManager.GetById(id);
            if (bolum != null)
            {
                return Ok(bolum); // Bölüm nesnesini JSON olarak döner
            }
            return NotFound();
        }
        [HttpPost]
        public IActionResult Update([FromBody] Bolum bolum)
        {
            if (bolum == null)
            {
                return BadRequest("Bölüm verisi eksik veya geçersiz.");
            }

            try
            {
                // Bölüm adı güncellemesi
                bolumManager.Update(bolum);
                return Ok(); // Başarılı yanıt
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Hata oluştu: " + ex.Message });
            }
        }
    }
}
