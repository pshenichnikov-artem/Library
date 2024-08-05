using Library.Core.Domain.IdentityEntities;

namespace Library.Core.Domain.RepositrotyContracts
{
    public interface IUserRepository
    {
        Task<ApplicationUser?> GetByIdAsync(Guid userId);
        Task<IEnumerable<ApplicationUser>> GetAllAsync();
        Task<bool> AddAsync(ApplicationUser user);
        Task<bool> UpdateAsync(ApplicationUser user);
        Task<bool> DeleteAsync(Guid userId);
    }
}
