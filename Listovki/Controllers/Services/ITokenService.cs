using Listovki.Models;

namespace Listovki.Controllers.Services {
    public interface ITokenService {
        Task<string> GenerateToken(AppUser user);
    }
}
