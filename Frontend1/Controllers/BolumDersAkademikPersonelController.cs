using Entity.Concrete;
using Frontend1.Services;
using Microsoft.AspNetCore.Mvc;

namespace Frontend1.Controllers
{
    public class BolumDersAkademikPersonelController : Controller
    {
        private readonly HttpService _httpService;

        public BolumDersAkademikPersonelController(HttpService httpService)
        {
            _httpService = httpService;
        }

        [HttpGet]
        [Route("BolumDersAkademikPersonel/index")]
        public async Task<IActionResult> Index()
        {
            try
            {
                var bolumler = await _httpService.GetAsync<List<Bolum>>("api/bolum");
                var dersler = await _httpService.GetAsync<List<Ders>>("api/ders");
                var akademikPersoneller = await _httpService.GetAsync<List<AkademikPersonel>>("api/akademikpersonel");
                var dbap = await _httpService.GetAsync<List<DersBolumAkademikPersonel>>("api/bolumdersakademikpersonel");

                ViewData["Bolumler"] = bolumler;
                ViewData["Dersler"] = dersler;
                ViewData["APler"] = akademikPersoneller;
                ViewData["BDAP"] = dbap;

                return View();
            }
            catch (Exception ex)
            {
                return View();
            }
        }

        [HttpPost]
        [Route("BolumDersAkademikPersonel/Add")]
        public async Task<IActionResult> Add([FromBody] DersBolumAkademikPersonel model)
        {
            try
            {
                var result = await _httpService.PostAsync<object>("api/bolumdersakademikpersonel", model);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost]
        [Route("BolumDersAkademikPersonel/Delete")]
        public async Task<IActionResult> Delete([FromBody] DersBolumAkademikPersonel model)
        {
            try
            {
                var result = await _httpService.DeleteAsync<object>($"api/bolumdersakademikpersonel/{model.Id}");
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet]
        [Route("/BolumDersAkademikPersonel/GetDetail/{id}")]
        public async Task<IActionResult> GetDetail(int id)
        {
            try
            {
                var result = await _httpService.GetAsync<object>($"api/bolumdersakademikpersonel/{id}");
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
