using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SmartGarage.Common.Exceptions;
using SmartGarage.Common.Models.ViewModels;
using SmartGarage.Data.Models;
using SmartGarage.Services.Contracts;

namespace SmartGarage.Controllers
{
    public class AuthController : Controller
    {
        private readonly SignInManager<AppUser> signInManager;
        private readonly UserManager<AppUser> userManager;
        private readonly IUsersService usersService;

        public AuthController(UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            IUsersService usersService)
        {
            this.signInManager = signInManager;
            this.userManager = userManager;
            this.usersService = usersService;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Login()
        {
            try
            {
                if (this.User.Identity!.IsAuthenticated)
                {
                    return await RedirectToCorrectActionBasedOnRole();
                }

                var model = new LoginViewModel();

                return View(model);
            }
            catch
            {
                return View("Login", "Auth");
            }
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginViewModel loginData)
        {
            if (!this.ModelState.IsValid)
            {
                if (string.IsNullOrEmpty(loginData.Email))
                    this.ModelState.AddModelError("Email", "Email can not be empty!");
                else if (string.IsNullOrEmpty(loginData.Password))
                {
                    loginData.Password = string.Empty;
                    this.ModelState.AddModelError("Password", "Password can not be empty!");
                }
                else
                {
                    this.ModelState.AddModelError("Email", "Invalid login attempt");
                    this.ModelState.AddModelError("Password", "Invalid login attempt");
                }

                return View(loginData);
            }

            try
            {
                var result = await this.signInManager.PasswordSignInAsync(loginData.Email, loginData.Password, false, lockoutOnFailure: false);

                if (result.Succeeded)
                {
                    return RedirectToAction("Login");
                }

                this.ModelState.AddModelError("Password", "Invalid credentials");
                return View(loginData);
            }
            catch (Exception)
            {
                this.ModelState.AddModelError("Email", "Invalid credentials");
                return View(loginData);
            }
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            if (this.User.Identity!.IsAuthenticated)
            {
                await this.signInManager.SignOutAsync();
            }

            return Redirect("Login");
        }


        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Register(RegisterViewModel registertionData)
        {
            try
            {
                if (!this.ModelState.IsValid)
                {
                    this.ModelState.AddModelError("Email", "Error. Please try again.");
                    return View(registertionData);
                }

                if (await this.usersService.UserWithEmailExists(registertionData.Email!))
                {
                    this.ModelState.AddModelError("Email", "Email is alredy registered.");
                    return View(registertionData);
                }

                var newUser = new AppUser
                {
                    UserName = registertionData.Email,
                    Email = registertionData.Email,
                    EmailConfirmed = true,
                    JoinDate = DateTime.UtcNow,
                };

                var result = await this.usersService.CreateUser(newUser);

                if (!result.Succeeded)
                {
                    this.ModelState.AddModelError("Email", "Error. Please try again.");
                    return View(registertionData);
                }

                this.ViewData["PostRegisterMessage"] = "Registration successful! Please check your email";

                return View(registertionData);

            }
            catch (EntityNotFoundException ex)
            {
                this.ModelState.AddModelError("Username", ex.Message);
                return View();
            }
            catch (Exception ex)
            {
                this.ModelState.AddModelError("Username", ex.Message);
                return View();
            }
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ResetPassword()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPassword(string userName)
        {
            try
            {
                var user = await this.usersService.GetByEmail(userName);

                if (user != null)
                {
                    var token = await this.userManager.GeneratePasswordResetTokenAsync(user);

                    var resetLink = Url.Action("OnResetPassword", "Auth", new { userName, token }, Request!.Scheme);

                    await this.usersService.ResetPassword(user, resetLink!);

                    return View("Login");
                }
                else
                {
                    this.ModelState.AddModelError("UserName", "Email not found.");
                    return View("ResetPassword");
                }
            }
            catch (EntityNotFoundException ex)
            {
                this.ModelState.AddModelError("UserName", ex.Message);
                return View();
            }
            catch (Exception ex)
            {
                this.ModelState.AddModelError("UserName", ex.Message);
                return View();
            }
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult OnResetPassword(string userName, string token)
        {
            var userResetPasswordViewModel = new UserResetPasswordViewModel
            {
                UserName = userName,
                ResetToken = token,
            };

            return View(userResetPasswordViewModel);
        }

        /// <notes>
        /// Keep a more general exception handling as "ResetPasswordAsync" might throw quite a lof of dfferent exceptions
        /// </notes>
        /// <param name="userResetPasswordData"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> OnResetPassword(UserResetPasswordViewModel userResetPasswordData, CancellationToken cancellationToken)
        {
            if (!this.ModelState.IsValid)
            {
                return View(userResetPasswordData);
            }

            try
            {
                var user = await this.usersService.GetByEmail(userResetPasswordData.UserName);

                var result = await this.usersService.UpdateResetPassword(user, userResetPasswordData.ResetToken, userResetPasswordData.NewPassword, cancellationToken);

                if (result.Succeeded)
                {
                    return RedirectToAction("Login");
                }
                else
                {
                    List<string> errors = new List<string>();
                    foreach (var error in result.Errors)
                    {
                        errors.Add(error.Description);
                    }
                    this.ViewData["Errors"] = errors;
                    return View(userResetPasswordData);
                }
            }
            catch (EntityNotFoundException)
            {
                return View(userResetPasswordData);
            }
            catch (Exception)
            {
                return View(userResetPasswordData);
            }
        }


        [Authorize]
        private async Task<IActionResult> RedirectToCorrectActionBasedOnRole()
        {
            if (User.IsInRole("Customer"))
            {
                return RedirectToAction("DisplayAll", "Visits");
            }

            if (User.IsInRole("Employee"))
            {
                return RedirectToAction("Index", "Home", new { area = "Employee" });
            }

            if (User.IsInRole("Admin"))
            {
                return RedirectToAction("Index", "Home", new { area = "Admin" });
            }

            await this.signInManager.SignOutAsync();
            return View("Login");
        }
    }
}