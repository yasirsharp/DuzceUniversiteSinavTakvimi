using Business.Abstract;
using Core.Entities.Concrete;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "super.admin")]
    public class ManagementController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IOperationClaimService _operationClaimService;
        private readonly IUserOperationClaimService _userOperationClaimService;

        public ManagementController(
            IUserService userService,
            IOperationClaimService operationClaimService,
            IUserOperationClaimService userOperationClaimService)
        {
            _userService = userService;
            _operationClaimService = operationClaimService;
            _userOperationClaimService = userOperationClaimService;
        }

        [HttpGet("users")]
        public IActionResult GetAllUsers()
        {
            var result = _userService.GetAll();
            if (result.Success)
            {
                return Ok(result.Data);
            }
            return BadRequest(result.Message);
        }

        [HttpGet("users/{id}")]
        public IActionResult GetUserById(int id)
        {
            var result = _userService.GetById(id);
            if (result.Success)
            {
                return Ok(result.Data);
            }
            return BadRequest(result.Message);
        }

        [HttpDelete("users/{id}")]
        public IActionResult DeleteUser(int id)
        {
            var result = _userService.Delete(new User { Id = id });
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result.Message);
        }

        [HttpGet("operationclaims")]
        public IActionResult GetAllOperationClaims()
        {
            var result = _operationClaimService.GetAll();
            if (result.Success)
            {
                return Ok(result.Data);
            }
            return BadRequest(result.Message);
        }

        [HttpGet("operationclaims/{id}")]
        public IActionResult GetOperationClaimById(int id)
        {
            var result = _operationClaimService.GetById(id);
            if (result.Success)
            {
                return Ok(result.Data);
            }
            return BadRequest(result.Message);
        }

        [HttpPost("operationclaims")]
        public IActionResult AddOperationClaim([FromBody] OperationClaim operationClaim)
        {
            var result = _operationClaimService.Add(operationClaim);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result.Message);
        }

        [HttpPut("operationclaims")]
        public IActionResult UpdateOperationClaim([FromBody] OperationClaim operationClaim)
        {
            var result = _operationClaimService.Update(operationClaim);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result.Message);
        }

        [HttpDelete("operationclaims/{id}")]
        public IActionResult DeleteOperationClaim(int id)
        {
            var operationClaim = _operationClaimService.GetById(id).Data;
            if (operationClaim == null)
            {
                return NotFound("Yetki bulunamadı");
            }

            var result = _operationClaimService.Delete(operationClaim);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result.Message);
        }

        [HttpGet("useroperationclaims")]
        public IActionResult GetAllUserOperationClaims()
        {
            var result = _userOperationClaimService.GetAll();
            if (result.Success)
            {
                return Ok(result.Data);
            }
            return BadRequest(result.Message);
        }

        [HttpPost("useroperationclaims")]
        public IActionResult AddUserOperationClaim([FromBody] UserOperationClaim userOperationClaim)
        {
            var result = _userOperationClaimService.Add(userOperationClaim);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result.Message);
        }

        [HttpDelete("useroperationclaims/{id}")]
        public IActionResult DeleteUserOperationClaim(int id)
        {
            var userOperationClaim = _userOperationClaimService.GetById(id).Data;
            if (userOperationClaim == null)
            {
                return NotFound("Kullanıcı yetkisi bulunamadı");
            }

            var result = _userOperationClaimService.Delete(userOperationClaim);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result.Message);
        }
    }
} 