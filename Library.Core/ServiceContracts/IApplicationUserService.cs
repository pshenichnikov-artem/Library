using Library.Core.DTO;
using Library.Core.DTO.Account;

namespace Library.Core.ServiceContracts
{
    public interface IApplicationUserService
    {
        Task<IEnumerable<UserBookViewResponse>> GetAllAsync();
        Task<UserBookViewResponse?> GetByIdAsync(Guid? id);
        Task<ApplicationUserResponse?> AddAsync(ApplicationUserUpdateRequest? userRequest);
        Task<bool> UpdateAsync(ApplicationUserUpdateRequest? updateRequest);
        Task<bool> UpdateNameAsync(Guid? userId, string? firstName, string? lastName);
        Task<bool> DeleteByIdAsync(Guid? id);
    }
}
