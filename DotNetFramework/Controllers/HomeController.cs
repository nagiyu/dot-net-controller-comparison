using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DotNetFramework.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            // カスタムヘッダーを追加
            Response.AddHeader("X-Custom-Header", "This is a custom header value");

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        // POST: Home/SaveUserName
        [HttpPost]
        public ActionResult SaveUserName(string userName)
        {
            // セッションにユーザー名を保存
            Session["UserName"] = userName;
            return RedirectToAction("Display");
        }

        // GET: Home/Display
        public ActionResult Display()
        {
            // セッションからユーザー名を取得
            var userName = Session["UserName"]?.ToString();
            ViewBag.UserName = userName;
            return View();
        }
    }
}