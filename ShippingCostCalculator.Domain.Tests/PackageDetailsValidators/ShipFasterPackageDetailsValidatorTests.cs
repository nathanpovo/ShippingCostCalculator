using FluentAssertions;
using ShippingCostCalculator.Domain.PackageDetailsValidators;
using Xunit;

namespace ShippingCostCalculator.Domain.Tests.PackageDetailsValidators
{
    public class ShipFasterPackageDetailsValidatorTests
    {
        [Theory]
        [InlineData(-123123.333321)]
        [InlineData(-10)]
        [InlineData(0)]
        [InlineData(8)]
        [InlineData(10)]
        public void WhenWeightValueIsTooLow_ShouldReturnFalse(float weight)
        {
            ShipFasterPackageDetailsValidator validator = new();

            bool isWeightValid = validator.IsWeightValid(weight);

            isWeightValid.Should().BeFalse("weight value is too low for this courier");
        }

        [Theory]
        [InlineData(30.00001)]
        [InlineData(50)]
        [InlineData(123456789)]
        public void WhenWeightValueIsTooHigh_ShouldReturnFalse(float weight)
        {
            ShipFasterPackageDetailsValidator validator = new();

            bool isWeightValid = validator.IsWeightValid(weight);

            isWeightValid.Should().BeFalse("weight value is too high for this courier");
        }

        [Theory]
        [InlineData(19.9999999)]
        [InlineData(15)]
        [InlineData(25)]
        [InlineData(29.999)]
        public void WhenWeightValueIsWithinTheLimits_ShouldReturnTrue(float weight)
        {
            ShipFasterPackageDetailsValidator validator = new();

            bool isWeightValid = validator.IsWeightValid(weight);

            isWeightValid.Should().BeTrue("weight value is within the limit for this courier");
        }

        [Theory]
        [InlineData(-123123.333321)]
        [InlineData(-10)]
        [InlineData(0)]
        public void WhenVolumeValueIsTooLow_ShouldReturnFalse(float volume)
        {
            ShipFasterPackageDetailsValidator validator = new();

            PackageDimensions packageDimensions = new(volume);
            bool isVolumeValid = validator.IsVolumeValid(packageDimensions);

            isVolumeValid.Should().BeFalse("volume value is too low for this courier");
        }

        [Theory]
        [InlineData(1700.0001)]
        [InlineData(2123.1233)]
        [InlineData(3000)]
        [InlineData(9876)]
        public void WhenVolumeValueIsTooHigh_ShouldReturnFalse(float volume)
        {
            ShipFasterPackageDetailsValidator validator = new();

            PackageDimensions packageDimensions = new(volume);
            bool isVolumeValid = validator.IsVolumeValid(packageDimensions);

            isVolumeValid.Should().BeFalse("volume value is too high for this courier");
        }

        [Theory]
        [InlineData(1699.999999)]
        [InlineData(100)]
        [InlineData(1)]
        [InlineData(123)]
        public void WhenVolumeValueIsWithinTheLimit_ShouldReturnTrue(float volume)
        {
            ShipFasterPackageDetailsValidator validator = new();

            PackageDimensions packageDimensions = new(volume);
            bool isVolumeValid = validator.IsVolumeValid(packageDimensions);

            isVolumeValid.Should().BeTrue("volume value is within the limit for this courier");
        }
    }
}
