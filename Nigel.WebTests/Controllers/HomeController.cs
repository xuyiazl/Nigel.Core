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
        [RazorHtmlGenerate(Template = "/static/home/{id}.html")]
        [Route("{id}")]

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


    /// <summary>
    /// 返回数据结构
    /// </summary>
    public class ReturnModel
    {
        private string _code = "-1";
        private string _subCode = "0";
        private long _requestLine = -1;
        private string _message = "service exception";
        private object _bodyMessage = "";


        /// <summary>
        /// 
        /// </summary>
        public ReturnModel()
        {
            code = _code;
            subCode = _subCode;
            message = _message;
        }
        /// <summary>
        /// 
        /// </summary>
        public ReturnModel(bool boolCode)
        {
            code = "0";
            subCode = _subCode;
            message = "";
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="msg"></param>
        public ReturnModel(string msg)
        {
            code = _code;
            subCode = _subCode;
            message = msg;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="bodyMsg"></param>
        public ReturnModel(string msg, string bodyMsg)
        {
            message = msg;
            bodyMessage = bodyMsg;
        }

        /// <summary>
        /// 请求状态码：0 成功，-1 失败
        /// </summary>
        public string code
        {
            get { return _code; }
            set { _code = value; }
        }
        /// <summary>
        /// 业务状态码
        /// </summary>
        public string subCode
        {
            get { return _subCode; }
            set { _subCode = value; }
        }
        /// <summary>
        /// 执行时间（毫秒）
        /// </summary>
        public long requestLine
        {
            get { return _requestLine; }
            set { _requestLine = value; }
        }
        /// <summary>
        /// 提示消息
        /// </summary>
        public string message
        {
            get { return _message; }
            set { _message = value; }
        }
        /// <summary>
        /// json数据
        /// </summary>
        public object bodyMessage
        {
            get { return _bodyMessage; }
            set { _bodyMessage = value; }
        }
    }
}
