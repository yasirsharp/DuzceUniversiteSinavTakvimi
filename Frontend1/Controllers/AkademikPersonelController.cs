using Business.Abstract;
using Business.Concrete;
using DataAccess.Concrete;
using Entity.Concrete;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Frontend1.Controllers
{
    public class AkademikPersonelController : Controller
    {
        IAkademikPersonelService _akademikPersonelService;

        public AkademikPersonelController(IAkademikPersonelService akademikPersonelService)
        {
            _akademikPersonelService = akademikPersonelService;
        }

        [HttpGet]
        [Route("AkademikPersonel/index")]
        [Route("AkademikPersonel/Yonetim")]
        [Route("AkademikPersonel/AkademikPersonelYonetim")]
        public IActionResult Index()
        {
            return View(_akademikPersonelService.GetList().Data);
        }
        [HttpPost]
        [Route("/AkademikPersonel/Add")]
        public IActionResult Add([FromBody] AkademikPersonel akademikPersonel)
        {
            var result = _akademikPersonelService.Add(akademikPersonel);
            if (result.Success) return Ok();

            return BadRequest(new { message = result.Message });
        }



        [HttpPost]
        public IActionResult Delete([FromBody] AkademikPersonel akademikPersonel)
        {
            var result = _akademikPersonelService.Delete(akademikPersonel);
            if (result.Success) return Ok();

            return BadRequest(new { message = result.Message });
        }
        [HttpPost]
        public IActionResult Update([FromBody] AkademikPersonel akademikPersonel)
        {
            var _akademikPersonel = _akademikPersonelService.GetById(akademikPersonel.Id);
            if (_akademikPersonel.Data == null)
            {
                return BadRequest(new { message = _akademikPersonel.Message });
            }

            var result = _akademikPersonelService.Update(akademikPersonel);
            if (!result.Success)
            {
                return BadRequest(new { message = result.Message });
            }
            return Ok(); // Başarılı yanıt
        }
    }
}
