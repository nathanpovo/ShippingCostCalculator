using System.Reactive.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using ShippingCostCalculator.Models;
using ShippingCostCalculator.ViewModels;

namespace ShippingCostCalculator.Views
{
    public partial class Index
    {
        [Inject]
        public IndexViewModel? IndexViewModel
        {
            get => ViewModel;
            set => ViewModel = value;
        }

        private bool CalculatedCost { get; set; }

        private void CalculateCost()
            => CalculatedCost = true;

        private async Task PostPackageAsync(CourierModel courierModel)
            => await ViewModel!.PostPackage.Execute(courierModel);
    }
}
