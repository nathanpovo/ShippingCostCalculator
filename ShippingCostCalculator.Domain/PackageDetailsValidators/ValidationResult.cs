using System;

namespace ShippingCostCalculator.Domain.PackageDetailsValidators
{
    public class ValidationResult
    {
        public ValidationError[] Errors { get; }
        public bool IsValid { get; private set; }

        private ValidationResult(ValidationError[]? errors)
            => Errors = errors ?? Array.Empty<ValidationError>();

        public static ValidationResult Failed(params ValidationError[] errors)
            => new(errors) { IsValid = false };

        public static ValidationResult Success => new(null) { IsValid = true };
    }
}
