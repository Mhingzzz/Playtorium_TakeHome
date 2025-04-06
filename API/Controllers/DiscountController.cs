using Application.DTOs.Requests;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DiscountController : ControllerBase
    {
        private readonly IDiscountCampaignService _discountCampaignService;

        public DiscountController(IDiscountCampaignService discountCampaignService)
        {
            _discountCampaignService = discountCampaignService;
        }

        [HttpPost("apply-fixed-discount")]
        public IActionResult ApplyFixedDiscount([FromBody] DiscountRequest request)
        {
            //if (request == null || request.TotalAmount < 0 || request.DiscountAmount < 0)
            //{
            //    return BadRequest("Invalid request data.");
            //}

            var result = _discountCampaignService.ApplyFixedDiscount(request.TotalAmount, request.DiscountAmount);
            return Ok(result);
        }

        [HttpGet("GetAvaibleDiscount")]
        public async Task<IActionResult> GetAvaibleDiscount()
        {
            var result = await _discountCampaignService.GetActiveCampaign();
            return Ok(result);
        }

    }

}
