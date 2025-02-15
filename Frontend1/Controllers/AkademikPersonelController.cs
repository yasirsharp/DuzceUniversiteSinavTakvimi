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
        public async Task<IActionResult> Add([FromBody] AkademikPersonel akademikPersonel)
        {
            var result = _akademikPersonelService.Add(akademikPersonel);
            if (result.Success) return Ok(result);

            return BadRequest(new { message = result.Message });
        }



        [HttpPost]
        [Route("/AkademikPersonel/Delete")]
        public IActionResult Delete([FromBody] AkademikPersonel akademikPersonel)
        {
            var result = _akademikPersonelService.Delete(akademikPersonel);
            if (result.Success) 
                return Ok(new { success = true, message = result.Message });

            return BadRequest(new { success = false, message = result.Message });
        }
        [HttpPost]
        public IActionResult Update([FromBody] AkademikPersonel akademikPersonel)
        {
            var result = _akademikPersonelService.Update(akademikPersonel);
            if (!result.Success)
            {
                return BadRequest(new { message = result.Message });
            }
            return Ok();
        }
    }
}
