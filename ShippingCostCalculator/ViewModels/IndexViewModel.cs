using System;
using System.Linq;
using System.Reactive.Linq;
using Microsoft.EntityFrameworkCore;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using ShippingCostCalculator.Data;
using ShippingCostCalculator.Domain;
using ShippingCostCalculator.Models;

namespace ShippingCostCalculator.ViewModels
{
    public class IndexViewModel : ReactiveObject
    {
        private readonly IDbContextFactory<ShippingContext> contextFactory;
        public readonly CourierModel[] Couriers;

        public IndexViewModel(IDbContextFactory<ShippingContext> contextFactory)
        {
            this.contextFactory = contextFactory;

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
