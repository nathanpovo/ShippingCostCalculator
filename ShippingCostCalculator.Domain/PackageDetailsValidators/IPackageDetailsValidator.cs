namespace ShippingCostCalculator.Domain.PackageDetailsValidators
{
    public interface IPackageDetailsValidator
    {
        /// <summary>
        /// Validates the dimensions of the package.
        /// </summary>
        /// <param name="packageDimensions">The dimensions of the package</param>
        /// <returns><see langword="true"/> if the package is within the limit and <see langword="false"/> if it is not.</returns>
        bool IsVolumeValid(PackageDimensions packageDimensions);

        /// <summary>
        /// Validates the weight of the package.
        /// </summary>
        /// <param name="weight">The weight of the package.</param>
        /// <returns><see langword="true"/> if the package is within the limit and <see langword="false"/> if it is not.</returns>
        bool IsWeightValid(float weight);
    }
}
