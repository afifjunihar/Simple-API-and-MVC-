using MVC.Models.ViewModels;
using MVC.Repository.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Net;
using System.Security.Principal;
using MVC.Models;
using Newtonsoft.Json.Linq;

namespace MVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AccountRepository _account;

        public HomeController(ILogger<HomeController> logger, AccountRepository account)
        {
            _logger = logger;
            _account = account;
        }

        [HttpPost]
        [Route("/Login")]
        public async Task<IActionResult> Auth(LoginVM login)
        {
            var jwtToken = await _account.Auth(login);
            var token = jwtToken.Token;
            if (string.IsNullOrEmpty(token))
            {
                if (jwtToken.Message.StartsWith("Password"))
                {
                    return Json(new { Status = HttpStatusCode.BadRequest, Message = jwtToken.Message });
                }
                return Json(new { Status = HttpStatusCode.NotFound, Message = jwtToken.Message });
            }

            HttpContext.Session.SetString("JWToken", token);
            HttpContext.Session.SetString("UserName", login.EmailOrUsername!);

            return Json(new { Status = HttpStatusCode.OK, Message = "Success" });

		}

		[Authorize]
        [HttpGet]
        [Route("/Logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("index", "home");
        }

        [HttpPost]
        [Route("/Registration")]
        public JsonResult Registration(LoginVM login)
        {
            var result = _account.Register(login);
			if (result.ToString().ToUpper() != "OK")
			{
				return Json(new { Status = HttpStatusCode.BadRequest, Message = "Username Sudah Terdaftar." });
			}
			return Json(result);
        }

        public IActionResult Index()
        {
            ViewData["Token"] = HttpContext.Session.GetString("JWToken");
            return View();
        }

        public IActionResult unauthorized()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

		public IActionResult Register()
		{
			return View();
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
