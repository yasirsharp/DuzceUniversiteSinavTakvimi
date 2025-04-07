using Entity.Concrete;
using Frontend1.Services;
using Microsoft.AspNetCore.Mvc;

namespace Frontend1.Controllers
{
    public class DerslikController : Controller
    {
        private readonly HttpService _httpService;

        public DerslikController(HttpService httpService)
        {
            _httpService = httpService;
        }

        [HttpGet]
        [Route("Derslik/index")]
        [Route("Derslik/Yonetim")]
        [Route("Derslik/DerslikYonetim")]
        public async Task<IActionResult> Index()
        {
            try
            {
                var derslikler = await _httpService.GetAsync<List<Derslik>>("api/derslik");
                return View(derslikler);
            }
            catch (Exception ex)
            {
                return View(new List<Derslik>());
            }
        }

        [HttpPost]
        [Route("/Derslik/Add")]
        public async Task<IActionResult> Add([FromBody] Derslik derslik)
        {
            try
            {
                var result = await _httpService.PostAsync<object>("api/derslik", derslik);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Delete([FromBody] Derslik derslik)
        {
            try
            {
                var result = await _httpService.DeleteAsync<object>($"api/derslik/{derslik.Id}");
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Update([FromBody] Derslik derslik)
        {
            try
            {
                var result = await _httpService.PutAsync<object>("api/derslik", derslik);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
