using Core.Entities.Concrete;
using Frontend1.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Frontend1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "super.admin")]
    public class OperationClaimsController : ControllerBase
    {
        private readonly HttpService _httpService;

        public OperationClaimsController(HttpService httpService)
        {
            _httpService = httpService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var result = await _httpService.GetAsync<List<OperationClaim>>("api/operationclaim");
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var result = await _httpService.GetAsync<OperationClaim>($"api/operationclaim/{id}");
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] OperationClaim operationClaim)
        {
            try
            {
                var result = await _httpService.PostAsync<object>("api/operationclaim", operationClaim);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var result = await _httpService.DeleteAsync<object>($"api/operationclaim/{id}");
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
} 