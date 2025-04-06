using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService _customerService;
        public CustomerController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        [HttpGet("GetByID")]
        public async Task<IActionResult> ApplyFixedDiscount(int id)
        {


            var result = await _customerService.GetCustomerById(id);
            return Ok(result);
        }

        [HttpPost("AddCustomerPoint")]
        public async Task<IActionResult> AddCustomerPoint(int id, int point)
        {
            if (point < 0)
            {
                return BadRequest("Point cannot be negative.");
            }
            if (id <= 0)
            {
                return BadRequest("Invalid customer ID.");
            }
            var result = await _customerService.AddCustomerPoint(id, point);

            return Ok(result);
        }
    }

}
