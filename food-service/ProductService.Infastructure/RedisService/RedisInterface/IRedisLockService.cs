namespace food_service.ProductService.Infastructure.RedisService.RedisInterface
{
    public interface IRedisLockService
    {
        Task<bool> AcquireAsync(string key, string value, TimeSpan expiry);
        Task ReleaseAsync(string key, string value);

    }
}
