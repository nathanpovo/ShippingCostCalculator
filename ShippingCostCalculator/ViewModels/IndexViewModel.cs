using System;
using System.Linq;
using System.Reactive.Linq;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using ShippingCostCalculator.Domain;
using ShippingCostCalculator.Models;

namespace ShippingCostCalculator.ViewModels
{
    public class IndexViewModel : ReactiveObject
    {
        public readonly CourierModel[] Couriers;

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

            Couriers = Enum.GetValues<CourierType>()
                .Select(courierType => new CourierModel(courierType, packageDimensionsObservable, weightObservable))
                .ToArray();
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
    }
}
