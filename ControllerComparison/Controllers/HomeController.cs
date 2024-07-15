using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

#if NETFRAMEWORK
using System.Web;
using System.Web.Mvc;
#elif NETCORE
using Microsoft.AspNetCore.Mvc;
using DotNetCore.Models;
#endif

namespace DotNetFramework.Controllers
{
    public class HomeController : Controller
    {
#if NETCORE
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }
#endif

        public ActionResult Index()
        {
#if NETFRAMEWORK

            // カスタムヘッダーを追加
            Response.AddHeader("X-Custom-Header", "This is a custom header value");

            // 現在のURLを取得
            string currentUrl = Request.Url.ToString();
#elif NETCORE
            // カスタムヘッダーを追加
            Response.Headers.Append("X-Custom-Header", "This is a custom header value");

            // 現在のURLを取得
            string currentUrl = $"{Request.Scheme}://{Request.Host}{Request.Path}{Request.QueryString}";
#endif
            // ビューに渡す
            ViewBag.CurrentUrl = currentUrl;

            return View();
        }

#if NETFRAMEWORK
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
#endif

#if NETCORE
        public ActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public ActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
#endif

        // POST: Home/SaveUserName
        [HttpPost]
        public ActionResult SaveUserName(string userName)
        {
#if NETFRAMEWORK
            // セッションにユーザー名を保存
            Session["UserName"] = userName;
#elif NETCORE
            // セッションにユーザー名を保存
            HttpContext.Session.SetString("UserName", userName);
#endif
            return RedirectToAction("Display");
        }

        // GET: Home/Display
        public ActionResult Display()
        {
#if NETFRAMEWORK
            // セッションからユーザー名を取得
            var userName = Session["UserName"]?.ToString();
#elif NETCORE
            // セッションからユーザー名を取得
            var userName = HttpContext.Session.GetString("UserName");
#endif
            ViewBag.UserName = userName;
            return View();
        }

        // Ajaxリクエストと通常リクエストを処理するアクションメソッド
        public ActionResult GetMessage()
        {
#if NETFRAMEWORK
            if (Request.IsAjaxRequest())
            {
                return Json(new { message = "これはAjaxリクエストからのメッセージです。" }, JsonRequestBehavior.AllowGet);
            }
#elif NETCORE
            if (Request.Headers.XRequestedWith == "XMLHttpRequest")
            {
                return Json(new { message = "これはAjaxリクエストからのメッセージです。" });
            }
#endif
            ViewBag.Message = "これは通常リクエストからのメッセージです。";
            return View();
        }

        public ActionResult QueryDetails()
        {
#if NETFRAMEWORK
            // クエリパラメータを取得
            string name = Request.Params.Get("name");
            string age = Request.Params.Get("age");
#elif NETCORE
            // Request.Queryを使用してクエリパラメータを取得
            string name = Request.Query["name"];
            string age = Request.Query["age"];
#endif

            // ViewBagを使ってビューにデータを渡す
            ViewBag.Name = name ?? "ゲスト";
            ViewBag.Age = age ?? "不明";

            // QueryDetailsビューを表示
            return View();
        }

        public ActionResult ParseUrl()
        {
#if NETFRAMEWORK
            string rawUrl = Request.RawUrl;
            string[] urlSegments = rawUrl.Split(new[] { '/' }, System.StringSplitOptions.RemoveEmptyEntries);

            // ViewBag を使ってデータを渡す
            ViewBag.UrlSegments = urlSegments;
#elif NETCORE
            string rawUrl = Request.Path + Request.QueryString.Value;
            string[] urlSegments = rawUrl.Split(new[] { '/' }, System.StringSplitOptions.RemoveEmptyEntries);

            // ViewData を使ってデータを渡す
            ViewData["UrlSegments"] = urlSegments;
#endif

            return View();
        }
    }
}