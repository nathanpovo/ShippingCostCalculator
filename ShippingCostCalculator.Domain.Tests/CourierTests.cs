using System;
using System.Collections.Generic;
using AutoFixture;
using FluentAssertions;
using Moq;
using ShippingCostCalculator.Domain.CostCalculators;
using ShippingCostCalculator.Domain.PackageDetailsValidators;
using Xunit;

namespace ShippingCostCalculator.Domain.Tests
{
    public class CourierTests
    {
        [Fact]
        public void WhenCreatingCargo4YouCourier_ShouldUseCorrectTypes()
        {
            Courier courier = Courier.Create(CourierType.Cargo4You);

            courier.CostCalculator.Should().BeOfType<Cargo4YouCostCalculator>();
            courier.PackageDetailsValidator.Should().BeOfType<Cargo4YouPackageDetailsValidator>();
        }

        [Fact]
        public void WhenCreatingShipFasterCourier_ShouldUseCorrectTypes()
        {
            Courier courier = Courier.Create(CourierType.ShipFaster);

            courier.CostCalculator.Should().BeOfType<ShipFasterCostCalculator>();
            courier.PackageDetailsValidator.Should().BeOfType<ShipFasterPackageDetailsValidator>();
        }

        [Fact]
        public void WhenCreatingMaltaShipCourier_ShouldUseCorrectTypes()
        {
            Courier courier = Courier.Create(CourierType.MaltaShip);

            courier.CostCalculator.Should().BeOfType<MaltaShipCostCalculator>();
            courier.PackageDetailsValidator.Should().BeOfType<MaltaShipPackageDetailsValidator>();
        }

        [Theory]
        [InlineData(-123)]
        [InlineData(0)]
        [InlineData(1000)]
        public void WhenCreatingUndefinedCourier_ShouldThrow(int value)
        {
            CourierType courierType = (CourierType) value;
            Action createCourier = () => Courier.Create(courierType);

            createCourier.Should().Throw<ArgumentOutOfRangeException>("there is no courier for the specified value");
        }

        [Theory]
        [MemberData(nameof(CostData))]
        public void WhenDimensionsCostIsGreater_ShouldReturnDimensionsCost(long dimensionsCost)
        {
            Fixture fixture = new();

            long weightCost = dimensionsCost - 1;

            Mock<ICostCalculator> costCalculatorMock = new();
            Mock<IPackageDetailsValidator> packageDetailsValidatorMock = new();

            costCalculatorMock
                .Setup(x => x.CalculateCostBasedOnWeight(It.IsAny<float>()))
                .Returns(weightCost);

            costCalculatorMock
                .Setup(x => x.CalculateCostBasedOnDimensions(It.IsAny<PackageDimensions>()))
                .Returns(dimensionsCost);

            PackageDimensions packageDimensions = fixture.Create<PackageDimensions>();
            float weight = fixture.Create<float>();

            Courier courier = Courier.Create(costCalculatorMock.Object, packageDetailsValidatorMock.Object);

            long cost = courier.CalculateCost(packageDimensions, weight);

            cost.Should().Be(dimensionsCost, "the cost calculated from the dimensions of the package is greater");
        }

        [Theory]
        [MemberData(nameof(CostData))]
        public void WhenWeightCostIsGreater_ShouldReturnWeightCost(long weightCost)
        {
            Fixture fixture = new();

            long dimensionsCost = weightCost - 1;

            Mock<ICostCalculator> costCalculatorMock = new();
            Mock<IPackageDetailsValidator> packageDetailsValidatorMock = new();

            costCalculatorMock
                .Setup(x => x.CalculateCostBasedOnWeight(It.IsAny<float>()))
                .Returns(weightCost);

            costCalculatorMock
                .Setup(x => x.CalculateCostBasedOnDimensions(It.IsAny<PackageDimensions>()))
                .Returns(dimensionsCost);

            PackageDimensions packageDimensions = fixture.Create<PackageDimensions>();
            float weight = fixture.Create<float>();

            Courier courier = Courier.Create(costCalculatorMock.Object, packageDetailsValidatorMock.Object);

            long cost = courier.CalculateCost(packageDimensions, weight);

            cost.Should().Be(weightCost, "the cost calculated from the weight of the package is greater");
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void WhenWeightIsValidated_ShouldReturnValidation(bool validationResult)
        {
            Fixture fixture = new();

            Mock<ICostCalculator> costCalculatorMock = new();
            Mock<IPackageDetailsValidator> packageDetailsValidatorMock = new();

            packageDetailsValidatorMock
                .Setup(x => x.IsWeightValid(It.IsAny<float>()))
                .Returns(validationResult);

            Courier courier = Courier.Create(costCalculatorMock.Object, packageDetailsValidatorMock.Object);

            float weight = fixture.Create<float>();

            bool isWeightValid = courier.IsWeightValid(weight);

            isWeightValid.Should().Be(validationResult);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void WhenVolumeIsValidated_ShouldReturnValidation(bool validationResult)
        {
            Fixture fixture = new();

            Mock<ICostCalculator> costCalculatorMock = new();
            Mock<IPackageDetailsValidator> packageDetailsValidatorMock = new();

            packageDetailsValidatorMock
                .Setup(x => x.IsVolumeValid(It.IsAny<PackageDimensions>()))
                .Returns(validationResult);

            Courier courier = Courier.Create(costCalculatorMock.Object, packageDetailsValidatorMock.Object);

            PackageDimensions packageDimensions = fixture.Create<PackageDimensions>();

            bool isVolumeValid = courier.IsVolumeValid(packageDimensions);

            isVolumeValid.Should().Be(validationResult);
        }

        public static IEnumerable<object[]> CostData =>
            new List<object[]>
            {
                new object[] { 1 },
                new object[] { 11320 },
                new object[] { 70000 },
                new object[] { 1230000 },
                new object[] { 4012300 },
                new object[] { 12331145 },
            };
    }
}
