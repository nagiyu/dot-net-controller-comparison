using DotNetCore.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace DotNetCore.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            // カスタムヘッダーを追加
            Response.Headers.Append("X-Custom-Header", "This is a custom header value");

            // 現在のURLを取得
            string currentUrl = $"{Request.Scheme}://{Request.Host}{Request.Path}{Request.QueryString}";
            // ビューに渡す
            ViewBag.CurrentUrl = currentUrl;

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        // POST: Home/SaveUserName
        [HttpPost]
        public IActionResult SaveUserName(string userName)
        {
            // セッションにユーザー名を保存
            HttpContext.Session.SetString("UserName", userName);
            return RedirectToAction("Display");
        }

        // GET: Home/Display
        public IActionResult Display()
        {
            // セッションからユーザー名を取得
            var userName = HttpContext.Session.GetString("UserName");
            ViewBag.UserName = userName;
            return View();
        }

        // Ajaxリクエストと通常リクエストを処理するアクションメソッド
        public IActionResult GetMessage()
        {
            if (Request.Headers.XRequestedWith == "XMLHttpRequest")
            {
                return Json(new { message = "これはAjaxリクエストからのメッセージです。" });
            }
            ViewBag.Message = "これは通常リクエストからのメッセージです。";
            return View();
        }

        public IActionResult QueryDetails()
        {
            // Request.Queryを使用してクエリパラメータを取得
            string name = Request.Query["name"];
            string age = Request.Query["age"];

            // ViewBagを使ってビューにデータを渡す
            ViewBag.Name = name ?? "ゲスト";
            ViewBag.Age = age ?? "不明";

            // QueryDetailsビューを表示
            return View();
        }

        public IActionResult ParseUrl()
        {
            string rawUrl = Request.Path + Request.QueryString.V;
            string[] urlSegments = rawUrl.Split(new[] { '/' }, System.StringSplitOptions.RemoveEmptyEntries);

            // ViewData を使ってデータを渡す
            ViewData["UrlSegments"] = urlSegments;

            return View();
        }

    }
}
