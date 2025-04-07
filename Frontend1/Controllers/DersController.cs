using Entity.Concrete;
using Frontend1.Services;
using Microsoft.AspNetCore.Mvc;

namespace Frontend1.Controllers
{
    public class DersController : Controller
    {
        private readonly HttpService _httpService;

        public DersController(HttpService httpService)
        {
            _httpService = httpService;
        }

        [HttpGet]
        [Route("Ders/index")]
        [Route("Ders/Yonetim")]
        [Route("Ders/DersYonetim")]
        public async Task<IActionResult> Index()
        {
            try
            {
                var dersler = await _httpService.GetAsync<List<Ders>>("api/ders");
                return View(dersler);
            }
            catch (Exception ex)
            {
                return View(new List<Ders>());
            }
        }

        [HttpPost]
        [Route("/Ders/Add")]
        public async Task<IActionResult> Add([FromBody] Ders ders)
        {
            try
            {
                var result = await _httpService.PostAsync<object>("api/ders", ders);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Delete([FromBody] Ders ders)
        {
            try
            {
                var result = await _httpService.DeleteAsync<object>($"api/ders/{ders.Id}");
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var ders = await _httpService.GetAsync<Ders>($"api/ders/{id}");
                if (ders != null)
                {
                    return Ok(ders);
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Update([FromBody] Ders ders)
        {
            try
            {
                if (ders == null)
                {
                    return BadRequest(new { message = "Ders verisi eksik veya geçersiz." });
                }

                var result = await _httpService.PutAsync<object>("api/ders", ders);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
