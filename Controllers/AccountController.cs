using HeadHunter.Models;
using HeadHunter.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace HeadHunter.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IWebHostEnvironment appEnvironment;
        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager, IWebHostEnvironment appEnvironment)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            this.appEnvironment = appEnvironment;
        }
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                string? imagePath = null;
                if (model.Avatar != null)
                {
                    long size = model.Avatar.Length;
                    if (size > 0)
                    {
                        string filePath = Path.Combine(appEnvironment.WebRootPath, "images", model.Avatar.FileName);
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await model.Avatar.CopyToAsync(stream);
                        }
                        imagePath = $"/images/{model.Avatar.FileName}";
                    }
                }
                var existingUser = await _userManager.FindByEmailAsync(model.Email);
                if (existingUser != null)
                {
                    ModelState.AddModelError("", "Пользователь с такой почтой уже зарегистрирован.");
                    return View(model);
                }
                User user = new User
                {
                    Email = model.Email,
                    UserName = model.Email,
                    Name = model.Name,
                    Avatar = imagePath != null ? imagePath : "https://herrmans.eu/wp-content/uploads/2019/01/765-default-avatar.png",
                    PhoneNumber = model.PhoneNumber
                };
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    if (model.Role == "applicant")
                        await _userManager.AddToRoleAsync(user, "applicant");
                    else
                        await _userManager.AddToRoleAsync(user, "employer");

                    await _signInManager.SignInAsync(user, false);
                    return RedirectToAction("Index", "Home");
                }
                foreach (var error in result.Errors)
                    ModelState.AddModelError(string.Empty, error.Description);
            }
            return View(model);
        }
        [HttpGet]
        public IActionResult Login(string returnUrl = null)
        {
            return View(new LoginViewModel { ReturnUrl = returnUrl });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                User user = await _userManager.FindByEmailAsync(model.Email);
                if (user != null)
                {
                    Microsoft.AspNetCore.Identity.SignInResult result = await _signInManager.PasswordSignInAsync(
                    user,
                    model.Password,
                    model.RememberMe,
                    false
                    );
                    if (result.Succeeded)
                    {
                        if (!string.IsNullOrEmpty(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
                        {
                            return Redirect(model.ReturnUrl);
                        }

                        return RedirectToAction("Index", "Home");
                    }
                }
                ModelState.AddModelError("", "Неправильный логин и (или) пароль");
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LogOff()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}
