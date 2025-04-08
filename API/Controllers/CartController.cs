using Application.DTOs.Responses;
using Application.Interfaces;
using Domain;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;
        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }

        [HttpGet("InitCart")]
        public async Task<IActionResult> InitCart(int customerId)
        {
            var response = new BaseHttpResponse<Cart?>();
            if (customerId <= 0)
            {
                return BadRequest("Invalid customer ID.");
            }

            var result = await _cartService.InitCart(customerId);

            response.SetSuccess(result, "InitCart", "201");



            return Ok(response);
        }

        [HttpGet("GetCartByCustomerId")]
        public async Task<IActionResult> GetCartByCustomerId(int customerId)
        {
            var response = new BaseHttpResponse<Cart?>();
            if (customerId <= 0)
            {
                return BadRequest("Invalid customer ID.");
            }

            var result = await _cartService.GetCartByCustomerIdAsync(customerId);

            response.SetSuccess(result, "GetCartByCustomerId", "200");

            return Ok(response);
        }

    }

}
