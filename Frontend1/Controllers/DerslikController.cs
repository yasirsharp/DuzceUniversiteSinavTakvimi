using Business.Abstract;
using Business.Concrete;
using DataAccess.Concrete;
using Entity.Concrete;
using Microsoft.AspNetCore.Mvc;

namespace Frontend1.Controllers
{
    public class DerslikController : Controller
    {
        IDerslikService _derslikService;

        public DerslikController(IDerslikService derslikService)
        {
            _derslikService = derslikService;
        }

        [HttpGet]
        [Route("Derslik/index")]
        [Route("Derslik/Yonetim")]
        [Route("Derslik/DerslikYonetim")]
        public IActionResult Index()
        {
            return View(_derslikService.GetList().Data);
        }
        [HttpPost]
        [Route("/Derslik/Add")]
        public IActionResult Add([FromBody] Derslik derslik)
        {
            var result = _derslikService.Add(derslik);
            
            if (!result.Success) return BadRequest();

            return Ok(result.Message);
        }

        [HttpPost]
        public IActionResult Delete([FromBody] Derslik derslik)
        {
            var result = _derslikService.Delete(derslik);

            if (!result.Success) return BadRequest();

            return Ok(result.Message);
        }

        [HttpPost]
        public IActionResult Update([FromBody] Derslik derslik)
        {
            var result = _derslikService.Update(derslik);

            if (!result.Success) return BadRequest();

            return Ok(result.Message);
        }
    }
}
