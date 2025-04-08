using Application.ContractRepo;
using Application.DTOs.Requests;
using Application.Interfaces;
using Application.Service;
using Domain;
using Domain.Enum;
using Moq;

namespace Test.Application
{
    public class AppliedDiscountServiceTests
    {
        private readonly Mock<IAppliedDiscountRepository> _appliedDiscountRepositoryMock;
        private readonly Mock<IDiscountCampaignRepository> _discountCampaignRepositoryMock;
        private readonly Mock<ICartRepository> _cartRepositoryMock;
        private readonly Mock<ICartItemRepository> _cartItemRepositoryMock;
        private readonly Mock<ICustomerRepository> _customerRepositoryMock;
        private readonly IAppliedDiscountService _appliedDiscountService;

        public AppliedDiscountServiceTests()
        {
            _appliedDiscountRepositoryMock = new Mock<IAppliedDiscountRepository>();
            _discountCampaignRepositoryMock = new Mock<IDiscountCampaignRepository>();
            _cartRepositoryMock = new Mock<ICartRepository>();
            _cartItemRepositoryMock = new Mock<ICartItemRepository>();
            _customerRepositoryMock = new Mock<ICustomerRepository>();

            _appliedDiscountService = new AppliedDiscountService(
                _appliedDiscountRepositoryMock.Object,
                _discountCampaignRepositoryMock.Object,
                _cartRepositoryMock.Object,
                _cartItemRepositoryMock.Object,
                _customerRepositoryMock.Object
            );
        }

        [Fact]
        public async Task AppliedDiscount_ShouldCalculateFinalTotalCorrectly()
        {
            // Arrange
            var cartId = 1;
            var customerId = 1;
            var customer = new Customers { Id = customerId, Name = "John Doe", Email = "John@gmail.com" };
            var cart = new Cart
            {
                Id = cartId,
                CustomerId = customerId,
                TotalPrice = 2540
            };

            var cartItems = new List<CartItems>
            {
                new CartItems { Item = new Items { Category = "Clothing", Price = 350 }, Quantity = 1 },
                new CartItems { Item = new Items { Category = "Clothing", Price = 700 }, Quantity = 1 },
                new CartItems { Item = new Items { Category = "Accessories", Price = 850 }, Quantity = 1 },
                new CartItems { Item = new Items { Category = "Accessories", Price = 640 }, Quantity = 1 }
            };
            var total1 = cartItems.Sum(x => x.Item.Price);
            var discountRequests = new[]
            {
                new DiscountCategoryRequest { DiscountCategoryType = DiscountCategoryType.Coupon, CampaignIds = 1 },
                new DiscountCategoryRequest { DiscountCategoryType = DiscountCategoryType.OnTop, CampaignIds = 2 },
                new DiscountCategoryRequest { DiscountCategoryType = DiscountCategoryType.Seasonal, CampaignIds = 3 }
            };

            var discountCampaigns = new List<DiscountCampaigns>
            {
                new DiscountCampaigns { Id = 1, CampaignType = CampaignType.Percentage.ToString(), DiscountValue = 10 },
                new DiscountCampaigns { Id = 2, CampaignType = CampaignType.PercentageByItemCategory.ToString(), DiscountValue = 15, Category = "Clothing" },
                new DiscountCampaigns { Id = 3, CampaignType = CampaignType.SpecialCampaign.ToString(), EveryXThb = 300, DiscountYThb = 40 }
            };

            _cartRepositoryMock.Setup(x => x.GetByIdAsync(cartId)).ReturnsAsync(cart);
            _cartItemRepositoryMock.Setup(x => x.GetCartItemsByCartId(cartId)).ReturnsAsync(cartItems);
            _appliedDiscountRepositoryMock.Setup(x => x.IsAlreadyApplyDiscount(cartId)).ReturnsAsync(false);
            _customerRepositoryMock.Setup(x => x.GetByIdAsync(customerId)).ReturnsAsync(customer);
            _discountCampaignRepositoryMock.Setup(x => x.GetByIdAsync(1)).ReturnsAsync(discountCampaigns[0]);
            _discountCampaignRepositoryMock.Setup(x => x.GetByIdAsync(2)).ReturnsAsync(discountCampaigns[1]);
            _discountCampaignRepositoryMock.Setup(x => x.GetByIdAsync(3)).ReturnsAsync(discountCampaigns[2]);

            var request = new AppliedDiscountRequestDTO
            {
                CartId = cartId,
                DiscountCategoryRequest = discountRequests
            };

            // Act
            var result = await _appliedDiscountService.AppliedDiscount(request);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2540m, total1);
            Assert.Equal(1848.5m, result.Cart.TotalPrice); // Final total after discounts
            Assert.Equal(691.5m, result.DiscountTotal); // Total discount applied

