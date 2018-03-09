using AutoFixture;
using AutoFixture.AutoNSubstitute;
using AutoFixture.Xunit2;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace AutofixtureWorkshop
{
    public class MockingFrameworkIntegrationTests
    {
        [Theory, DefaultAutoData]
        public void CalculatesTotalPrice(
            [Frozen]IOrderPriceService orderPriceService,
            [Frozen]IShippingPriceCalculator shippingPriceCalculator,
            TotalPriceCalculator sut)
        {
            orderPriceService.GetPrice().Returns(45);
            shippingPriceCalculator.Calculate().Returns(5);
            var result = sut.Calculate();
            result.Should().Be(50);
        }

        public class DefaultAutoDataAttribute : AutoDataAttribute
        {
            public DefaultAutoDataAttribute()
                : base(() => new Fixture().Customize(new AutoNSubstituteCustomization()))
            {

            }
        }

        public interface IOrderPriceService
        {
            int GetPrice();
        }

        public interface IShippingPriceCalculator
        {
            int Calculate();
        }

        public class TotalPriceCalculator
        {
            private readonly IOrderPriceService _orderPriceService;
            private readonly IShippingPriceCalculator _shippingPriceCalculator;

            public TotalPriceCalculator(IOrderPriceService orderPriceService, IShippingPriceCalculator shippingPriceCalculator)
            {
                _orderPriceService = orderPriceService;
                _shippingPriceCalculator = shippingPriceCalculator;
            }

            public int Calculate()
            {
                return _orderPriceService.GetPrice() + _shippingPriceCalculator.Calculate();
            }
        }
    }
}
