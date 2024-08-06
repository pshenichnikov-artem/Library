using AutoMapper;
using Library.Core.Domain.Entities;
using Library.Core.Domain.IdentityEntities;
using Library.Core.Domain.RepositrotyContracts;
using Library.Core.DTO;
using Library.Core.DTO.Account;
using Library.Core.ServiceContracts;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;

namespace Library.Core.Services
{
    public class ApplicationUserService : IApplicationUserService
    {
        private readonly IUserBookViewRepository _userViewRepository;
        private readonly IUserRepository _userRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;

        public ApplicationUserService(
            IUserBookViewRepository userViewRepository,
            IUserRepository userRepository,
            UserManager<ApplicationUser> userManager,
            IMapper mapper)
        {
            _userViewRepository = userViewRepository;
            _userManager = userManager;
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<UserBookViewResponse?>> GetAllAsync()
        {
            var usersViewBook = await _userViewRepository.GetAllAsync();

            return _mapper.Map<IEnumerable<UserBookViewResponse>>(usersViewBook);
        }

        public async Task<UserBookViewResponse?> GetByIdAsync(Guid? id)
        {
            if (id == null)
            {
                throw new ArgumentNullException(nameof(id));
            }

            var user = await _userViewRepository.GetByUserIdAsync(id.Value);
            if (user == null )
            {
                var usersViewBookResponse = new UserBookViewResponse();
                usersViewBookResponse.User = await _userRepository.GetByIdAsync(id.Value);
                if (usersViewBookResponse.User == null)
                    return null;

                return usersViewBookResponse;
            }
            var usersViewBook = new List<UserBookView>() { user };
            return _mapper.Map<UserBookViewResponse>(usersViewBook.AsEnumerable());
        }

        public async Task<ApplicationUserResponse?> AddAsync(ApplicationUserUpdateRequest? userRequest)
        {
            if (userRequest == null)
            {
                throw new ArgumentNullException(nameof(userRequest));
            }

            var user = _mapper.Map<ApplicationUser>(userRequest);
            var result = await _userManager.CreateAsync(user, userRequest.Password);

            if (result.Succeeded)
            {
                return _mapper.Map<ApplicationUserResponse>(user);
            }

            throw new InvalidOperationException("User could not be created.");
        }

        public async Task<bool> UpdateAsync(ApplicationUserUpdateRequest? updateRequest)
        {
            if (updateRequest == null || updateRequest.UserId == null)
            {
                throw new ArgumentNullException(nameof(updateRequest));
            }

            var user = await _userRepository.GetByIdAsync(updateRequest.UserId.Value);

            if (user == null)
            {
                throw new KeyNotFoundException("User not found.");
            }

            _mapper.Map(updateRequest, user);
            var result = await _userManager.UpdateAsync(user);

            if (updateRequest.Password != null)
            {
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                var resetResult = await _userManager.ResetPasswordAsync(user, token, updateRequest.Password);

                if (!resetResult.Succeeded)
                {
                    throw new InvalidOperationException("Password could not be updated.");
                }
            }

            return result.Succeeded;
        }

        public async Task<bool> DeleteByIdAsync(Guid? id)
        {
            if (id == null)
            {
                throw new ArgumentNullException(nameof(id));
            }

            var result = await _userViewRepository.DeleteAsync(id.Value);
            return result;
        }
    }

}
