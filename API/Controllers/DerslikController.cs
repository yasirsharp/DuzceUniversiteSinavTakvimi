using Business.Abstract;
using Entity.Concrete;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DerslikController : ControllerBase
    {
        private readonly IDerslikService _derslikService;

        public DerslikController(IDerslikService derslikService)
        {
            _derslikService = derslikService;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var result = _derslikService.GetList();
            if (result.Success)
            {
                return Ok(result.Data);
            }
            return BadRequest(result.Message);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var result = _derslikService.GetById(id);
            if (result.Success)
            {
                return Ok(result.Data);
            }
            return BadRequest(result.Message);
        }

        [HttpPost]
        public IActionResult Add([FromBody] Derslik derslik)
        {
            var result = _derslikService.Add(derslik);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result.Message);
        }

        [HttpPut]
        public IActionResult Update([FromBody] Derslik derslik)
        {
            var result = _derslikService.Update(derslik);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result.Message);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var derslik = _derslikService.GetById(id).Data;
            if (derslik == null)
            {
                return NotFound("Derslik bulunamadı");
            }

            var result = _derslikService.Delete(derslik);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result.Message);
        }
    }
} 