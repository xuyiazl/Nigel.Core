using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Nigel.WebTests.Models;
using Nigel.Core.Razors;
using Nigel.Helpers;
using Nigel.Drawing;
using Nigel.Core.Filters;
using Nigel.Core.HttpFactory;
using Nigel.Core.Extensions;
using System.Threading;

namespace Nigel.WebTests.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IHttpService _httpService;

        public HomeController(ILogger<HomeController> logger, IHttpService httpService)
        {
            _logger = logger;
            _httpService = httpService;
        }

        [NoCache]
        [Route("{id}")]
        [RazorHtmlStatic(Template = "/static/{controller}/{action}-{id}.html")]
        public async Task<IActionResult> Index(int id, CancellationToken cancellationToken)
        {
            var url = UrlArguments.Create("test", $"/api/CommentsLive/GetPaged")
                 .Add("aid", 1539)
                 .Add("commentId", 0)
                 .Add("pageSize", 10000);

            var res = await _httpService.GetAsync<ReturnModel>(url, cancellationToken);

            return View(res);
        }

        public async Task<IActionResult> IndexView()
        {
            var url = UrlArguments.Create("test", $"/api/CommentsLive/GetPaged")
                 .Add("aid", 1539)
                 .Add("commentId", 0)
                 .Add("pageSize", 10000);

            var res = await _httpService.GetAsync<ReturnModel>(url);

            return View(res);
        }

        [RazorHtml(Path = "/static/home/privacy.html", ViewName = "Privacy")]
        public IActionResult Privacy()
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
