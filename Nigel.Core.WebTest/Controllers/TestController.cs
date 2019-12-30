using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Nigel.Core.Collection;
using Nigel.Core.HttpFactory;

namespace Nigel.Core.WebTest.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TestController : ControllerBase
    {
        private readonly ILogger<TestController> _logger;
        private readonly IHttpService _httpService;

        public TestController(ILogger<TestController> logger, IHttpService httpService)
        {
            _logger = logger;
            _httpService = httpService;

            logger.LogError("test");
            logger.LogDebug("test");
            logger.LogInformation("test");
            logger.LogWarning("test");
            logger.LogCritical("test");
            logger.LogTrace("test");
        }

        [HttpGet]
        public async Task<ReturnModel> Get(CancellationToken cancellationToken)
        {
            var url = UrlArguments.Create("testmswebapi", $"api/CommentsLive/GetPaged")
                 .Add("aid", 1539)
                 .Add("commentId", 0)
                 .Add("pageSize", 10000);

            return await _httpService.GetAsync<ReturnModel>(url, cancellationToken);
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
