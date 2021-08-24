using System;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using ShippingCostCalculator.Domain.CostCalculators;
using ShippingCostCalculator.Domain.PackageDetailsValidators;

namespace ShippingCostCalculator.Domain
{
    public class Courier
    {
        internal readonly ICostCalculator CostCalculator;
        internal readonly IPackageDetailsValidator PackageDetailsValidator;

        private readonly ReplaySubject<ValidationResult> volumeValidationSubject = new(1);
        private readonly ReplaySubject<ValidationResult> weightValidationSubject = new(1);
        private readonly ReplaySubject<float?> packageCostSubject = new(1);

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

        public void InitializeObservables(IObservable<PackageDimensions> packageDimensionsObservable,
            IObservable<float> weightObservable)
            => packageDimensionsObservable
                .CombineLatest(weightObservable, (packageDimensions, weight) =>
                {
                    ValidationResult volumeValidation = IsVolumeValid(packageDimensions);
                    ValidationResult weightValidation = IsWeightValid(weight);

                    volumeValidationSubject.OnNext(volumeValidation);
                    weightValidationSubject.OnNext(weightValidation);

                    return volumeValidation.IsValid && weightValidation.IsValid
                        ? CalculateCost(packageDimensions, weight)
                        : (long?) null;
                })
                .Select(cost
                    => cost is not null
                        ? (float) cost / 1000f
                        : (float?) null)
                .Subscribe(packageCostSubject);

        public long CalculateCost(PackageDimensions packageDimensions, float weight)
        {
            long costBasedOnDimensions = CostCalculator.CalculateCostBasedOnDimensions(packageDimensions);
            long costBasedOnWeight = CostCalculator.CalculateCostBasedOnWeight(weight);

            return costBasedOnDimensions >= costBasedOnWeight
                ? costBasedOnDimensions
                : costBasedOnWeight;
        }

        public IObservable<float?> PackageCost => packageCostSubject.AsObservable();
        public IObservable<ValidationResult> VolumeValidation => volumeValidationSubject.AsObservable();
        public IObservable<ValidationResult> WeightValidation => weightValidationSubject.AsObservable();

        /// <inheritdoc cref="IPackageDetailsValidator.IsVolumeValid"/>
        public ValidationResult IsVolumeValid(PackageDimensions packageDimensions)
            => PackageDetailsValidator.IsVolumeValid(packageDimensions);

        /// <inheritdoc cref="IPackageDetailsValidator.IsWeightValid"/>
        public ValidationResult IsWeightValid(float weight)
            => PackageDetailsValidator.IsWeightValid(weight);
    }
}
