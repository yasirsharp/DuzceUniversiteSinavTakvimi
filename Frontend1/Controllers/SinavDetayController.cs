using Microsoft.AspNetCore.Mvc;
using Business.Abstract;
using Entity.Concrete;
using Core.Utilities.Results;
using Entity.DTOs;
using System.Linq;

namespace Frontend1.Controllers
{

    public class SinavDetayController : Controller
    {
        private readonly ISinavDetayService _sinavDetayService;
        private readonly IDBAPService _dBAPService;
        private readonly IDerslikService _derslikService;
        private readonly ISinavDerslikService _sinavDerslikService;
        private readonly IAkademikPersonelService _akademikPersonelService;


        public SinavDetayController(
            ISinavDetayService sinavDetayService, 
            IDBAPService dBAPService, 
            IDerslikService derslikService,
            ISinavDerslikService sinavDerslikService,
            IAkademikPersonelService akademikPersonelService)
        {
            _sinavDetayService = sinavDetayService;
            _dBAPService = dBAPService;
            _derslikService = derslikService;
            _sinavDerslikService = sinavDerslikService;
            _akademikPersonelService = akademikPersonelService;
        }

        // Tüm sınav detaylarını listeleyen action
        public IActionResult Index()
        {
            ViewData["DBAP"] = _dBAPService.GetAll();
            ViewData["DBAPDetail"] = _dBAPService.GetAllDetails();
            ViewData["Derslikler"] = _derslikService.GetList();
            ViewData["AkademikPersoneller"] = _akademikPersonelService.GetList();
            
            var sinavDetayResult = _sinavDetayService.GetAllDetails();
            // Debug için sınav verilerini kontrol edelim
            Console.WriteLine($"Sınav Detayları: {System.Text.Json.JsonSerializer.Serialize(sinavDetayResult)}");
            
            ViewData["SinavDetay"] = sinavDetayResult;
            
            if (sinavDetayResult.Success)
            {
                return View(sinavDetayResult);
            }
            return View("Error", sinavDetayResult.Message);
        }

        // Yeni sınav detayı ekleme işlemi - POST
        [HttpPost]
        public IActionResult Add([FromBody] SinavKayitDTO model)
        {
            try
            {
                // 1. Önce SinavDetay kaydını yap
                var sinavDetay = new SinavDetay
                {
                    DBAPId = model.DbapId,
                    SinavTarihi = model.SinavTarihi.Date,
                    SinavSaati = TimeOnly.FromDateTime(model.SinavTarihi)
                };

                var sinavDetayResult = _sinavDetayService.Add(sinavDetay);
                if (!sinavDetayResult.Success)
                {
                    return Json(new { success = false, message = sinavDetayResult.Message });
                }

                // 2. Sonra SinavDerslik kayıtlarını yap
                foreach (var derslik in model.Derslikler)
                {
                    var sinavDerslik = new SinavDerslik
                    {
                        SinavDetayId = sinavDetay.Id,
                        DerslikId = derslik.DerslikId,
                        GozetmenId = derslik.GozetmenId ?? 0
                    };

                    var sinavDerslikResult = _sinavDerslikService.Add(sinavDerslik);
                    if (!sinavDerslikResult.Success)
                    {
                        // Hata durumunda önceki kayıtları silmek için
                        _sinavDetayService.Delete(sinavDetay);
                        return Json(new { success = false, message = "Derslik ataması yapılırken bir hata oluştu: " + sinavDerslikResult.Message });
                    }
                }

                return Json(new { success = true, message = "Sınav programı başarıyla kaydedildi." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "İşlem sırasında bir hata oluştu: " + ex.Message });
            }
        }

        // Sınav detayını düzenleme işlemi - POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Update(SinavDetay sinavDetay)
        {
            var _sinavDetay = _sinavDetayService.GetById(sinavDetay.Id);
            if (!_sinavDetay.Success)
            {
                return BadRequest(_sinavDetay);
            }

            var result = _sinavDetayService.Update(sinavDetay);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        // Sınav detayını silme işlemi - GET
        [HttpDelete]
        [Route("/sinavdetay/delete/{id}")]
        public IActionResult Delete(int id)
        {
            var result = _sinavDetayService.GetById(id);
            if (result.Success)
            {
                return Ok(result.Data);
            }
            return BadRequest(result.Message);
        }

        // Dersliğe göre sınavları getiren action
        [HttpPost]
        public IActionResult GetSinavlarByDerslik([FromBody] int? derslikId)
        {
            try 
            {
                var sinavlar = _sinavDetayService.GetAllDetails();
                if (!sinavlar.Success)
                {
                    return Json(new { success = false, message = sinavlar.Message });
                }

                // Eğer derslikId null ise tüm sınavları döndür
                if (!derslikId.HasValue)
                {
                    return Json(new { success = true, data = sinavlar.Data });
                }

                var result = _sinavDerslikService.GetByDerslikId(derslikId.Value);
                return Json(new { success = true, data = result });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Sınavlar getirilirken bir hata oluştu: " + ex.Message });
            }
        }
    }
}
