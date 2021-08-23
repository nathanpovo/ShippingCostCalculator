using ShippingCostCalculator.ViewModels;

namespace ShippingCostCalculator.Views
{
    public partial class Index
    {
        public Index()
        {
            ViewModel ??= new IndexViewModel();
        }

        private string Cargo4YouCostValue => $"€{ViewModel?.Cargo4YouCost?.ToString("F3")}";
        private string ShipFasterCostValue => $"€{ViewModel?.ShipFasterCost?.ToString("F3")}";
        private string MaltaShipCostValue => $"€{ViewModel?.MaltaShipCost?.ToString("F3")}";

        private bool CalculatedCost { get; set; }

        private void CalculateCost()
            => CalculatedCost = true;
    }
}
