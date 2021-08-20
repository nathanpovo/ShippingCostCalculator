namespace ShippingCostCalculator.Domain.PackageDetailsValidators
{
    internal class MaltaShipPackageDetailsValidator : IPackageDetailsValidator
    {
        public bool IsVolumeValid(PackageDimensions packageDimensions)
            => packageDimensions.Volume >= 500;

        public bool IsWeightValid(float weight)
            => weight >= 10;
    }
}
