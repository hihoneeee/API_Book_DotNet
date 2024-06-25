using StackExchange.Redis;

namespace TestWebAPI.Configs
{
    public class RedisCacheConfig
    {
        private readonly ConnectionMultiplexer _redis;
        private readonly IDatabase _db;

        public RedisCacheConfig(string connectionString)
        {
            _redis = ConnectionMultiplexer.Connect(connectionString);
            _db = _redis.GetDatabase();
        }

        public async Task<string> GetCacheValueAsync(string key)
        {
            return await _db.StringGetAsync(key);
        }

        public async Task SetCacheValueAsync(string key, string value, TimeSpan expiration)
        {
            await _db.StringSetAsync(key, value, expiration);
        }
    }
}
