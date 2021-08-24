using ShippingCostCalculator.ViewModels;

namespace ShippingCostCalculator.Views
{
    public partial class Index
    {
        public Index()
        {
            ViewModel ??= new IndexViewModel();
        }

        private bool CalculatedCost { get; set; }

        private void CalculateCost()
            => CalculatedCost = true;

        private void PostWithCargo4You() { }

        private void PostWithShipFaster() { }

        private void PostWithMaltaShip() { }
    }
}
