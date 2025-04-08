using Application.ContractRepo;
using Application.DTOs.Requests;
using Application.Interfaces;
using Application.Service;
using Domain;
using Moq;

namespace Test.Application
{
    public class CartItemServiceTests
    {
        private readonly Mock<ICartItemRepository> _cartItemRepositoryMock;
        private readonly Mock<ICartRepository> _cartRepositoryMock;
        private readonly Mock<IItemRepository> _itemRepositoryMock;
        private readonly ICartItemService _cartItemService;

        public CartItemServiceTests()
        {
            _cartItemRepositoryMock = new Mock<ICartItemRepository>();
            _cartRepositoryMock = new Mock<ICartRepository>();
            _itemRepositoryMock = new Mock<IItemRepository>();
            _cartItemService = new CartItemService(
                _cartItemRepositoryMock.Object, 
                _cartRepositoryMock.Object, 
                _itemRepositoryMock.Object
            );
        }

        [Fact]
        public async Task AddItemToCart_ShouldAddItemsAndUpdateCartTotal()
        {
            // Arrange
            var request = new AddItemToCartRequest
            {
                CartId = 1,
                ItemId = new[] { 1, 2 },
                Quantity = 2
            };

            var cart = new Cart { Id = 1, TotalPrice = 0 };
            var item1 = new Domain.Items { Id = 1, Price = 100 };
            var item2 = new Domain.Items { Id = 2, Price = 200 };

            var expectedCartItems = new List<CartItems>
            {
                new CartItems { CartId = 1, ItemId = 1, Quantity = 2 },
                new CartItems { CartId = 1, ItemId = 2, Quantity = 2 }
            };

            _cartRepositoryMock.Setup(x => x.GetByIdAsync(request.CartId))
                .ReturnsAsync(cart);
            
            _itemRepositoryMock.Setup(x => x.GetByIdAsync(1))
                .ReturnsAsync(item1);
            
            _itemRepositoryMock.Setup(x => x.GetByIdAsync(2))
                .ReturnsAsync(item2);
            
            _cartItemRepositoryMock.Setup(x => x.AddRangeAsync(It.IsAny<List<CartItems>>()))
                .ReturnsAsync(expectedCartItems);

            // Act
            var result = await _cartItemService.AddItemToCart(request);

            // Assert
            Assert.Equal(2, result.Count);
            Assert.Equal(600, cart.TotalPrice); // (100 + 200) * 2
            
            // Verify repository calls
            _cartRepositoryMock.Verify(x => x.GetByIdAsync(request.CartId), Times.Once);
            _itemRepositoryMock.Verify(x => x.GetByIdAsync(It.IsAny<int>()), Times.Exactly(2));
            _cartRepositoryMock.Verify(x => x.UpdateAsync(cart), Times.Once);
            _cartItemRepositoryMock.Verify(x => x.AddRangeAsync(It.IsAny<List<CartItems>>()), Times.Once);
        }

        [Fact]
        public async Task UpdateCartItem_ShouldUpdateQuantity()
        {
            // Arrange
            int cartItemId = 1;
            int newQuantity = 5;
            var cartItem = new CartItems { Id = cartItemId, Quantity = 2 };

            _cartItemRepositoryMock.Setup(x => x.GetByIdAsync(cartItemId))
                .ReturnsAsync(cartItem);
            
            _cartItemRepositoryMock.Setup(x => x.UpdateAsync(It.IsAny<CartItems>()))
                .ReturnsAsync(cartItem);

            // Act
            var result = await _cartItemService.UpdateCartItem(cartItemId, newQuantity);

            // Assert
            Assert.Equal(newQuantity, result.Quantity);
            
            // Verify repository calls
            _cartItemRepositoryMock.Verify(x => x.GetByIdAsync(cartItemId), Times.Once);
            _cartItemRepositoryMock.Verify(x => x.UpdateAsync(cartItem), Times.Once);
        }

        [Fact]
        public async Task GetCartItemsByCartId_ShouldReturnCartItems()
        {
            // Arrange
            int cartId = 1;
            var allCartItems = new List<CartItems>
            {
                new CartItems { Id = 1, CartId = cartId, ItemId = 1, Quantity = 2 },
                new CartItems { Id = 2, CartId = cartId, ItemId = 2, Quantity = 3 },
                new CartItems { Id = 3, CartId = 2, ItemId = 1, Quantity = 1 }
            };

            _cartItemRepositoryMock.Setup(x => x.GetAllAsync())
                .ReturnsAsync(allCartItems);

            // Act
            var result = await _cartItemService.GetCartItemsByCartId(cartId);

            // Assert
            Assert.Equal(2, result.Count);
            Assert.All(result, item => Assert.Equal(cartId, item.CartId));
            
            // Verify repository call
            _cartItemRepositoryMock.Verify(x => x.GetAllAsync(), Times.Once);
        }
    }
}
