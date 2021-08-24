namespace ShippingCostCalculator.Data
{
    public class ShippingData
    {
        public ShippingData(int courierId, float weight, float length, float width, float height, float shippingCost)
        {
            CourierId = courierId;
            Weight = weight;
            Length = length;
            Width = width;
            Height = height;
            ShippingCost = shippingCost;
        }

        public long Id { get; set; }
        public int CourierId { get; set; }
        public Courier? Courier { get; set; }
        public float Weight { get; set; }
        public float Length { get; set; }
        public float Width { get; set; }
        public float Height { get; set; }
        public float ShippingCost { get; set; }
    }
}
