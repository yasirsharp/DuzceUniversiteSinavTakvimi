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
            
            ViewData["SinavDetay"] = sinavDetayResult;
            
            if (sinavDetayResult.Success)
            {
                return View(sinavDetayResult);
            }
            return View("Error", sinavDetayResult.Message);
        }

        [Route("/sinavdetay/{bolumId}")]
        public IActionResult Index(int bolumId)
        {
            if (bolumId <= 0) return Json("Düzgün Veri girerseniz çok sevinirim. isterseniz düzgün veri nasıl girdirilir gösterebilirim. Hem de uygulamalı.");

            ViewData["DersBolumAkademikPersonel"] = _dBAPService.GetByBolumId(bolumId);
            ViewData["DersBolumAkademikPersonelDetails"] = _dBAPService.GetDetailsByBolumId(bolumId);
            ViewData["Derslikler"] = _derslikService.GetList();
            ViewData["AkademikPersoneller"] = _akademikPersonelService.GetList();

            var sinavDetayResult = _sinavDetayService.GetByBolumId(bolumId);
            ViewData["SinavDetay"] = sinavDetayResult;

            return View();
        }

        // Yeni sınav detayı ekleme işlemi - POST
        [HttpPost]
        [Route("/SinavDetay/Add")]
        public IActionResult Add([FromBody] SinavKayitDTO model)
        {
            try
            {
                var result = _sinavDetayService.Add(model);

                if (!result.Success)
                    return Json(new { success = false, message = result.Message });
                


                return Json(new { success = true, message = result.Message });
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
                // Gelen modelin geçerliliğini kontrol et
                if (model == null || model.Id <= 0)
                {
                    return Json(new { success = false, message = "Geçersiz sınav verisi." });
                }

                // Önce mevcut sınav-derslik eşleştirmelerini sil
                var mevcutDerslikler = _sinavDerslikService.GetBySinavDetayId(model.Id);
                if (mevcutDerslikler.Success)
                {
                    foreach (var derslik in mevcutDerslikler.Data)
                    {
                        _sinavDerslikService.Delete(derslik);
                    }
                }

                // Sınav detayını güncelle
                var sinavDetay = new SinavDetay
                {
                    Id = model.Id,
                    DerBolumAkademikPersonelId = model.DbapId,
                    SinavTarihi = model.SinavTarihi,
                    SinavBaslangicSaati = model.SinavBaslangicSaati,
                    SinavBitisSaati = model.SinavBitisSaati
                };

                var updateResult = _sinavDetayService.Update(sinavDetay);
                if (!updateResult.Success)
                {
                    return Json(new { success = false, message = updateResult.Message });
                }

                // Yeni derslik-gözetmen eşleştirmelerini ekle
                foreach (var derslik in model.Derslikler)
                {
                    var sinavDerslik = new SinavDerslik
                    {
                        SinavDetayId = model.Id,
                        DerslikId = derslik.DerslikId,
                        GozetmenId = derslik.GozetmenId ?? 0
                    };

                    var derslikResult = _sinavDerslikService.Add(sinavDerslik);
                    if (!derslikResult.Success)
                    {
                        return Json(new { success = false, message = derslikResult.Message });
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
                /*// 1. Önce SinavDerslik eşleştirmelerini sil
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
                }*/

                return Json(new { success = false, message = "Yapım Aşamasında (~yasirsharp)." });
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
