using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SmartGarage.Common.Exceptions;
using SmartGarage.Common.Models.ViewModels;
using SmartGarage.Data.Models;
using SmartGarage.Services.Contracts;
using SmartGarage.Utilities;

namespace SmartGarage.Controllers
{
    public class AuthController : BaseCustomerController
    {
        private readonly SignInManager<AppUser> signInManager;
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly AzureEmailService emailService;
        private readonly UserManager<AppUser> userManager;
        private readonly IUsersService usersService;

        public AuthController(UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            IWebHostEnvironment webHostEnvironment,
            AzureEmailService emailService,
            IUsersService usersService)
        {
            this.signInManager = signInManager;
            this.webHostEnvironment = webHostEnvironment;
            this.emailService = emailService;
            this.userManager = userManager;
            this.usersService = usersService;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Login()
        {
            if (User.Identity.IsAuthenticated)
            {
                // User is already authenticated, no need to show the login page again
                return await RedirectToCorrectActionBasedOnRole(User.Identity.Name);
            }

            LoginViewModel model = new LoginViewModel();

            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel loginData)
        {
            if (!ModelState.IsValid)
            {
                if (string.IsNullOrEmpty(loginData.Email))
                    ModelState.AddModelError("Email", "Email can not be empty!");
                else if (string.IsNullOrEmpty(loginData.Password))
                {
                    loginData.Password = string.Empty;
                    ModelState.AddModelError("Password", "Password can not be empty!");
                }
                else
                {
                    ModelState.AddModelError("Email", "Invalid login attempt");
                    ModelState.AddModelError("Password", "Invalid login attempt");
                }

                return View(loginData);
            }

            try
            {
                var result = await signInManager.PasswordSignInAsync(loginData.Email, loginData.Password, false, lockoutOnFailure: false);

                if (result.Succeeded)
                {
                    return RedirectToAction("Login");
                }

                ModelState.AddModelError("Password", "Invalid credentials");
                return View(loginData);
            }
            catch (Exception)
            {
                ModelState.AddModelError("Email", "Invalid credentials");
                return View(loginData);
            }
        }

        [Authorize]
        private async Task<IActionResult> RedirectToCorrectActionBasedOnRole(string email)
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

            //await Task.Delay(1000);
            await signInManager.SignOutAsync();
            return View("Login");
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            if (User.Identity.IsAuthenticated)
            {
                await signInManager.SignOutAsync();
                return Redirect("Login");
            }

            return View("Error");
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
                if (!ModelState.IsValid)
                {
                    ModelState.AddModelError("Email", "Error. Please try again.");
                    return View(registertionData);
                }

                if (await usersService.UserWithEmailExists(registertionData.Email))
                {
                    ModelState.AddModelError("Email", "Email is alredy registered.");
                    return View(registertionData);
                }

                var newUser = new AppUser
                {
                    UserName = registertionData.Email,
                    Email = registertionData.Email,
                    EmailConfirmed = true,
                    JoinDate = DateTime.UtcNow,
                };

                var result = await usersService.CreateUser(newUser);

                if (!result.Succeeded)
                {
                    ModelState.AddModelError("Email", "Error. Please try again.");
                    return View(registertionData);
                }

                ViewData["PostRegisterMessage"] = "Registration successful! Please check your email";

                //await signInManager.SignInAsync(newUser, isPersistent: false);
                //var token = await userManager.GenerateEmailConfirmationTokenAsync(newUser);
                //var callbackUrl = Url.Action("Login", "Auth", new { userId = newUser.Id, code = token });


                // NOTE : this might be needed later
                // do not remove for now
                {
                    //// Instead of sending email, return the HTML content
                    //return Content(body, "text/html");

                    //ViewData["PostRegisterMessage"] = "Registration successful! Please check your email";
                    //LoginViewModel model = new LoginViewModel();
                }

                //ConfirmEmailViewModel model = new ConfirmEmailViewModel()
                //{
                //    UserName = newUser.Email,
                //    UserId = newUser.Id,
                //    EmailConfirmToken = token,
                //};

                return View(registertionData);

            }
            catch (EntityNotFoundException ex)
            {
                ModelState.AddModelError("Username", ex.Message);
                return View();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Username", ex.Message);
                return View();
            }
        }

        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail(string userId, string confirmToken)
        {
            // NOTE : need to review validation

            if (userId == null || confirmToken == null)
            {
                return View("Error");
            }

            var user = await userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return View("Error");
            }

            var result = await userManager.ConfirmEmailAsync(user, confirmToken);

            if (result.Succeeded)
            {
                await signInManager.SignInAsync(user, isPersistent: false);

                return View("Login");
            }
            else
            {
                return View("Error");
            }
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPassword()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPassword(string userName)
        {
            try
            {
                var user = await usersService.GetByEmail(userName);

                if (user != null)
                {
                    var token = await userManager.GeneratePasswordResetTokenAsync(user);

                    var resetLink = Url.Action("OnResetPassword", "Auth", new { userName, token }, Request.Scheme);

                    await usersService.ResetPassword(user, resetLink);

                    return View("Login");
                }
                else
                {
                    ModelState.AddModelError("UserName", "Email not found.");
                    return View("ResetPassword");
                }
            }
            catch (EntityNotFoundException ex)
            {
                ModelState.AddModelError("UserName", ex.Message);
                return View();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("UserName", ex.Message);
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

        /// <summary>
        /// 
        /// </summary>
        /// <notes>
        /// Keep a more general exception handling as "ResetPasswordAsync" might throw quite a lof of dfferent exceptions
        /// </notes>
        /// <param name="userResetPasswordData"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> OnResetPassword(UserResetPasswordViewModel userResetPasswordData, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return View(userResetPasswordData);
            }

            try
            {
                var user = await usersService.GetByEmail(userResetPasswordData.UserName);

                var result = await usersService.UpdateResetPassword(user, userResetPasswordData.ResetToken, userResetPasswordData.NewPassword, cancellationToken);

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
                    ViewData["Errors"] = errors;
                    return View(userResetPasswordData);
                }
            }
            catch (EntityNotFoundException ex)
            {
                return View(userResetPasswordData);
            }
            catch (Exception ex)
            {
                return View("Error");
            }
        }
    }
}