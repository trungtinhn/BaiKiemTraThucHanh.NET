using BTThucHanh2.Models;
using Microsoft.AspNetCore.Mvc;

namespace BTThucHanh2.Controllers
{
    public class AccountController : Controller
    {
        QlbanVaLiContext db = new QlbanVaLiContext();
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Login()
        {
            if (HttpContext.Session.GetString("UserName") == null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        [HttpPost]
        public IActionResult Login(TUser user)
        {
            if (HttpContext.Session.GetString("UserName") == null)
            {
                var u = db.TUsers.Where(x => x.Username.Equals(user.Username) && x.Password.Equals(user.Password)).FirstOrDefault();
                if (u != null)
                {
                    HttpContext.Session.SetString("UserName", u.Username.ToString());
                    return RedirectToAction("Index", "Home");
                }
            }
            return View();
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(TUser user, string confirmPassword)
        {
            // Kiểm tra nếu tên đăng nhập đã tồn tại
            if (db.TUsers.Any(x => x.Username == user.Username))
            {
                ViewBag.Error = "Tên đăng nhập đã tồn tại";
                return View();
            }

            // Kiểm tra nếu mật khẩu và xác nhận mật khẩu khớp nhau
            if (user.Password != confirmPassword)
            {
                ViewBag.Error = "Mật khẩu và xác nhận mật khẩu không khớp";
                return View();
            }

            // Nếu hợp lệ, thêm người dùng mới vào cơ sở dữ liệu
            db.TUsers.Add(user);
            db.SaveChanges();

            // Chuyển hướng người dùng sang trang đăng nhập sau khi đăng ký thành công
            return RedirectToAction("Login", "Account");
        }
    }
}
