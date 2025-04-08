using Application.ContractRepo;
using Application.DTOs.Requests;
using Application.DTOs.Responses;
using Application.Interfaces;
using Domain;
using Domain.Enum;

namespace Application.Service
{
    public class AppliedDiscountService : IAppliedDiscountService
    {

        private readonly IAppliedDiscountRepository _appliedDiscountRepository;
        private readonly IDiscountCampaignRepository _discountCampaignRepository;
        private readonly ICartRepository _cartRepository;
        private readonly ICartItemRepository _cartItemRepository;
        private readonly ICustomerRepository _customerRepository;
        public AppliedDiscountService(IAppliedDiscountRepository appliedDiscountRepository
            , IDiscountCampaignRepository discountCampaignRepository
            , ICartRepository cartRepository
            , ICartItemRepository cartItemRepository
            , ICustomerRepository customerRepository)
        {
            _appliedDiscountRepository = appliedDiscountRepository;
            _discountCampaignRepository = discountCampaignRepository;
            _cartRepository = cartRepository;
            _cartItemRepository = cartItemRepository;
            _customerRepository = customerRepository;

        }
        public async Task<AppliedDiscountSummaryDTO> AppliedDiscount(AppliedDiscountRequestDTO request)
        {
            // Validate the request object before proceeding if in appliedDistountDto have DiscountCategoryType in the same type more than one time then throw exception
            var duplicateCategoryTypes = request.DiscountCategoryRequest
            .GroupBy(d => d.DiscountCategoryType)
            .Where(g => g.Count() > 1)
            .Select(g => g.Key)
            .ToList();

            if (duplicateCategoryTypes.Any())
            {
                throw new InvalidOperationException($"Duplicate DiscountCategoryType(s) found: {string.Join(", ", duplicateCategoryTypes)}");
            }

            var orderedDiscounts = request.DiscountCategoryRequest
            .OrderBy(d =>
                d.DiscountCategoryType == DiscountCategoryType.Coupon ? 1 :
                d.DiscountCategoryType == DiscountCategoryType.OnTop ? 2 :
                d.DiscountCategoryType == DiscountCategoryType.Seasonal ? 3 : 4
            ).ToList();

            try
            {
                var cart = await _cartRepository.GetByIdAsync(request.CartId);
                if (cart == null)
                {
                    throw new Exception("Cart not found");
                }

                var customer = await _customerRepository.GetByIdAsync(cart.CustomerId);
                if (customer == null)
                {
                    throw new Exception("Customer not found");
                }
                // check if this cart is already applied discount or not
                var alreadyApply = await _appliedDiscountRepository.IsAlreadyApplyDiscount(request.CartId);
                if (alreadyApply)
                {
                    throw new Exception("This cart already applied discount.");
                }

                // get item in cart
                var cartItems = await _cartItemRepository.GetCartItemsByCartId(request.CartId);
                decimal? discountAmount = 0;
                decimal? totalPrice = cart.TotalPrice;
                decimal? totalDiscount = 0;

                var discountApplied = new List<AppliedDiscountInfo>();
                foreach (var discountCategory in orderedDiscounts)
                {
                    // Coupon -> OnTop -> Seasonal
                    // Get the discount amount by discountid

                    var discountCampaign = await _discountCampaignRepository.GetByIdAsync(discountCategory.CampaignIds);
                    if (discountCampaign == null)
                    {
                        throw new Exception($"Discount campaign with ID {discountCategory.CampaignIds} not found.");
                    }


                    switch (discountCategory.DiscountCategoryType)
                    {
                        case DiscountCategoryType.Coupon:
                            if (discountCampaign.CampaignType == CampaignType.FixedAmount.ToString())
                            {
                                discountAmount = discountCampaign.DiscountValue;
                            }
                            else if (discountCampaign.CampaignType == CampaignType.Percentage.ToString())
                            {
                                discountAmount = totalPrice * (discountCampaign.DiscountValue / 100);
                            }
                            discountApplied.Add(new AppliedDiscountInfo
                            {
                                Type = discountCampaign.CampaignType,
                                Amount = discountAmount ?? 0
                            });
                            break;
                        case DiscountCategoryType.OnTop:
                            if (discountCampaign.CampaignType == CampaignType.PercentageByItemCategory.ToString())
                            {
                                var category = discountCampaign.ItemCategory;
                                var matchingItems = cartItems.Where(x => x.Item != null && x.Item.Category == category).ToList();
                                discountAmount = matchingItems.Sum(x => x.Item.Price * x.Quantity) * ((discountCampaign.DiscountValue ?? 0) / 100);
                            }
                            else if (discountCampaign.CampaignType == CampaignType.DiscountByPoints.ToString())
                            {
                                decimal maxDiscount = (decimal)(totalPrice * discountCampaign.PointsCap);
                                decimal pointsValue = cart.Customer != null
                                    ? Math.Min((decimal)cart.Customer.Points, maxDiscount) : 0

                                ;
                                
                                discountAmount = pointsValue;
                                customer.Points -= (int)pointsValue;
                                await _customerRepository.UpdateAsync(customer);
                            }
                            discountApplied.Add(new AppliedDiscountInfo
                            {
                                Type = discountCampaign.CampaignType,
                                Amount = discountAmount ?? 0
                            });
                            break;

                        case DiscountCategoryType.Seasonal:
                            if (discountCampaign.CampaignType == CampaignType.SpecialCampaign.ToString())
                            {
                                var x = discountCampaign.EveryXThb;
                                var y = discountCampaign.DiscountYThb;
                                int unit = (int?)(totalPrice / x) ?? 0;
                                discountAmount = unit * y;
                            }
                            discountApplied.Add(new AppliedDiscountInfo
                            {
                                Type = discountCampaign.CampaignType,
                                Amount = discountAmount ?? 0
                            });
                            break;
                    }
                    totalDiscount += discountAmount;
                    totalPrice -= discountAmount;
                    discountAmount = 0;
                    await _appliedDiscountRepository.AddAsync(new AppliedDiscounts
                    {
                        CartId = request.CartId,
                        DiscountAmount = discountAmount,
                        DiscountType = discountCampaign.CampaignType,
                        AppliedAt = DateTime.UtcNow,
                    });
                }
                cart.TotalPrice = totalPrice;
                await _cartRepository.UpdateAsync(cart);

                var result = new AppliedDiscountSummaryDTO
                {
                    Cart = cart,
                    DiscountTotal = totalDiscount ?? 0,
                    DiscountsApplied = discountApplied
                };


                return result;
            }
            catch (Exception ex)
            {
                throw new Exception("Error while adding applied discount", ex);
            }
        }
    }
}