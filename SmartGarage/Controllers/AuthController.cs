using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SmartGarage.Models;
using SmartGarage.Services.Services.Contracts;
using SmartGarage.Utilities;
using SmartGarage.WebAPI.Models;

namespace SmartGarage.Controllers
{
    public class AuthController : Controller
    {
        private readonly EmailService emailService;
        private readonly SignInManager<AppUser> signInManager;
        private readonly UserManager<AppUser> userManager;
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly IUsersService usersService;

        public AuthController(ILogger<HomeController> logger,
            EmailService emailService,
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            IWebHostEnvironment webHostEnvironment,
            IUsersService usersService)
        {
            this.signInManager = signInManager;
            this.userManager = userManager;
            this.emailService = emailService;
            this.webHostEnvironment = webHostEnvironment;
            this.usersService = usersService;
        }

        [HttpGet]
        public IActionResult Login()
        {
            LoginViewModel model = new LoginViewModel();

            return View(model);
        }

        [HttpPost]
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
                    // Successfully authenticated
                    return RedirectToAction("Index", "Home");
                }

                ModelState.AddModelError("Password", "Invalid credentials");
                return View(loginData);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Email", "Invalid credentials.");
                return View(loginData);
            }
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel registertionData)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    ModelState.AddModelError("Email", "Error. Please try again.");
                    return View();
                }

                var userExists = await usersService.UserWithEmailExists(registertionData.Email);

                if (await usersService.UserWithEmailExists(registertionData.Email))
                {
                    ModelState.AddModelError("Email", "Email is alredy registered.");
                    return View();
                }

                var newUser = new AppUser
                {
                    UserName = registertionData.Email,
                    Email = registertionData.Email,
                    EmailConfirmed = false,
                    JoinDate = DateTime.UtcNow,
                };

                var result = await usersService.Create(newUser);

                if (result.Succeeded)
                {
                    ViewData["PostRegisterMessage"] = "Registration successful! Please check your email";

                    await signInManager.SignInAsync(newUser, isPersistent: false);
                    var token = await userManager.GenerateEmailConfirmationTokenAsync(newUser);
                    var callbackUrl = Url.Action("Login", "Auth", new { userId = newUser.Id, code = token });

                    // await UserManager.SendEmailAsync(user.Id, "Confirm your account", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");  
                    string body = string.Empty;

                    // Then use it to get the content root path
                    string filePath = Path.Combine(webHostEnvironment.ContentRootPath, "Views/MailTemplate/AccountConfirmation.html");

                    using (StreamReader reader = new StreamReader(filePath))
                    {
                        body = reader.ReadToEnd();
                    }
                    body = body.Replace("{ConfirmationLink}", callbackUrl);
                    body = body.Replace("{UserName}", newUser.Email);

                    // NOTE : this might be needed later
                    // do not remove for now
                    {
                        //// Instead of sending email, return the HTML content
                        //return Content(body, "text/html");

                        //ViewData["PostRegisterMessage"] = "Registration successful! Please check your email";
                        //LoginViewModel model = new LoginViewModel();
                    }

                    ConfirmEmailViewModel model = new ConfirmEmailViewModel()
                    {
                        UserName = newUser.Email,
                        UserId = newUser.Id,
                        EmailConfirmToken = token,
                    };
                    return View("ConfirmEmail", model);
                }

                return View("Error");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Username", ex.Message);
                return View();
                //return RedirectToAction("Register", "Auth", new { ex.Message });
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

    }
}
