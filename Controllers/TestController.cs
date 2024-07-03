using IQuizMVC.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace IQuizMVC.Controllers
{
    public class TestController : Controller
    {
        private readonly UserManager<User> _userManeger;

        public TestController(UserManager<User> userManager)
        {
            _userManeger = userManager;

        }

        [Authorize]
        public IActionResult Protected()
        {
            string userId = _userManeger.GetUserId(User);
            return Ok($"this is Protected {userId}");
        }

        [Authorize(Roles ="admin")]
        public IActionResult AdminProtected() {

            return Ok("This is admin Protected");
        }
    }
}
