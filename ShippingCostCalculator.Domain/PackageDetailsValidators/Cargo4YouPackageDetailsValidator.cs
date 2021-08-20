namespace ShippingCostCalculator.Domain.PackageDetailsValidators
{
    internal class Cargo4YouPackageDetailsValidator : IPackageDetailsValidator
    {
        public bool IsVolumeValid(PackageDimensions packageDimensions)
            => packageDimensions.Volume is > 0 and <= 2000;

        public bool IsWeightValid(float weight)
            => weight is > 0 and <= 20;
    }
}
