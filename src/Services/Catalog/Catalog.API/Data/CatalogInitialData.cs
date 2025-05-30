using Marten.Schema;

namespace Catalog.API.Data
{
    public class CatalogInitialData : IInitialData
    {
        public async Task Populate(IDocumentStore store, CancellationToken cancellation)
        {
            using var session = store.LightweightSession();

            if (await session.Query<Product>().AnyAsync(cancellation))
                return;

            session.Store<Product>(GetPreconfiguredProducts());
            await session.SaveChangesAsync(cancellation);
        }

        private static IEnumerable<Product> GetPreconfiguredProducts() => new List<Product>()
            {
                new Product()
                {
                    Id = Guid.NewGuid(),
                    Name = "IPhone X",
                    Description = "This phone is the company's biggest change to its product line!",
                    ImageFile = "product-1.png",
                    Price = 1000.00M,
                    Category = new List<string> { "Smart Phone" }

                },
                new Product()
                {
                    Id = Guid.NewGuid(),
                    Name = "Samsung S20 Ultra",
                    Description = "This phone is the company's biggest change to its product line!",
                    ImageFile = "product-2.png",
                    Price = 900.00M,
                    Category = new List<string> { "Smart Phone" }
                }
            };
    }
}
