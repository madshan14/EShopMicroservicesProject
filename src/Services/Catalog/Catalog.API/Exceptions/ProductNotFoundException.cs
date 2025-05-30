using BuildingBlocks.Exceptions;

namespace Catalog.API.Exceptions
{
    internal class ProductNotFoundException : NotFoundException
    {
        public ProductNotFoundException(Guid Id) : base("Product", Id)
        {
        }
    }
}
