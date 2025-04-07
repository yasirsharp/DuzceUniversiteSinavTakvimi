using Entity.Concrete;
using Frontend1.Services;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;

namespace Frontend1.Controllers
{
    public class GorevliSinavYonetimController : Controller
    {
        private readonly HttpService _httpService;

        public GorevliSinavYonetimController(HttpService httpService)
        {
            _httpService = httpService;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var bolumIds = GetBolumIdsFromToken();
                ViewData["BolumIds"] = bolumIds;

                var uniqueBolumIds = bolumIds.Distinct().ToList();
                List<Bolum> bolums = new List<Bolum>();

                foreach (var item in uniqueBolumIds)
                {
                    var result = await _httpService.GetAsync<Bolum>($"api/bolum/{item}");
                    if (result != null) bolums.Add(result);
                }

                return View(bolums.OrderBy(b => b.Ad).ToList());
            }
            catch (Exception ex)
            {
                return View(new List<Bolum>());
            }
        }

        private List<int> GetBolumIdsFromToken()
        {
            var token = HttpContext.Request.Cookies["AuthToken"];
            if (string.IsNullOrEmpty(token))
                return new List<int>();

            try
            {
                var handler = new JwtSecurityTokenHandler();
                var jwtToken = handler.ReadJwtToken(token);
                var claims = jwtToken.Claims;

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
        public async Task<JsonResult> GetSinavlarByBolum(int bolumId)
        {
            try
            {
                var sinavlar = await _httpService.GetAsync<List<SinavDetay>>($"api/sinavdetay/bolum/{bolumId}");
                return Json(sinavlar);
            }
            catch (Exception ex)
            {
                return Json(new { error = ex.Message });
            }
        }
    }
}
