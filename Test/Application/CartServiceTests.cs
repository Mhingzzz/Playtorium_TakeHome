using Application.ContractRepo;
using Application.Interfaces;
using Application.Service;
using Domain;
using Moq;

namespace Test.Application
{
    public class CartServiceTests
    {
        private readonly Mock<ICartRepository> _cartRepositoryMock;
        private readonly ICartService _cartService;

        public CartServiceTests()
        {
            _cartRepositoryMock = new Mock<ICartRepository>();
            _cartService = new CartService(_cartRepositoryMock.Object);
        }

        [Fact]
        public async Task InitCart_WhenActiveCartExists_ShouldReturnExistingCart()
        {
            // Arrange
            int customerId = 1;
            var activeCart = new Cart { Id = 1, CustomerId = customerId, Status = CartStatus.Active.ToString() };
            var cartList = new List<Cart> { activeCart };

            _cartRepositoryMock.Setup(x => x.GetCartByCustomerId(customerId))
                .ReturnsAsync(cartList);

            // Act
            var result = await _cartService.InitCart(customerId);

            // Assert
            Assert.Equal(activeCart.Id, result.Id);
            Assert.Equal(activeCart.CustomerId, result.CustomerId);
            Assert.Equal(CartStatus.Active.ToString(), result.Status);
            
            // Verify repository was called but AddAsync was not
            _cartRepositoryMock.Verify(x => x.GetCartByCustomerId(customerId), Times.Once);
            _cartRepositoryMock.Verify(x => x.AddAsync(It.IsAny<Cart>()), Times.Never);
        }

        [Fact]
        public async Task InitCart_WhenNoActiveCartExists_ShouldCreateNewCart()
        {
            // Arrange
            int customerId = 1;
            var inactiveCart = new Cart { Id = 1, CustomerId = customerId, Status = CartStatus.Completed.ToString() };
            var cartList = new List<Cart> { inactiveCart };

            _cartRepositoryMock.Setup(x => x.GetCartByCustomerId(customerId))
                .ReturnsAsync(cartList);
            
            _cartRepositoryMock.Setup(x => x.AddAsync(It.IsAny<Cart>()))
                .ReturnsAsync((Cart cart) => { 
                    cart.Id = 2; 
                    return cart; 
                });

            // Act
            var result = await _cartService.InitCart(customerId);

            // Assert
            Assert.Equal(customerId, result.CustomerId);
            Assert.Equal(CartStatus.Active.ToString(), result.Status);
            
            // Verify both methods were called
            _cartRepositoryMock.Verify(x => x.GetCartByCustomerId(customerId), Times.Once);
            _cartRepositoryMock.Verify(x => x.AddAsync(It.IsAny<Cart>()), Times.Once);
        }

        [Fact]
        public async Task GetCartByCustomerIdAsync_WhenActiveCartExists_ShouldReturnCart()
        {
            // Arrange
            int customerId = 1;
            var activeCart = new Cart { Id = 1, CustomerId = customerId, Status = CartStatus.Active.ToString() };
            var cartList = new List<Cart> { activeCart };

            _cartRepositoryMock.Setup(x => x.GetCartByCustomerId(customerId))
                .ReturnsAsync(cartList);

            // Act
            var result = await _cartService.GetCartByCustomerIdAsync(customerId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(activeCart.Id, result.Id);
            Assert.Equal(activeCart.CustomerId, result.CustomerId);
            
            // Verify repository was called
            _cartRepositoryMock.Verify(x => x.GetCartByCustomerId(customerId), Times.Once);
        }

        [Fact]
        public async Task GetCartByCustomerIdAsync_WhenNoActiveCartExists_ShouldReturnNull()
        {
            // Arrange
            int customerId = 1;
            var inactiveCart = new Cart { Id = 1, CustomerId = customerId, Status = CartStatus.Completed.ToString() };
            var cartList = new List<Cart> { inactiveCart };

            _cartRepositoryMock.Setup(x => x.GetCartByCustomerId(customerId))
                .ReturnsAsync(cartList);

            // Act
            var result = await _cartService.GetCartByCustomerIdAsync(customerId);

            // Assert
            Assert.Null(result);
            
            // Verify repository was called
            _cartRepositoryMock.Verify(x => x.GetCartByCustomerId(customerId), Times.Once);
        }
    }
}
