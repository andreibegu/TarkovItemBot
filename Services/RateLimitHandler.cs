using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace TarkovItemBot.Services
{
    public class RateLimitHandler : DelegatingHandler
    {
        private DateTimeOffset? _resetTime = null;
        private int _requestCount;
        private readonly int _requestLimit;
        private readonly TimeSpan _resetDuration;
        private readonly SemaphoreSlim _requestLock = new SemaphoreSlim(1, 1);
        private readonly SemaphoreSlim _timeLock = new SemaphoreSlim(1, 1);

        public RateLimitHandler(int requestLimit, TimeSpan resetDuration)
        {
            _requestLimit = requestLimit;
            _resetDuration = resetDuration;
        }

        protected override async Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
        {

            await HandleRateLimitAsync(cancellationToken);

            var req = await base.SendAsync(request, cancellationToken);

            if (req.IsSuccessStatusCode)
            {
                await _requestLock.WaitAsync(cancellationToken);
                _requestCount--;
                _requestLock.Release();
            }

            return req;
        }

        private async Task HandleRateLimitAsync(CancellationToken cancellationToken)
        {
            await _timeLock.WaitAsync(cancellationToken);
            if (_resetTime == null || DateTimeOffset.UtcNow > _resetTime)
            {
                _resetTime = DateTimeOffset.UtcNow + _resetDuration;
                _requestCount = _requestLimit;
            }
            _timeLock.Release();

            if (_requestCount > 0) return;

            await Task.Delay((_resetTime - DateTimeOffset.UtcNow).Value, cancellationToken);
        }
    }
}