            // Verify repository calls
            _cartRepositoryMock.Verify(x => x.GetByIdAsync(cartId), Times.Once);
            _cartItemRepositoryMock.Verify(x => x.GetCartItemsByCartId(cartId), Times.Once);
            _appliedDiscountRepositoryMock.Verify(x => x.IsAlreadyApplyDiscount(cartId), Times.Once);
            _discountCampaignRepositoryMock.Verify(x => x.GetByIdAsync(It.IsAny<int>()), Times.Exactly(3));
            _appliedDiscountRepositoryMock.Verify(x => x.AddAsync(It.IsAny<AppliedDiscounts>()), Times.Exactly(3));
            _cartRepositoryMock.Verify(x => x.UpdateAsync(cart), Times.Once);
        }

        [Fact]
        public async Task AppliedDiscount_FixedAmountDiscount_ShouldCalculateCorrectly()
        {
            // Arrange
            var cartId = 1;
            var customerId = 1;
            var customer = new Customers { Id = customerId, Name = "John Doe", Email = "John@gmail.com" };
            var cart = new Cart
            {
                Id = cartId,
                CustomerId = customerId,
                TotalPrice = 600
            };

            var cartItems = new List<CartItems>
            {
                new CartItems { Item = new Items { Category = "Clothing", Name = "T-Shirt", Price = 350 }, Quantity = 1 },
                new CartItems { Item = new Items { Category = "Accessories", Name = "Hat", Price = 250 }, Quantity = 1 }
            };

            var discountRequests = new[]
            {
                new DiscountCategoryRequest { DiscountCategoryType = DiscountCategoryType.Coupon, CampaignIds = 1 }
            };

            var discountCampaigns = new List<DiscountCampaigns>
            {
                new DiscountCampaigns { Id = 1, CampaignType = CampaignType.FixedAmount.ToString(), DiscountValue = 50, Category = "Coupon" }
            };

            SetupCommonMocks(cartId, customerId, cart, cartItems, customer, discountCampaigns);

            var request = new AppliedDiscountRequestDTO
            {
                CartId = cartId,
                DiscountCategoryRequest = discountRequests
            };

            // Act
            var result = await _appliedDiscountService.AppliedDiscount(request);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(600m, cartItems.Sum(x => x.Item.Price));
            Assert.Equal(550m, result.Cart.TotalPrice); // Final total after fixed discount
            Assert.Equal(50m, result.DiscountTotal); // Total discount applied

            VerifyCommonMocks(cartId, cart);
        }

        [Fact]
        public async Task AppliedDiscount_PercentageDiscount_ShouldCalculateCorrectly()
        {
            // Arrange
            var cartId = 1;
            var customerId = 1;
            var customer = new Customers { Id = customerId, Name = "John Doe", Email = "John@gmail.com" };
            var cart = new Cart
            {
                Id = cartId,
                CustomerId = customerId,
                TotalPrice = 600
            };

            var cartItems = new List<CartItems>
            {
                new CartItems { Item = new Items { Category = "Clothing", Name = "T-Shirt", Price = 350 }, Quantity = 1 },
                new CartItems { Item = new Items { Category = "Accessories", Name = "Hat", Price = 250 }, Quantity = 1 }
            };

            var discountRequests = new[]
            {
                new DiscountCategoryRequest { DiscountCategoryType = DiscountCategoryType.Coupon, CampaignIds = 2 }
            };

            var discountCampaigns = new List<DiscountCampaigns>
            {
                new DiscountCampaigns { Id = 2, CampaignType = CampaignType.Percentage.ToString(), DiscountValue = 10, Category = "Coupon" }
            };

            SetupCommonMocks(cartId, customerId, cart, cartItems, customer, discountCampaigns);

            var request = new AppliedDiscountRequestDTO
            {
                CartId = cartId,
                DiscountCategoryRequest = discountRequests
            };

            // Act
            var result = await _appliedDiscountService.AppliedDiscount(request);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(600m, cartItems.Sum(x => x.Item.Price));
            Assert.Equal(540m, result.Cart.TotalPrice); // Final total after 10% discount
            Assert.Equal(60m, result.DiscountTotal); // Total discount applied

            VerifyCommonMocks(cartId, cart);
        }

