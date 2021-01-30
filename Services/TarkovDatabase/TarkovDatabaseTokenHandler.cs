﻿using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace TarkovItemBot.Services
{
    public class TarkovDatabaseTokenHandler : DelegatingHandler
    {
        private readonly TarkovDatabaseTokenCache _cache;

        public TarkovDatabaseTokenHandler(TarkovDatabaseTokenCache cache)
        {
            _cache = cache;
        }

        protected override async Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            var token = await _cache.GetTokenAsync();

            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

            return await base.SendAsync(request, cancellationToken);
        }
    }
}
