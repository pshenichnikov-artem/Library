using Library.Core.Domain.IdentityEntities;
using Library.Core.DTO;
using Library.Core.DTO.Account;
using Library.Core.Enums;
using Library.Core.ServiceContracts;
using Library.Core.Validation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace Library.UI.Controllers
{
    [Route("account")]
    public class AccountController : Controller
    {

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly IApplicationUserService _applicationUserService;
        private readonly IUserImageService _userImageService;

        public AccountController(UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            RoleManager<ApplicationRole> roleManager,
            IApplicationUserService applicationUserService,
            IUserImageService userImageService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _applicationUserService = applicationUserService;
            _userImageService = userImageService;
        }

        [HttpGet]
        public async Task<IActionResult> Account()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            Guid userGuid;
            if (userId != null)
            {
                userGuid = Guid.Parse(userId);
            }
            else
            {
                return NotFound();
            }

            var user = await _applicationUserService.GetByIdAsync(userGuid);
            if (user == null)
                return NotFound();

            return View(user);
        }

        [Route("/updateData")]
        [HttpGet]
        public async Task<IActionResult> UpdateData()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
            {
                return NotFound();
            }

            Guid userGuid;
            if (!Guid.TryParse(userId, out userGuid))
            {
                return BadRequest("Invalid user ID");
            }

            var user = await _applicationUserService.GetByIdAsync(userGuid);
            if (user == null)
            {
                return NotFound();
            }

            ViewBag.FirstName = user.User.FirstName;
            ViewBag.LastName = user.User.LastName;
            if (user.User.UserImages.Count != 0)
                ViewBag.FileName = user.User.UserImages.First()?.FileName;

            return View();
        }

        [Route("/updateData")]
        [HttpPost]
        public async Task<IActionResult> UpdateData(
            [Required(ErrorMessage = "First name is required")]
            [StringLength(50, ErrorMessage = "Last name cannot exceed 50 characters")]
            string? firstName,
            [Required(ErrorMessage = "Last name is required")]
            [StringLength(50, ErrorMessage = "Last name cannot exceed 50 characters")]
            string? lastName,
            [ImageValidation]
            IFormFile? newImage)
        {
            if (ModelState.IsValid == false)
            {
                ViewBag.Errors = ModelState.Values.SelectMany(temp => temp.Errors).Select(temp => temp.ErrorMessage);
                ViewBag.FirstName = firstName;
                ViewBag.LastName = lastName;
                return View();
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
            {
                return NotFound();
            }

            bool sassesSaveImage = false;

            if (newImage == null)
            {
                sassesSaveImage = await _userImageService.DeleteImagesByUserIdAsync(Guid.Parse(userId));
            }
            else
            {
                var image = await _userImageService.GetImagesByUserIdAsync(Guid.Parse(userId));
                if (image == null || image.Count() == 0)
                {
                    var updatedImage = await _userImageService.AddImageAsync(newImage, Guid.Parse(userId));
                    if (updatedImage != null)
                        sassesSaveImage = true;
                }
                else
                {
                    sassesSaveImage = await _userImageService.UpdateImageAsync(Guid.Parse(userId), newImage);
                }
            }

            bool sassesSaveUser = await _applicationUserService.UpdateNameAsync(Guid.Parse(userId), firstName, lastName);

            if (!sassesSaveImage || !sassesSaveUser)
                return StatusCode(500);

            return Redirect("/account");
        }


        [Route("/updatePassword")]
        [HttpGet]
        public IActionResult UpdatePassword()
        {
            return View();
        }

        [Route("/updatePassword")]
        [HttpPost]
        public async Task<IActionResult> UpdatePassword(AccountUpdatePasswordRequest accountUpdatePasswordRequest)
        {
            if (ModelState.IsValid == false)
            {
                ViewBag.Errors = ModelState.Values.SelectMany(temp => temp.Errors).Select(temp => temp.ErrorMessage);
                return View();
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
            {
                return NotFound();
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound();
            }

            var isOldPasswordValid = await _userManager.CheckPasswordAsync(user, accountUpdatePasswordRequest.OldPassword);
            if (!isOldPasswordValid)
            {
                ViewBag.Errors = new List<string> { "Старый пароль неверен." };
                return View();
            }

            var result = await _userManager.ChangePasswordAsync(user, accountUpdatePasswordRequest.OldPassword, accountUpdatePasswordRequest.NewPassword);
            if (!result.Succeeded)
            {
                ViewBag.Errors = result.Errors.Select(e => e.Description);
            }


            return Redirect("/account");
        }

        [Route("{action}")]
        [HttpGet]
        [Authorize("NotAuthorized")]
        public IActionResult Registration()
        {
            return View();
        }

        [Route("{action}")]
        [HttpPost]
        [Authorize("NotAuthorized")]
        public async Task<IActionResult> Registration(RegisterDTO registerDTO, [ImageValidation] IFormFile? newImage)
        {
            if (ModelState.IsValid == false)
            {
                ViewBag.Errors = ModelState.Values.SelectMany(temp => temp.Errors).Select(temp => temp.ErrorMessage);
                return View(registerDTO);
            }

            if (!(await IsEmailAlreadyRegistered(registerDTO.Email)))
            {
                ViewBag.Erros = new List<string>() { "A user with this email is already registered" };
            }

            ApplicationUser user = new ApplicationUser() { Email = registerDTO.Email, UserName = registerDTO.Email, FirstName = registerDTO.FirstName, LastName = registerDTO.LastName };
            IdentityResult result = await _userManager.CreateAsync(user, registerDTO.Password);

            if (result.Succeeded)
            {
                //if (registerDTO.UserType == UserTypeOptions.Admin)
                //{
                //    if (await _roleManager.FindByNameAsync(UserTypeOptions.Admin.ToString()) is null)
                //    {
                //        ApplicationRole applicationRole = new ApplicationRole() { Name = UserTypeOptions.Admin.ToString() };
                //        await _roleManager.CreateAsync(applicationRole);
                //    }

                //    await _userManager.AddToRoleAsync(user, UserTypeOptions.Admin.ToString());
                //}
                //else
                //{
                if (await _roleManager.FindByNameAsync(UserTypeOptions.User.ToString()) is null)
                {
                    ApplicationRole applicationRole = new ApplicationRole() { Name = UserTypeOptions.User.ToString() };
                    await _roleManager.CreateAsync(applicationRole);
                }

                await _userManager.AddToRoleAsync(user, UserTypeOptions.User.ToString());
                //}

                await _signInManager.SignInAsync(user, isPersistent: false);

                return Redirect("/");
            }
            else
            {
                ViewBag.Errors = result.Errors.Select(x => x.Description);
                return View(registerDTO);
            }
        }

        [Route("{action}")]
        [HttpGet]
        [Authorize("NotAuthorized")]
        public IActionResult Login()
        {
            return View();
        }

        [Route("{action}")]
        [HttpPost]
        [Authorize("NotAuthorized")]
        public async Task<IActionResult> Login(LoginDTO loginDTO, string? ReturnURL)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Errors = ModelState.Values.SelectMany(temp => temp.Errors).Select(temp => temp.ErrorMessage);
                return View(loginDTO);
            }

            var result = await _signInManager.PasswordSignInAsync(loginDTO.Email, loginDTO.Password, isPersistent: false, lockoutOnFailure: false);

            if (result.Succeeded)
            {
                ApplicationUser user = await _userManager.FindByEmailAsync(loginDTO.Email);
                if (user != null)
                {
                    if (await _userManager.IsInRoleAsync(user, UserTypeOptions.Admin.ToString()))
                    {
                        return RedirectToAction("Index", "Home", new { area = "Admin" });
                    }
                }

                if (!string.IsNullOrEmpty(ReturnURL) && Url.IsLocalUrl(ReturnURL))
                {
                    return LocalRedirect(ReturnURL);
                }
                return Redirect("/");
            }

            ViewBag.Errors = new List<string> { "Invalid email or password" }; ;
            return View(loginDTO);
        }

        [Route("logout")]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return Redirect("/");
        }

        private async Task<bool> IsEmailAlreadyRegistered(string email)
        {
            return (await _userManager.FindByEmailAsync(email)) != null;
        }
    }
}
