using FluentAssertions;
using ShippingCostCalculator.Domain.PackageDetailsValidators;
using Xunit;

namespace ShippingCostCalculator.Domain.Tests.PackageDetailsValidators
{
    public class MaltaShipPackageDetailsValidatorTests
    {
        [Theory]
        [InlineData(-123123.333321)]
        [InlineData(-10)]
        [InlineData(0)]
        [InlineData(8)]
        [InlineData(9.9999)]
        public void WhenWeightValueIsTooLow_ShouldReturnFalse(float weight)
        {
            MaltaShipPackageDetailsValidator validator = new();

            ValidationResult isWeightValid = validator.IsWeightValid(weight);

            isWeightValid.IsValid.Should().BeFalse("weight value is too low for this courier");
            isWeightValid.Errors
                .Should()
                .Contain(error => error.ErrorType == ValidationErrorType.ValueIsTooLow)
                .Which
                .ValidationLimits
                .Should()
                .BeOfType<ValidationLimits>()
                .Which
                .LowerLimit
                .Should()
                .Be(10);
        }

        [Theory]
        [InlineData(15)]
        [InlineData(19.9999999)]
        [InlineData(123.13333212)]
        [InlineData(2000)]
        public void WhenWeightValueIsWithinTheLimits_ShouldReturnTrue(float weight)
        {
            MaltaShipPackageDetailsValidator validator = new();

            ValidationResult isWeightValid = validator.IsWeightValid(weight);

            isWeightValid.IsValid.Should().BeTrue("weight value is within the limits for this courier");
            isWeightValid.Errors.Should().BeNullOrEmpty();
        }

        [Theory]
        [InlineData(-123123.333321)]
        [InlineData(-10)]
        [InlineData(0)]
        [InlineData(100)]
        [InlineData(499.123)]
        public void WhenVolumeValueIsTooLow_ShouldReturnFalse(float volume)
        {
            MaltaShipPackageDetailsValidator validator = new();

            PackageDimensions packageDimensions = new(volume);
            ValidationResult isVolumeValid = validator.IsVolumeValid(packageDimensions);

            isVolumeValid.IsValid.Should().BeFalse("volume value is too low for this courier");
            isVolumeValid.Errors
                .Should()
                .Contain(error => error.ErrorType == ValidationErrorType.ValueIsTooLow)
                .Which
                .ValidationLimits
                .Should()
                .BeOfType<ValidationLimits>()
                .Which
                .LowerLimit
                .Should()
                .Be(500);
        }

        [Theory]
        [InlineData(500)]
        [InlineData(900)]
        [InlineData(1234.123456)]
        [InlineData(9999)]
        public void WhenVolumeValueIsWithinTheLimit_ShouldReturnTrue(float volume)
        {
            MaltaShipPackageDetailsValidator validator = new();

            PackageDimensions packageDimensions = new(volume);
            ValidationResult isVolumeValid = validator.IsVolumeValid(packageDimensions);

            isVolumeValid.IsValid.Should().BeTrue("volume value is within the limit for this courier");
            isVolumeValid.Errors.Should().BeNullOrEmpty();
        }
    }
}
