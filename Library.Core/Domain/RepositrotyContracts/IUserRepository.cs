using Library.Core.Domain.IdentityEntities;

namespace Library.Core.Domain.RepositrotyContracts
{
    public interface IUserRepository
    {
        Task<ApplicationUser?> GetByIdAsync(Guid userId);
        Task<IEnumerable<ApplicationUser>> GetAllAsync();
        Task<bool> Update(ApplicationUser user);
    }
}