        [Fact]
        public async Task AppliedDiscount_CategoryBasedDiscount_ShouldCalculateCorrectly()
        {
            // Arrange
            var cartId = 1;
            var customerId = 1;
            var customer = new Customers { Id = customerId, Name = "John Doe", Email = "John@gmail.com" };
            var cart = new Cart
            {
                Id = cartId,
                CustomerId = customerId,
                TotalPrice = 2540
            };

            var cartItems = new List<CartItems>
            {
                new CartItems { Item = new Items { Category = "Clothing", Name = "T-Shirt", Price = 350 }, Quantity = 1 },
                new CartItems { Item = new Items { Category = "Clothing", Name = "Hoodie", Price = 700 }, Quantity = 1 },
                new CartItems { Item = new Items { Category = "Accessories", Name = "Watch", Price = 850 }, Quantity = 1 },
                new CartItems { Item = new Items { Category = "Accessories", Name = "Bag", Price = 640 }, Quantity = 1 }
            };

            var discountRequests = new[]
            {
                new DiscountCategoryRequest { DiscountCategoryType = DiscountCategoryType.OnTop, CampaignIds = 3 }
            };

            var discountCampaigns = new List<DiscountCampaigns>
            {
                new DiscountCampaigns {
                    Id = 3,
                    CampaignType = CampaignType.PercentageByItemCategory.ToString(),
                    DiscountValue = 15,
                    Category = "OnTop",
                    ItemCategory = "Clothing"
                }
            };

            SetupCommonMocks(cartId, customerId, cart, cartItems, customer, discountCampaigns);

            var request = new AppliedDiscountRequestDTO
            {
                CartId = cartId,
                DiscountCategoryRequest = discountRequests
            };

            // Act
            var result = await _appliedDiscountService.AppliedDiscount(request);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2540m, cartItems.Sum(x => x.Item.Price));
            Assert.Equal(2382.5m, result.Cart.TotalPrice); // Final total after 15% off clothing items
            Assert.Equal(157.5m, result.DiscountTotal); // Total discount applied (15% of 350+700 = 157.5)

