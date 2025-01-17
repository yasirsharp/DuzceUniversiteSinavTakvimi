using Business.Abstract;
using Entity.Concrete;
using Microsoft.AspNetCore.Mvc;

namespace Frontend1.Controllers
{
    public class BolumDersAkademikPersonelController : Controller
    {
        IBolumService _bolumService;
        IDersService _dersService;
        IAkademikPersonelService _akademikPersonelService;
        IDBAPService _dbapService;

        public BolumDersAkademikPersonelController(IDBAPService dBAPService, IBolumService bolumService, IDersService dersService, IAkademikPersonelService akademikPersonelService)
        {
            _bolumService = bolumService;
            _dersService = dersService;
            _akademikPersonelService = akademikPersonelService;
            _dbapService = dBAPService;
        }


        [HttpGet]
        [Route("BolumDersAkademikPersonel/index")]
        public IActionResult Index()
        {
            ViewData["Bolumler"] = _bolumService.GetList();
            ViewData["Dersler"] = _dersService.GetList();
            ViewData["APler"] = _akademikPersonelService.GetList();
            ViewData["BDAP"] = _dbapService.GetAllDetails();

            return View();
        }

        [HttpPost]
        [Route("BolumDersAkademikPersonel/Add")]
        public IActionResult Add([FromBody] DersBolumAkademikPersonel model)
        {
            var result = _dbapService.Add(model);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

    }
}
