using Entity.Concrete;
using Frontend1.Services;
using Microsoft.AspNetCore.Mvc;

namespace Frontend1.Controllers
{
    public class AkademikPersonelController : Controller
    {
        private readonly HttpService _httpService;

        public AkademikPersonelController(HttpService httpService)
        {
            _httpService = httpService;
        }

        [HttpGet]
        [Route("AkademikPersonel/index")]
        [Route("AkademikPersonel/Yonetim")]
        [Route("AkademikPersonel/AkademikPersonelYonetim")]
        public async Task<IActionResult> Index()
        {
            try
            {
                var akademikPersoneller = await _httpService.GetAsync<List<AkademikPersonel>>("api/akademikpersonel");
                return View(akademikPersoneller);
            }
            catch (Exception ex)
            {
                return View(new List<AkademikPersonel>());
            }
        }

        [HttpPost]
        [Route("/AkademikPersonel/Add")]
        public async Task<IActionResult> Add([FromBody] AkademikPersonel akademikPersonel)
        {
            try
            {
                var result = await _httpService.PostAsync<object>("api/akademikpersonel", akademikPersonel);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost]
        [Route("/AkademikPersonel/Delete")]
        public async Task<IActionResult> Delete([FromBody] AkademikPersonel akademikPersonel)
        {
            try
            {
                var result = await _httpService.DeleteAsync<object>($"api/akademikpersonel/{akademikPersonel.Id}");
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Update([FromBody] AkademikPersonel akademikPersonel)
        {
            try
            {
                var result = await _httpService.PutAsync<object>("api/akademikpersonel", akademikPersonel);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
