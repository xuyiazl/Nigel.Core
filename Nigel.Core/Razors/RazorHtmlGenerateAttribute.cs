using System;
using System.IO;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Nigel.Helpers;
using Nigel.Extensions;
using Nigel.Core.Extensions;
using System.Threading.Tasks;
using System.Text;

namespace Nigel.Core.Razors
{
    /// <summary>
    /// Razor生成Html静态文件（保存目录为wwwroot）
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public class RazorHtmlGenerateAttribute : ActionFilterAttribute
    {
        /// <summary>
        /// 路径模板，范例：static/{area}/{controller}/{action}.component.html
        /// </summary>
        public string Template { get; set; }

        /// <summary>
        /// 是否部分视图，默认：false
        /// </summary>
        public bool IsPartialView { get; set; } = false;

        /// <summary>
        /// 动作执行之前 before
        /// </summary>
        /// <param name="context"></param>
        /// <param name="next"></param>
        /// <returns></returns>
        public override Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            return base.OnActionExecutionAsync(context, next);
        }

        /// <summary>
        /// 动作执行之后 after
        /// </summary>
        /// <param name="context"></param>
        public override void OnActionExecuted(ActionExecutedContext context)
        {
            base.OnActionExecuted(context);
        }

        /// <summary>
        /// 结果执行之前 before
        /// </summary>
        /// <param name="context"></param>
        /// <param name="next"></param>
        /// <returns></returns>
        public override async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
        {
            await base.OnResultExecutionAsync(context, next);
        }

        /// <summary>
        /// 结果执行之后 after
        /// </summary>
        /// <param name="context"></param>
        public override void OnResultExecuted(ResultExecutedContext context)
        {
            WriteHtml(context, (ViewResult)context.Result);

            base.OnResultExecuted(context);
        }

        /// <summary>
        /// 写HTML
        /// </summary>
        /// <param name="context"></param>
        /// <param name="viewResult"></param>
        protected void WriteHtml(ResultExecutedContext context, ViewResult viewResult)
        {
            var _logger = Web.HttpContext.RequestServices.GetService<ILogger<RazorHtmlGenerateAttribute>>();
            try
            {
                var html = viewResult?.ToHtml(context.HttpContext, IsPartialView);

                if (string.IsNullOrWhiteSpace(html)) return;

                var path = Nigel.Helpers.Common.GetWebRootPath(GetPath(context));

                Save(html, path);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "生成html静态文件失败");
            }
        }

        /// <summary>
        /// 获取实际地址，路由参数进行替换
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        protected string GetPath(ResultExecutedContext context)
        {
            var path = Template;
            foreach (var route in context.GetRouteValues())
                path = path.Replace("{" + route.Key + "}", route.Value.SafeString());
            return path.ToLower();
        }

        /// <summary>
        /// 这里采用新创建临时文件 再拷贝 ， 避免引起IO独占的问题  
        /// </summary>
        /// <param name="html"></param>
        /// <param name="path"></param>
        protected void Save(string html, string path)
        {
            var tmpPath = $"{path}.tmp";

            FileInfo fi = new FileInfo(tmpPath);

            if (!fi.Directory.Exists)
                fi.Directory.Create();

            using (var fs = fi.OpenWrite())
                fs.Write(html, Encoding.UTF8);

            fi.CopyTo(path, true);

            fi.Delete();
        }
    }
}