            VerifyCommonMocks(cartId, cart);
        }

        [Fact]
        public async Task AppliedDiscount_PointsRedemption_ShouldCalculateCorrectly()
        {
            // Arrange
            var cartId = 1;
            var customerId = 1;
            var customer = new Customers { Id = customerId, Name = "John Doe", Email = "John@gmail.com", Points = 68 };
            var cart = new Cart
            {
                Id = cartId,
                CustomerId = customerId,
                TotalPrice = 830
            };

            var cartItems = new List<CartItems>
            {
                new CartItems { Item = new Items { Category = "Clothing", Name = "T-Shirt", Price = 350 }, Quantity = 1 },
                new CartItems { Item = new Items { Category = "Accessories", Name = "Hat", Price = 250 }, Quantity = 1 },
                new CartItems { Item = new Items { Category = "Accessories", Name = "Belt", Price = 230 }, Quantity = 1 }
            };

            var discountRequests = new[]
            {
                new DiscountCategoryRequest {
                    DiscountCategoryType = DiscountCategoryType.OnTop,
                    CampaignIds = 4,
                }
            };

            var discountCampaigns = new List<DiscountCampaigns>
            {
                new DiscountCampaigns {
                    Id = 4,
                    CampaignType = CampaignType.DiscountByPoints.ToString(),
                    PointsCap = 0.20m,
                    Category = "OnTop"
                }
            };

            SetupCommonMocks(cartId, customerId, cart, cartItems, customer, discountCampaigns);

            var request = new AppliedDiscountRequestDTO
            {
                CartId = cartId,
                DiscountCategoryRequest = discountRequests
            };

            // Act
            var result = await _appliedDiscountService.AppliedDiscount(request);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(830m, cartItems.Sum(x => x.Item.Price));
            Assert.Equal(762m, result.Cart.TotalPrice); // Final total after points redemption
            Assert.Equal(68m, result.DiscountTotal); // Total discount applied from points

            VerifyCommonMocks(cartId, cart);
            _customerRepositoryMock.Verify(x => x.UpdateAsync(It.Is<Customers>(c => c.Points == 32)), Times.Once);
        }

        [Fact]
        public async Task AppliedDiscount_SpecialCampaign_ShouldCalculateCorrectly()
        {
            // Arrange
            var cartId = 1;
            var customerId = 1;
            var customer = new Customers { Id = customerId, Name = "John Doe", Email = "John@gmail.com" };
            var cart = new Cart
            {
                Id = cartId,
                CustomerId = customerId,
                TotalPrice = 830
            };

            var cartItems = new List<CartItems>
            {
                new CartItems { Item = new Items { Category = "Clothing", Name = "T-Shirt", Price = 350 }, Quantity = 1 },
                new CartItems { Item = new Items { Category = "Accessories", Name = "Hat", Price = 250 }, Quantity = 1 },
                new CartItems { Item = new Items { Category = "Accessories", Name = "Belt", Price = 230 }, Quantity = 1 }
            };

            var discountRequests = new[]
            {
                new DiscountCategoryRequest { DiscountCategoryType = DiscountCategoryType.Seasonal, CampaignIds = 5 }
            };

            var discountCampaigns = new List<DiscountCampaigns>
            {
                new DiscountCampaigns {
                    Id = 5,
                    CampaignType = CampaignType.SpecialCampaign.ToString(),
                    EveryXThb = 300,
                    DiscountYThb = 40,
                    Category = "Seasonal"
                }
            };

            SetupCommonMocks(cartId, customerId, cart, cartItems, customer, discountCampaigns);

            var request = new AppliedDiscountRequestDTO
            {
                CartId = cartId,
                DiscountCategoryRequest = discountRequests
            };

            // Act
            var result = await _appliedDiscountService.AppliedDiscount(request);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(830m, cartItems.Sum(x => x.Item.Price));
            Assert.Equal(750m, result.Cart.TotalPrice); // Final total after special campaign discount
            Assert.Equal(80m, result.DiscountTotal); // Total discount applied (40 THB x 2 times for 830 THB)

            VerifyCommonMocks(cartId, cart);
        }

        private void SetupCommonMocks(int cartId, int customerId, Cart cart, List<CartItems> cartItems,
            Customers customer, List<DiscountCampaigns> discountCampaigns)
        {
            _cartRepositoryMock.Setup(x => x.GetByIdAsync(cartId)).ReturnsAsync(cart);
            _cartItemRepositoryMock.Setup(x => x.GetCartItemsByCartId(cartId)).ReturnsAsync(cartItems);
            _appliedDiscountRepositoryMock.Setup(x => x.IsAlreadyApplyDiscount(cartId)).ReturnsAsync(false);
            _customerRepositoryMock.Setup(x => x.GetByIdAsync(customerId)).ReturnsAsync(customer);
            _customerRepositoryMock.Setup(x => x.UpdateAsync(It.IsAny<Customers>())).ReturnsAsync(customer);
            foreach (var campaign in discountCampaigns)
            {
                _discountCampaignRepositoryMock.Setup(x => x.GetByIdAsync(campaign.Id)).ReturnsAsync(campaign);
            }
        }

        private void VerifyCommonMocks(int cartId, Cart cart)
        {
            _cartRepositoryMock.Verify(x => x.GetByIdAsync(cartId), Times.Once);
            _cartItemRepositoryMock.Verify(x => x.GetCartItemsByCartId(cartId), Times.Once);
            _appliedDiscountRepositoryMock.Verify(x => x.IsAlreadyApplyDiscount(cartId), Times.Once);
            _appliedDiscountRepositoryMock.Verify(x => x.AddAsync(It.IsAny<AppliedDiscounts>()), Times.AtLeastOnce);
            _cartRepositoryMock.Verify(x => x.UpdateAsync(cart), Times.Once);

        }
    }
}
