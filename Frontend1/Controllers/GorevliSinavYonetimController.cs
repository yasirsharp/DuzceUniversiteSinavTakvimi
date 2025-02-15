using Business.Abstract;
using Entity.Concrete;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;

namespace Frontend1.Controllers
{
    public class GorevliSinavYonetimController : Controller
    {
        private readonly IBolumService _bolumService;
        private readonly ISinavDetayService _sinavDetayService;

        public GorevliSinavYonetimController(IBolumService bolumService, ISinavDetayService sinavDetayService)
        {
            _bolumService = bolumService;
            _sinavDetayService = sinavDetayService;
        }

        public IActionResult Index()
        {
            var bolumIds = GetBolumIdsFromToken();
            ViewData["BolumIds"] = bolumIds; // Bölüm ID'lerini View'a gönderiyoruz

            List<Bolum> bolums = new List<Bolum>();

            foreach (var item in bolumIds)
            {
                var result = _bolumService.GetById(item);
                if(result.Success) bolums.Add(result.Data);
            }

            return View(bolums);
        }

        private List<int> GetBolumIdsFromToken()
        {
            var token = HttpContext.Request.Cookies["AuthToken"]; // Cookie'den JWT Token'ı al
            if (string.IsNullOrEmpty(token))
                return new List<int>();

            try
            {
                var handler = new JwtSecurityTokenHandler();
                var jwtToken = handler.ReadJwtToken(token); // Token'ı çözümle
                var claims = jwtToken.Claims;

                // "X.BolumId" formatındaki tüm claim'leri seç ve int listesi olarak döndür
                var bolumIds = claims
                    .Where(c => c.Type.EndsWith(".BolumId"))
                    .Select(c => int.TryParse(c.Value, out int id) ? id : (int?)null)
                    .Where(id => id.HasValue)
                    .Select(id => id.Value)
                    .ToList();

                return bolumIds;
            }
            catch
            {
                return new List<int>();
            }
        }

        [HttpGet]
        public JsonResult GetSinavlarByBolum(int bolumId)
        {
            var sinavlar = _sinavDetayService.GetByBolumId(bolumId);
            return Json(sinavlar);
        }
    }
}
