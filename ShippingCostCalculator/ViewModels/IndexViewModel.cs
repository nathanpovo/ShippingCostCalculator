using System;
using System.Linq.Expressions;
using System.Reactive.Linq;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using ShippingCostCalculator.Domain;

namespace ShippingCostCalculator.ViewModels
{
    public class IndexViewModel : ReactiveObject
    {
        private readonly Courier cargo4You = Courier.Create(CourierType.Cargo4You);
        private readonly Courier shipFaster = Courier.Create(CourierType.ShipFaster);
        private readonly Courier maltaShip = Courier.Create(CourierType.MaltaShip);

        public IndexViewModel()
        {
            this.WhenAnyValue(x => x.Length, x => x.Width, x => x.Height,
                    (length, width, height) => new PackageDimensions(length, width, height))
                .ToPropertyEx(this, x => x.PackageDimensions);

            SetupCostProperty(cargo4You, x => x.Cargo4YouCost);
            SetupCostProperty(shipFaster, x => x.ShipFasterCost);
            SetupCostProperty(maltaShip, x => x.MaltaShipCost);
        }

        private void SetupCostProperty(Courier courier, Expression<Func<IndexViewModel, float?>> property)
        {
            IObservable<bool> volumeIsValid = this.WhenAnyValue(x => x.PackageDimensions)
                .Select(packageDimensions => packageDimensions is not null && courier.IsVolumeValid(packageDimensions))
                .StartWith(false)
                .Publish()
                .RefCount();

            IObservable<bool> weightIsValid = this.WhenAnyValue(x => x.Weight)
                .Select(courier.IsWeightValid)
                .StartWith(false)
                .Publish()
                .RefCount();

            volumeIsValid
                .CombineLatest(weightIsValid, (volumeValid, weightValid) => volumeValid && weightValid)
                .Select<bool, long?>(isValid => isValid
                    ? courier.CalculateCost(PackageDimensions!, Weight)
                    : null)
                .Select<long?, float?>(cost => cost is not null
                    ? (float) cost / 1000f
                    : null)
                .ToPropertyEx(this, property);
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
        public float? Cargo4YouCost { get; }

        [ObservableAsProperty]
        public float? ShipFasterCost { get; }

        [ObservableAsProperty]
        public float? MaltaShipCost { get; }
    }
}
