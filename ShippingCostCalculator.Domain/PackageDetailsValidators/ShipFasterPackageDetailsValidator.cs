namespace ShippingCostCalculator.Domain.PackageDetailsValidators
{
    public class ShipFasterPackageDetailsValidator : IPackageDetailsValidator
    {
        public bool IsVolumeValid(PackageDimensions packageDimensions)
            => packageDimensions.Volume is > 0 and <= 1700;

        public bool IsWeightValid(float weight)
            => weight is > 10 and <= 30;
    }
}
