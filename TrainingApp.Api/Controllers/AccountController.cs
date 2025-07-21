using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TrainingApp.Application.DTO;
using TrainingApp.Application.DTO;
using TrainingApp.Application.Services;
using TrainingApp.Core.Interfaces.Services;
using TrainingApp.Infrastructure.Identity;

namespace TrainingApp.Api.Controllers
{
    [Route("[controller]/[action]")]
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ITrainerService _trainerService;
        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, TrainerService trainerService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _trainerService = trainerService;
        }

        [HttpGet]
        public IActionResult Register()
        {

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterDTO registerDto)
        {
            if (!ModelState.IsValid)
            {
                return View(registerDto);
            }

            ApplicationUser user = new ApplicationUser()
            {
                Email = registerDto.Email,
                UserName = registerDto.Email,
            };

            IdentityResult result = await _userManager.CreateAsync(user, registerDto.Password);
            if (result.Succeeded)
            {
                try
                {
                    await _trainerService.AddTrainer(user.Id, user.Email);
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction(nameof(HomeController.Index), "Home");
                }
                catch (Exception ex)
                {
                    // Jakby coś się wywróciło to tu robie rollback
                    await _userManager.DeleteAsync(user);
                    ModelState.AddModelError("Register", "Błąd przy tworzeniu trenera: " + ex.Message);
                    return View(registerDto);
                }
            }
            else
            {
                foreach (IdentityError error in result.Errors)
                {
                    ModelState.AddModelError("Register", error.Description);
                }
                return View(registerDto);
            }
        }

        [HttpGet]
        public IActionResult Login()
        {

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginDTO loginDto)
        {
            if (!ModelState.IsValid) 
            {
                return View(loginDto);
            }

           var result =   await _signInManager.PasswordSignInAsync(loginDto.Email, loginDto.Password, false, true);

            if(result.Succeeded)
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }

            else
            {
                ModelState.AddModelError("Login", "Invalid email or password");
                return View();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}