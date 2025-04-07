using Microsoft.AspNetCore.Mvc;
using Business.Abstract;
using Entity.Concrete;
using Core.Utilities.Results;
using Entity.DTOs;
using System.Linq;
using Frontend1.Services;

namespace Frontend1.Controllers
{
    public class SinavDetayController : Controller
    {
        private readonly HttpService _httpService;

        public SinavDetayController(HttpService httpService)
        {
            _httpService = httpService;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var dbap = await _httpService.GetAsync<List<DersBolumAkademikPersonel>>("api/bolumdersakademikpersonel");
                var dbapDetail = await _httpService.GetAsync<List<DersBolumAkademikPersonel>>("api/bolumdersakademikpersonel/details");
                var derslikler = await _httpService.GetAsync<List<Derslik>>("api/derslik");
                var akademikPersoneller = await _httpService.GetAsync<List<AkademikPersonel>>("api/akademikpersonel");
                var sinavDetayResult = await _httpService.GetAsync<List<SinavDetay>>("api/sinavdetay/details");

                ViewData["DBAP"] = dbap;
                ViewData["DBAPDetail"] = dbapDetail;
                ViewData["Derslikler"] = derslikler;
                ViewData["AkademikPersoneller"] = akademikPersoneller;
                ViewData["SinavDetay"] = sinavDetayResult;

                return View();
            }
            catch (Exception ex)
            {
                return View("Error", ex.Message);
            }
        }

        [Route("/sinavdetay/{bolumId}")]
        public async Task<IActionResult> Index(int bolumId)
        {
            if (bolumId <= 0) return Json("Geçersiz bölüm ID'si.");

            try
            {
                var dersBolumAkademikPersonel = await _httpService.GetAsync<List<DersBolumAkademikPersonel>>($"api/bolumdersakademikpersonel/bolum/{bolumId}");
                var dersBolumAkademikPersonelDetails = await _httpService.GetAsync<List<DersBolumAkademikPersonel>>($"api/bolumdersakademikpersonel/bolum/{bolumId}/details");
                var derslikler = await _httpService.GetAsync<List<Derslik>>("api/derslik");
                var akademikPersoneller = await _httpService.GetAsync<List<AkademikPersonel>>("api/akademikpersonel");
                var sinavDetayResult = await _httpService.GetAsync<List<SinavDetay>>($"api/sinavdetay/bolum/{bolumId}");

                ViewData["DersBolumAkademikPersonel"] = dersBolumAkademikPersonel;
                ViewData["DersBolumAkademikPersonelDetails"] = dersBolumAkademikPersonelDetails;
                ViewData["Derslikler"] = derslikler;
                ViewData["AkademikPersoneller"] = akademikPersoneller;
                ViewData["BolumId"] = bolumId;
                ViewData["SinavDetay"] = sinavDetayResult;

                return View();
            }
            catch (Exception ex)
            {
                return View("Error", ex.Message);
            }
        }

        [Route("/report/bolum/{bolumId}")]
        public async Task<IActionResult> Report(int bolumId)
        {
            if (bolumId <= 0) return Json("Geçersiz bölüm ID'si.");

            try
            {
                var dersBolumAkademikPersonel = await _httpService.GetAsync<List<DersBolumAkademikPersonel>>($"api/bolumdersakademikpersonel/bolum/{bolumId}");
                var dersBolumAkademikPersonelDetails = await _httpService.GetAsync<List<DersBolumAkademikPersonel>>($"api/bolumdersakademikpersonel/bolum/{bolumId}/details");
                var derslikler = await _httpService.GetAsync<List<Derslik>>("api/derslik");
                var akademikPersoneller = await _httpService.GetAsync<List<AkademikPersonel>>("api/akademikpersonel");
                var sinavDetayResult = await _httpService.GetAsync<List<SinavDetay>>($"api/sinavdetay/bolum/{bolumId}");

                ViewData["DersBolumAkademikPersonel"] = dersBolumAkademikPersonel;
                ViewData["DersBolumAkademikPersonelDetails"] = dersBolumAkademikPersonelDetails;
                ViewData["Derslikler"] = derslikler;
                ViewData["AkademikPersoneller"] = akademikPersoneller;
                ViewData["SinavDetay"] = sinavDetayResult;

                return View();
            }
            catch (Exception ex)
            {
                return View("Error", ex.Message);
            }
        }

        [HttpPost]
        [Route("/SinavDetay/Add")]
        public async Task<IActionResult> Add([FromBody] SinavKayitDTO model)
        {
            try
            {
                var result = await _httpService.PostAsync<object>("api/sinavdetay", model);
                return Json(new { success = true, message = "Sınav başarıyla eklendi." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Update([FromBody] SinavGuncelleDTO model)
        {
            try
            {
                if (model == null || model.Id <= 0)
                {
                    return Json(new { success = false, message = "Geçersiz sınav verisi." });
                }

                var result = await _httpService.PutAsync<object>("api/sinavdetay", model);
                return Json(new { success = true, message = "Sınav başarıyla güncellendi." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Delete([FromBody] int id)
        {
            try
            {
                var result = await _httpService.DeleteAsync<object>($"api/sinavdetay/{id}");
                return Json(new { success = true, message = "Sınav başarıyla silindi." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> GetSinavlarByDerslik([FromBody] int? derslikId)
        {
            try
            {
                var sinavlar = await _httpService.GetAsync<List<SinavDetay>>("api/sinavdetay/details");
                if (!derslikId.HasValue)
                {
                    return Json(new { success = true, data = sinavlar });
                }

                var result = await _httpService.GetAsync<List<SinavDetay>>($"api/sinavdetay/derslik/{derslikId}");
                return Json(new { success = true, data = result });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpGet]
        [Route("SinavDetay/GetSinavDerslikler/{sinavDetayId}")]
        public async Task<IActionResult> GetSinavDerslikler(int sinavDetayId)
        {
            try
            {
                var result = await _httpService.GetAsync<List<SinavDerslik>>($"api/sinavdetay/{sinavDetayId}/derslikler");
                return Json(new { success = true, data = result });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpGet]
        [Route("SinavDetay/GetSinavDersliklerDetay/{sinavDetayId}")]
        public async Task<IActionResult> GetSinavDersliklerDetay(int sinavDetayId)
        {
            try
            {
                var result = await _httpService.GetAsync<List<object>>($"api/sinavdetay/{sinavDetayId}/derslikler/detay");
                return Json(new { success = true, data = result });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
    }
}
