using Application.DTOs.Requests;
using Application.DTOs.Responses;

namespace Application.Interfaces
{
    public interface IAppliedDiscountService
    {
        Task<AppliedDiscountSummaryDTO> AppliedDiscount(AppliedDiscountRequestDTO appliedDiscountDTO);
    }
}