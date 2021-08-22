using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using ShippingCostCalculator.Domain;

namespace ShippingCostCalculator.ViewModels
{
    public class IndexViewModel : ReactiveObject
    {
        public IndexViewModel()
        {
            this.WhenAnyValue(x => x.Length, x => x.Width, x => x.Height,
                    (length, width, height) => new PackageDimensions(length, width, height))
                .ToPropertyEx(this, x => x.PackageDimensions);
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
