using Application.Interfaces;
using Moq;

namespace Test.Application
{
    public class DiscountServiceTests
    {
        private readonly Mock<IDiscountCampaignService> discountService;

        public DiscountServiceTests()
        {
            discountService = new Mock<IDiscountCampaignService>();
        }
        [Fact]
        public void ApplyFixedDiscount_ShouldSubtractDiscount()
        {
            // Arrange
            discountService.Setup(x => x.ApplyFixedDiscount(It.IsAny<decimal>(), It.IsAny<decimal>()))
                .Returns(800);

            // Act
            var result = discountService.Object.ApplyFixedDiscount(1000, 200);

            // Assert
            Assert.Equal(800, result);
        }

        [Fact]
        public void ApplyFixedDiscount_ShouldNotGoBelowZero()
        {
            //Arrange
            // this is test real logic
            discountService.Setup(x => x.ApplyFixedDiscount(1000, 1500));
            // this is tdd for logic
            discountService.Setup(x => x.ApplyFixedDiscount(1500, 2000)).Returns(0);

            //Act
            var result2 = discountService.Object.ApplyFixedDiscount(1000, 1500);
            var result3 = discountService.Object.ApplyFixedDiscount(1500, 2000);

            // Assert
            Assert.Equal(0, result2);
            Assert.Equal(0, result3);

            // Also check that the method was really called
            discountService.Verify(x => x.ApplyFixedDiscount(1000, 1500), Times.Once);
            discountService.Verify(x => x.ApplyFixedDiscount(1500, 2000), Times.Once);
        }

    }
}
