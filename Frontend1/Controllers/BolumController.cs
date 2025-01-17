using Business.Abstract;
using Business.Concrete;
using DataAccess.Concrete;
using Entity.Concrete;
using Microsoft.AspNetCore.Mvc;

namespace Frontend1.Controllers
{
    public class BolumController : Controller
    {
        IBolumService _bolumService;

        public BolumController(IBolumService bolumService)
        {
            _bolumService = bolumService;
        }

        [HttpGet]
        [Route("Bolum/index")]
        [Route("Bolum/Yonetim")]
        [Route("Bolum/BolumYonetim")]
        public IActionResult Index()
        {
            return View(_bolumService.GetList().Data);
        }
        [HttpPost]
        [Route("/Bolum/Add")]
        public IActionResult Add([FromBody] Bolum bolum)
        {
            _bolumService.Add(bolum);
            return Ok();
        }



        [HttpPost]
        public IActionResult Delete([FromBody] Bolum bolum)
        {
            var result = _bolumService.Delete(bolum);
            if (result.Success)
            {
                return Ok(); // Başarılı yanıt
            }
            return BadRequest(new { message = result.Message });

        }
        [HttpPost]
        public IActionResult Update([FromBody] Bolum bolum)
        {
            var _bolum = _bolumService.GetById(bolum.Id);
            if (_bolum.Data == null)
            {
                return BadRequest(new { message = _bolum.Message });
            }

            var result = _bolumService.Update(bolum);
            if (!result.Success)
            {
                return BadRequest(new { message = result.Message });
            }
            return Ok(); // Başarılı yanıt
        }
    }
}
