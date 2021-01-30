using Microsoft.Extensions.Caching.Memory;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;

namespace TarkovItemBot.Services
{
    public class TarkovDatabaseTokenCache
    {
        private readonly IMemoryCache _cache;
        private readonly TarkovDatabaseAuthClient _authClient;

        public TarkovDatabaseTokenCache(IMemoryCache cache, TarkovDatabaseAuthClient authClient)
        {
            _cache = cache;
            _authClient = authClient;
        }

        public async Task<string> GetTokenAsync()
        {
            return await _cache.GetOrCreateAsync(nameof(TarkovDatabaseTokenCache), async factory =>
            {
                var token = await _authClient.GetTokenAsync();

                var handler = new JwtSecurityTokenHandler();
                var expiry = handler.ReadJwtToken(token).ValidTo;

                factory.SetAbsoluteExpiration(expiry - TimeSpan.FromMinutes(1));

                return token;
            });
        }
    }
}
