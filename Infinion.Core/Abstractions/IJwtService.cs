using Infinion.Domain.Entities;

namespace Infinion.Core.Abstractions
{
    public interface IJwtService
    {
        public string GenerateToken(AppUser user, IList<string> roles);
    }
}
