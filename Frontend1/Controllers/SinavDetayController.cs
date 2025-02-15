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

        [Route("/sinavdetay/{bolumId}")]
        // Tüm sınav detaylarını listeleyen action
        public IActionResult Index(int bolumId)
        {
            if (bolumId <= 0) return BadRequest("Git düzgün veri gir KÖPEK.");
            ViewData["DBAP"] = _dBAPService.GetAll();
            ViewData["DBAPDetail"] = _dBAPService.GetAllDetails();
            ViewData["Derslikler"] = _derslikService.GetList();
            ViewData["AkademikPersoneller"] = _akademikPersonelService.GetList();
            ViewData["BolumID"] = bolumId;
            
            var sinavDetayResult = _sinavDetayService.GetAllDetails();
            
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
                    SinavTarihi = model.SinavTarihi,
                    SinavSaati = model.SinavSaati
                };

                var sinavDetayResult = _sinavDetayService.Add(sinavDetay);
                if (!sinavDetayResult.Success)
                {
                    return Json(new { success = false, message = sinavDetayResult.Message });
                }
                List<DerslikGozetmenDTO> eklenenDerslikler = new List<DerslikGozetmenDTO>();
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
                        if (eklenenDerslikler.Count>0)
                        {
                            _sinavDerslikService.Delete(sinavDerslik);
                        }
                        return Json(new { success = false, message = "Derslik ataması yapılırken bir hata oluştu: " + sinavDerslikResult.Message });
                    }
                    eklenenDerslikler.Add(derslik);
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
        public IActionResult Update([FromBody] SinavGuncelleDTO model)
        {
            try
            {
                Console.WriteLine($"Gelen model: {System.Text.Json.JsonSerializer.Serialize(model)}");

                // 1. SinavDetay'ı güncelle
                var sinavDetay = new SinavDetay
                {
                    Id = model.Id,
                    DBAPId = model.DbapId,
                    SinavTarihi = model.SinavTarihi.Date,
                    SinavSaati = TimeOnly.Parse(model.SinavSaati)
                };

                Console.WriteLine($"Güncellenecek sınav: {System.Text.Json.JsonSerializer.Serialize(sinavDetay)}");

                var updateResult = _sinavDetayService.Update(sinavDetay);
                if (!updateResult.Success)
                {
                    return Json(new { success = false, message = updateResult.Message });
                }

                // 2. Önce eski derslik eşleştirmelerini sil
                var eskiDerslikler = _sinavDerslikService.GetBySinavDetayId(model.Id);
                if (eskiDerslikler.Success && eskiDerslikler.Data != null)
                {
                    foreach (var eskiDerslik in eskiDerslikler.Data)
                    {
                        _sinavDerslikService.Delete(eskiDerslik);
                    }
                }

                // 3. Yeni derslik eşleştirmelerini ekle
                if (model.Derslikler != null)
                {
                    foreach (var derslik in model.Derslikler)
                    {
                        var sinavDerslik = new SinavDerslik
                        {
                            SinavDetayId = model.Id,
                            DerslikId = derslik.DerslikId,
                            GozetmenId = derslik.GozetmenId ?? 0
                        };

                        var addResult = _sinavDerslikService.Add(sinavDerslik);
                        if (!addResult.Success)
                        {
                            return Json(new { success = false, message = $"Derslik {derslik.DerslikId} eklenirken hata: {addResult.Message}" });
                        }
                    }
                }

                return Json(new { success = true, message = "Sınav başarıyla güncellendi." });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Güncelleme hatası: {ex}");
                return Json(new { success = false, message = "Güncelleme sırasında bir hata oluştu: " + ex.Message });
            }
        }

        // Sınav detayını silme işlemi
        [HttpPost]
        public IActionResult Delete([FromBody] int id)
        {
            try
            {
                // 1. Önce SinavDerslik eşleştirmelerini sil
                var sinavDerslikler = _sinavDerslikService.GetBySinavDetayId(id);
                foreach (var sinavDerslik in sinavDerslikler.Data)
                {
                    _sinavDerslikService.Delete(sinavDerslik);
                }

                // 2. Sonra SinavDetay'ı sil
                var sinavDetay = new SinavDetay { Id = id };
                var result = _sinavDetayService.Delete(sinavDetay);

                if (!result.Success)
                {
                    return Json(new { success = false, message = result.Message });
                }

                return Json(new { success = true, message = "Sınav başarıyla silindi." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Silme işlemi sırasında bir hata oluştu: " + ex.Message });
            }
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

        // Sınav detayına ait derslikleri getiren action
        [HttpGet]
        [Route("SinavDetay/GetSinavDerslikler/{sinavDetayId}")]
        public IActionResult GetSinavDerslikler(int sinavDetayId)
        {
            try
            {
                var result = _sinavDerslikService.GetBySinavDetayId(sinavDetayId);
                if (!result.Success)
                {
                    return Json(new { success = false, message = result.Message });
                }

                return Json(new { success = true, data = result.Data });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Derslik bilgileri getirilirken bir hata oluştu: " + ex.Message });
            }
        }
    }
}
