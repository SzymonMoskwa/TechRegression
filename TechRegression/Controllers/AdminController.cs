using Microsoft.AspNetCore.Mvc;

namespace TechRegression.Controllers
{
    public class AdminController : Controller
    {
        private readonly IConfiguration _configuration;

        public AdminController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        public IActionResult Login()
        {
            if (HttpContext.Session.GetString("AdminLoggedIn") == "true")
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(string username, string password)
        {
            var adminUser = _configuration["AdminSettings:Username"];
            var adminPasswordHash = _configuration["AdminSettings:PasswordHash"];

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                ViewBag.ErrorMessage = "Dane logowania nie mogą być puste!";
                return View();
            }

            var hasher = new Microsoft.AspNetCore.Identity.PasswordHasher<object>();

            try
            {
                var verificationResult = hasher.VerifyHashedPassword(null, adminPasswordHash, password);

                if (username == adminUser && verificationResult == Microsoft.AspNetCore.Identity.PasswordVerificationResult.Success)
                {
                    HttpContext.Session.SetString("AdminLoggedIn", "true");
                    return RedirectToAction("Index", "Home");
                }
            }
            catch (Exception)
            {
                // jeśli jakimś cudem jeszcze będzie jakiś błąd to wszystko nie eksploduje
            }

            ViewBag.ErrorMessage = "Nieprawidłowe dane logowania!";
            return View();
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }
    }
}