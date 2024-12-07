using Business.Concrete;
using DataAccess.Concrete;
using Entity.Concrete;
using Microsoft.AspNetCore.Mvc;

namespace Frontend1.Controllers
{
    public class AkademikPersonelController : Controller
    {
        AkademikPersonelManager _akademikPersonelManager = new AkademikPersonelManager(new AkademikPersonelDal()); 
        [HttpGet]
        [Route("AkademikPersonel/index")]
        [Route("AkademikPersonel/Yonetim")]
        [Route("AkademikPersonel/AkademikPersonelYonetim")]
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        [Route("/AkademikPersonel/Add")]
        public IActionResult Add([FromBody] AkademikPersonel akademikPersonel)
        {
            _akademikPersonelManager.Add(akademikPersonel);
            return Ok();
        }



        [HttpPost]
        public IActionResult Delete([FromBody] AkademikPersonel akademikPersonel)
        {
            try
            {
                // Bolum nesnesini sil
                _akademikPersonelManager.Delete(akademikPersonel);
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
            var akademikPersonel = _akademikPersonelManager.GetById(id);
            if (akademikPersonel != null)
            {
                return Ok(akademikPersonel); // Bölüm nesnesini JSON olarak döner
            }
            return NotFound();
        }
        [HttpPost]
        public IActionResult Update([FromBody] AkademikPersonel akademikPersonel)
        {
            if (akademikPersonel == null)
            {
                return BadRequest("AkademikPersonel verisi eksik veya geçersiz.");
            }

            try
            {
                _akademikPersonelManager.Update(akademikPersonel);
                return Ok(); // Başarılı yanıt
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Hata oluştu: " + ex.Message });
            }
        }
    }
}
