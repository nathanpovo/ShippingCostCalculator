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
        private readonly Courier cargo4You;
        private readonly Courier shipFaster;
        private readonly Courier maltaShip;

        public IndexViewModel()
        {
            this.WhenAnyValue(x => x.Length, x => x.Width, x => x.Height,
                    (length, width, height) => new PackageDimensions(length, width, height))
                .ToPropertyEx(this, x => x.PackageDimensions);

            IObservable<PackageDimensions> packageDimensionsObservable = this.WhenAnyValue(x => x.PackageDimensions)
                .WhereNotNull()
                .Publish()
                .RefCount();

            IObservable<float> weightObservable = this.WhenAnyValue(x => x.Weight)
                .Publish()
                .RefCount();

            cargo4You = CreateCourier(CourierType.Cargo4You, packageDimensionsObservable, weightObservable);
            shipFaster = CreateCourier(CourierType.ShipFaster, packageDimensionsObservable, weightObservable);
            maltaShip = CreateCourier(CourierType.MaltaShip, packageDimensionsObservable, weightObservable);

            SetupCostProperty(cargo4You, x => x.Cargo4YouCost);
            SetupCostProperty(shipFaster, x => x.ShipFasterCost);
            SetupCostProperty(maltaShip, x => x.MaltaShipCost);
        }

        private void SetupCostProperty(Courier courier, Expression<Func<IndexViewModel, string?>> property)
            => courier.PackageCost
                    .Select(cost => $"€{cost?.ToString("F3")}")
                    .ToPropertyEx(this, property);

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
    }
}
