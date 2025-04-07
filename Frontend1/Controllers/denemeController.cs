using Entity.Concrete;
using Frontend1.Services;
using Microsoft.AspNetCore.Mvc;

namespace Frontend1.Controllers
{
    public class denemeController : Controller
    {
        private readonly HttpService _httpService;

        public denemeController(HttpService httpService)
        {
            _httpService = httpService;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var sinavDetaylar = await _httpService.GetAsync<List<SinavDetay>>("api/sinavdetay");
                ViewData["SinavDetaylar"] = sinavDetaylar;
                return View();
            }
            catch (Exception ex)
            {
                return View();
            }
        }
    }
}
