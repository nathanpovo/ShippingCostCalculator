using FluentAssertions;
using ShippingCostCalculator.Domain.PackageDetailsValidators;
using Xunit;

namespace ShippingCostCalculator.Domain.Tests.PackageDetailsValidators
{
    public class Cargo4YouPackageDetailsValidatorTests
    {
        [Theory]
        [InlineData(-123123.333321)]
        [InlineData(-10)]
        [InlineData(0)]
        public void WhenWeightValueIsTooLow_ShouldReturnFalse(float weight)
        {
            Cargo4YouPackageDetailsValidator validator = new();

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
                .Be(0);
        }

        [Theory]
        [InlineData(20.00001)]
        [InlineData(30)]
        [InlineData(123456789)]
        public void WhenWeightValueIsTooHigh_ShouldReturnFalse(float weight)
        {
            Cargo4YouPackageDetailsValidator validator = new();

            ValidationResult isWeightValid = validator.IsWeightValid(weight);

            isWeightValid.IsValid.Should().BeFalse("weight value is too high for this courier");
            isWeightValid.Errors
                .Should()
                .Contain(error => error.ErrorType == ValidationErrorType.ValueIsTooHigh)
                .Which
                .ValidationLimits
                .Should()
                .BeOfType<ValidationLimits>()
                .Which
                .UpperLimit
                .Should()
                .Be(20);
        }

        [Theory]
        [InlineData(19.9999999)]
        [InlineData(15)]
        [InlineData(2)]
        public void WhenWeightValueIsWithinTheLimits_ShouldReturnTrue(float weight)
        {
            Cargo4YouPackageDetailsValidator validator = new();

            ValidationResult isWeightValid = validator.IsWeightValid(weight);

            isWeightValid.IsValid.Should().BeTrue("weight value is within the limit for this courier");
            isWeightValid.Errors.Should().BeNullOrEmpty();
        }

        [Theory]
        [InlineData(-123123.333321)]
        [InlineData(-10)]
        [InlineData(0)]
        public void WhenVolumeValueIsTooLow_ShouldReturnFalse(float volume)
        {
            Cargo4YouPackageDetailsValidator validator = new();

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
                .Be(0);
        }

        [Theory]
        [InlineData(2000.0001)]
        [InlineData(2123.1233)]
        [InlineData(3000)]
        [InlineData(9876)]
        public void WhenVolumeValueIsTooHigh_ShouldReturnFalse(float volume)
        {
            Cargo4YouPackageDetailsValidator validator = new();

            PackageDimensions packageDimensions = new(volume);
            ValidationResult isVolumeValid = validator.IsVolumeValid(packageDimensions);

            isVolumeValid.IsValid.Should().BeFalse("volume value is too high for this courier");
            isVolumeValid.Errors
                .Should()
                .Contain(error => error.ErrorType == ValidationErrorType.ValueIsTooHigh)
                .Which
                .ValidationLimits
                .Should()
                .BeOfType<ValidationLimits>()
                .Which
                .UpperLimit
                .Should()
                .Be(2000);
        }

        [Theory]
        [InlineData(1999.999999)]
        [InlineData(100)]
        [InlineData(1)]
        [InlineData(123)]
        public void WhenVolumeValueIsWithinTheLimit_ShouldReturnTrue(float volume)
        {
            Cargo4YouPackageDetailsValidator validator = new();

            PackageDimensions packageDimensions = new(volume);
            ValidationResult isVolumeValid = validator.IsVolumeValid(packageDimensions);

            isVolumeValid.IsValid.Should().BeTrue("volume value is within the limit for this courier");
            isVolumeValid.Errors.Should().BeNullOrEmpty();
        }
    }
}
