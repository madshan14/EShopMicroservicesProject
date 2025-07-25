using System.Text.Json;

namespace Basket.API.Data
{
    public class CacheBasketRepository
        (IBasketRepository repository, IDistributedCache cache) 
        : IBasketRepository
    {

        public async Task<ShoppingCart> GetBasketAsync(string userName, CancellationToken cancellationToken = default)
        {
            var cachedBasket = await cache.GetStringAsync(userName, cancellationToken);
             
            if (!string.IsNullOrEmpty(cachedBasket))
            {
                return JsonSerializer.Deserialize<ShoppingCart>(cachedBasket)!;
            }

            var basket = await repository.GetBasketAsync(userName, cancellationToken);

            await cache.SetStringAsync(userName, JsonSerializer.Serialize(basket), cancellationToken);
            return basket;
        }

        public async Task<ShoppingCart> StoreBasketAsync(ShoppingCart cart, CancellationToken cancellationToken = default)
        {

            await repository.StoreBasketAsync(cart, cancellationToken);
            await cache.SetStringAsync(cart.UserName, JsonSerializer.Serialize(cart), cancellationToken);
            return cart;
        }
        public async Task<bool> DeleteBasketAsync(string userName, CancellationToken cancellationToken = default)
        {
            var deleted = await repository.DeleteBasketAsync(userName, cancellationToken);
            if (deleted)
            {
                await cache.RemoveAsync(userName, cancellationToken);
            }
            return deleted;
        }
    }
}
