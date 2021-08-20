using System;
using ShippingCostCalculator.Domain.CostCalculators;
using ShippingCostCalculator.Domain.PackageDetailsValidators;

namespace ShippingCostCalculator.Domain
{
    public class Courier
    {
        internal readonly ICostCalculator CostCalculator;
        internal readonly IPackageDetailsValidator PackageDetailsValidator;

        private Courier(ICostCalculator costCalculator, IPackageDetailsValidator packageDetailsValidator)
        {
            CostCalculator = costCalculator;
            PackageDetailsValidator = packageDetailsValidator;
        }

        public static Courier Create(CourierType courierType)
            => courierType switch
            {
                CourierType.Cargo4You => new Courier(new Cargo4YouCostCalculator(),
                    new Cargo4YouPackageDetailsValidator()),
                CourierType.ShipFaster => new Courier(new ShipFasterCostCalculator(),
                    new ShipFasterPackageDetailsValidator()),
                CourierType.MaltaShip => new Courier(new MaltaShipCostCalculator(),
                    new MaltaShipPackageDetailsValidator()),
                _ => throw new ArgumentOutOfRangeException(nameof(courierType), courierType, null)
            };

        internal static Courier Create(ICostCalculator costCalculator, IPackageDetailsValidator packageDetailsValidator)
            => new(costCalculator, packageDetailsValidator);

        public long CalculateCost(PackageDimensions packageDimensions, float weight)
        {
            long costBasedOnDimensions = CostCalculator.CalculateCostBasedOnDimensions(packageDimensions);
            long costBasedOnWeight = CostCalculator.CalculateCostBasedOnWeight(weight);

            return costBasedOnDimensions >= costBasedOnWeight
                ? costBasedOnDimensions
                : costBasedOnWeight;
        }

        /// <inheritdoc cref="IPackageDetailsValidator.IsVolumeValid"/>
        public bool IsVolumeValid(PackageDimensions packageDimensions)
            => PackageDetailsValidator.IsVolumeValid(packageDimensions);

        /// <inheritdoc cref="IPackageDetailsValidator.IsWeightValid"/>
        public bool IsWeightValid(float weight)
            => PackageDetailsValidator.IsWeightValid(weight);
    }
}
