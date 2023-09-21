using CitizenManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using BCrypt.Net;
using MongoDB.Driver;

namespace CitizenManagement.Controllers
{
    public class LoginController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(string UserName, string Password)
        {
            ViewData.Clear();
            if (UserName == null || Password == null || UserName == "" || Password == "")
            {
                return View();
            }

            var check = DB.get<Account>("account").AsQueryable().Where(x => x.UserName == UserName).FirstOrDefault();

            bool isPasswordMatch = BCrypt.Net.BCrypt.Verify(Password, check.Password);

            if (check == null || isPasswordMatch == false)
            {
                Response.StatusCode = 406;
                ViewData["Error"] = "Đã xảy ra sự cố khi đăng nhập. Hãy kiểm tra tên tài khoản và mật khẩu!!!";
                return View();
            }

            else
            {

                DateTime currentTime = DateTime.Now;
                long timestamp = ((DateTimeOffset)currentTime).ToUnixTimeSeconds();
                var token = JwtService.GenerateToken(new Dictionary<string, object>()
                    {
                        { "Info", check.Info },
                        { "UserName" , check.UserName },
                        { "exp", timestamp + 600 }
                    });
                var x = new HttpCookie("citizenToken", token);
                Response.Cookies.Add(x);
                FormsAuthentication.SetAuthCookie(check.UserName, true);
                Session["infoA"] = check;


                Response.StatusCode = 200;
                return RedirectToAction("Index", "Home");
            }
        }


        public ActionResult Logout()
        {
            var x = new HttpCookie("citizenToken", "");
            Response.Cookies.Set(x);
            Session.Remove("info");
            Session["info"] = null;
            Session.Clear();
            Session.Abandon();
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Login");
        }
    }
}