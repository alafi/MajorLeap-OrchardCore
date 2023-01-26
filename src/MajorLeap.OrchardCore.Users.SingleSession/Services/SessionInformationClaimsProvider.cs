using Microsoft.Extensions.Caching.Distributed;
using OrchardCore.Users;
using OrchardCore.Users.Services;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MajorLeap.OrchardCore.Users.SingleSession.Services
{
    public class SessionInformationClaimsProvider : IUserClaimsProvider
    {
        private readonly IDistributedCache _distributedCache;

        public SessionInformationClaimsProvider(IDistributedCache distributedCache)
        {
            _distributedCache = distributedCache;
        }
        public async Task GenerateAsync(IUser user, ClaimsIdentity claims)
        {
            var newSessionId = Guid.NewGuid().ToString("N");
            await _distributedCache.SetStringAsync(Startup._SESSION_ID_KEY, newSessionId);
            claims.AddClaim(new Claim(Startup._SESSION_ID_KEY, newSessionId));
        }
    }
}
