using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace ShippingCostCalculator.Data
{
    public class ShippingContextFactory : IDesignTimeDbContextFactory<ShippingContext>
    {
        public ShippingContext CreateDbContext(string[] args)
        {
            DbContextOptionsBuilder<ShippingContext> optionsBuilder = new();
            optionsBuilder.UseSqlite("Data Source=ShippingData.db");

            return new ShippingContext(optionsBuilder.Options);
        }
    }
}
