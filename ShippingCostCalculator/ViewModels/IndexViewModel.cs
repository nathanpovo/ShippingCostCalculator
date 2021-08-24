using System;
using System.Linq.Expressions;
using System.Reactive.Linq;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using ShippingCostCalculator.Domain;
using ShippingCostCalculator.Domain.PackageDetailsValidators;

namespace ShippingCostCalculator.ViewModels
{
    public class IndexViewModel : ReactiveObject
    {
        public IndexViewModel()
        {
            this.WhenAnyValue(x => x.Length, x => x.Width, x => x.Height,
                    (length, width, height) => new PackageDimensions(length, width, height))
                .ToPropertyEx(this, x => x.PackageDimensions);

            IObservable<PackageDimensions> packageDimensionsObservable = this.WhenAnyValue(x => x.PackageDimensions)
                .WhereNotNull()
                .Replay(1)
                .RefCount();

            IObservable<float> weightObservable = this.WhenAnyValue(x => x.Weight)
                .Replay(1)
                .RefCount();

            Courier cargo4You = CreateCourier(CourierType.Cargo4You, packageDimensionsObservable, weightObservable);
            Courier shipFaster = CreateCourier(CourierType.ShipFaster, packageDimensionsObservable, weightObservable);
            Courier maltaShip = CreateCourier(CourierType.MaltaShip, packageDimensionsObservable, weightObservable);

            SetupCostProperty(cargo4You, x => x.Cargo4YouCost);
            SetupCostProperty(shipFaster, x => x.ShipFasterCost);
            SetupCostProperty(maltaShip, x => x.MaltaShipCost);

            SetupValidationProperty(cargo4You, x => x.Cargo4YouValidation);
            SetupValidationProperty(shipFaster, x => x.ShipFasterValidation);
            SetupValidationProperty(maltaShip, x => x.MaltaShipValidation);
        }

        private void SetupCostProperty(Courier courier, Expression<Func<IndexViewModel, string?>> property)
            => courier.PackageCost
                    .Select(cost => cost is not null ? $"€{cost.Value:F3}" : null)
                    .ToPropertyEx(this, property);

        private void SetupValidationProperty(Courier courier, Expression<Func<IndexViewModel, string?>> property)
        {
            IObservable<string?> weightMessageObservable = courier
                .WeightValidation
                .Select(validationResult => BuildValidationMessage(validationResult, "weight"));

            IObservable<string?> volumeMessageObservable = courier
                .VolumeValidation
                .Select(validationResult => BuildValidationMessage(validationResult, "volume"));

            volumeMessageObservable
                .CombineLatest(weightMessageObservable, (volumeMessage, weightMessage)
                        => volumeMessage is not null && weightMessage is not null
                            ? $"For this courier the {weightMessage} and the {volumeMessage}."
                            : volumeMessage is not null
                                ? $"For this courier the {volumeMessage}."
                                : weightMessage is not null
                                    ? $"For this courier the {weightMessage}."
                                    : null)
                .ToPropertyEx(this, property);
        }

        private static string? BuildValidationMessage(ValidationResult validationResult, string validationType)
        {
            if (validationResult.IsValid || validationResult.ValidationError is null)
            {
                return null;
            }

            ValidationLimits? limits = validationResult.ValidationError.ValidationLimits;

            return validationResult.ValidationError.ErrorType switch
            {
                ValidationErrorType.ValueIsTooHigh => limits?.UpperLimit is not null
                    ? $"{validationType} must be less than {limits.UpperLimit}"
                    : $"{validationType} is too high",
                ValidationErrorType.ValueIsTooLow => limits?.LowerLimit is not null
                    ? $"{validationType} must be greater than {limits.LowerLimit}"
                    : $"{validationType} is too low",
                _ => limits?.UpperLimit is not null && limits.LowerLimit is not null
                    ? $"{validationType} must be between {limits.LowerLimit} and {limits.UpperLimit}"
                    : $"{validationType} is not within range"
            };
        }

        private static Courier CreateCourier(CourierType courierType,
            IObservable<PackageDimensions> packageDimensionsObservable, IObservable<float> weightObservable)
        {
            Courier courier = Courier.Create(courierType);
            courier.InitializeObservables(packageDimensionsObservable, weightObservable);

            return courier;
        }

        [Reactive]
        public float Length { get; set; }

        [Reactive]
        public float Width { get; set; }

        [Reactive]
        public float Height { get; set; }

        [Reactive]
        public float Weight { get; set; }

        [ObservableAsProperty]
        public PackageDimensions? PackageDimensions { get; }

        [ObservableAsProperty]
        public string? Cargo4YouCost { get; }

        [ObservableAsProperty]
        public string? ShipFasterCost { get; }

        [ObservableAsProperty]
        public string? MaltaShipCost { get; }

        [ObservableAsProperty]
        public string? Cargo4YouValidation { get; }

        [ObservableAsProperty]
        public string? ShipFasterValidation { get; }

        [ObservableAsProperty]
        public string? MaltaShipValidation { get; }
    }
}
