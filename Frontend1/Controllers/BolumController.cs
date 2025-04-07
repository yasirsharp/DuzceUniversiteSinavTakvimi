using Entity.Concrete;
using Frontend1.Services;
using Microsoft.AspNetCore.Mvc;

namespace Frontend1.Controllers
{
    public class BolumController : Controller
    {
        private readonly HttpService _httpService;

        public BolumController(HttpService httpService)
        {
            _httpService = httpService;
        }

        [HttpGet]
        [Route("Bolum/index")]
        [Route("Bolum/Yonetim")]
        [Route("Bolum/BolumYonetim")]
        public async Task<IActionResult> Index()
        {
            try
            {
                var bolumler = await _httpService.GetAsync<List<Bolum>>("api/bolum");
                return View(bolumler);
            }
            catch (Exception ex)
            {
                // Log the error
                return View(new List<Bolum>());
            }
        }

        [HttpPost]
        [Route("/Bolum/Add")]
        public async Task<IActionResult> Add([FromBody] Bolum bolum)
        {
            try
            {
                var result = await _httpService.PostAsync<object>("api/bolum", bolum);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Delete([FromBody] Bolum bolum)
        {
            try
            {
                var result = await _httpService.DeleteAsync<object>($"api/bolum/{bolum.Id}");
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Update([FromBody] Bolum bolum)
        {
            try
            {
                var result = await _httpService.PutAsync<object>("api/bolum", bolum);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
