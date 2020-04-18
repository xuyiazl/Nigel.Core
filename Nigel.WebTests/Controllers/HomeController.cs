﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Nigel.Core.Filters;
using Nigel.Core.HttpFactory;
using Nigel.Core.Razors;
using Nigel.Core.Uploads;
using Nigel.Core.Uploads.Params;
using Nigel.Drawing;
using Nigel.Helpers;
using Nigel.Webs;
using Nigel.WebTests.Models;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Nigel.Core.Redis;
using Nigel.WebTests.Data.DbService;
using Nigel.Paging;
using Nigel.Json;
using MessagePack;
using System;

namespace Nigel.WebTests.Controllers
{
    [MessagePackObject]
    public class User
    {
        [Key(0)]
        public int Id { get; set; }
        [Key(1)]
        public string Name { get; set; }
        [Key(2)]
        public DateTime CreateTime { get; set; }
    }
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IHttpService _httpService;

        private readonly IRedisService _redisService;
        private readonly IDbAdminUsersServiceProvider _dbAdminUsersServiceProvider;
        /// <summary>
        /// 文件上传服务
        /// </summary>
        private IFileUploadService _fileUploadService;

        public HomeController(ILogger<HomeController> logger, IDbAdminUsersServiceProvider dbAdminUsersServiceProvider, IHttpService httpService, IFileUploadService fileUploadService, IRedisService redisService)
        {
            _logger = logger;
            _httpService = httpService;
            _fileUploadService = fileUploadService;
            _redisService = redisService;
            _dbAdminUsersServiceProvider = dbAdminUsersServiceProvider;

        }

        public async Task<IActionResult> Index(CancellationToken cancellationToken)
        {
            var url = UrlArguments.Create("msgpack", "home/getuser");

            var res = await _httpService.GetMsgPackAsync<User>(url, cancellationToken);

            var postUrl = UrlArguments.Create("msgpack", "home/postuser");

            var res1 = await _httpService.PostMsgPackAsync<User, User>(postUrl, res, cancellationToken);

            var url1 = UrlArguments.Create("test", $"/api/CommentsLive/GetPaged")
                        .Add("aid", 1539)
                        .Add("commentId", 0)
                        .Add("pageSize", 10000);

            var res2 = await _httpService.GetAsync<ReturnModel>(url1, cancellationToken);

            return View(res2);
        }

        //[NoCache]
        //[Route("{id}")]
        //[RazorHtmlStatic(Template = "/static/{controller}/{action}-{id}.html")]
        //public async Task<IActionResult> Index(int id, CancellationToken cancellationToken)
        //{
        //var url = UrlArguments.Create("test", $"/api/CommentsLive/GetPaged")
        //     .Add("aid", 1539)
        //     .Add("commentId", 0)
        //     .Add("pageSize", 10000);

        //var res = await _httpService.GetAsync<ReturnModel>(url, cancellationToken);

        //    return View(res);
        //}

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

        [HttpPost]
        public async Task<IActionResult> Upload(IFormFile formFile, CancellationToken cancellationToken)
        {
            var param = new SingleFileUploadParam()
            {
                Request = Request,
                FormFile = formFile,
                RootPath = Web.WebRootPath,
                Module = "Test",
                Group = "Logo"
            };

            var result = await _fileUploadService.UploadAsync(param, cancellationToken);

            return View(result);
        }

        [HttpPost]
        public async Task<IActionResult> UploadMulit(IFormCollection formCollection, CancellationToken cancellationToken)
        {
            FormFileCollection filelist = (FormFileCollection)formCollection.Files;

            var param = new MultipleFileUploadParam()
            {
                Request = Request,
                FormFiles = filelist.ToList(),
                RootPath = Web.WebRootPath,
                Module = "Test",
                Group = "Logo"
            };

            var result = await _fileUploadService.UploadAsync(param, cancellationToken);

            return View(result);
        }

        [HttpPost]
        public async Task<IActionResult> UploadImage(IFormFile formFile, CancellationToken cancellationToken)
        {
            var param = new SingleImageUploadParam()
            {
                Request = Request,
                FormFile = formFile,
                RootPath = Web.WebRootPath,
                Module = "Test",
                Group = "Logo",
                ThumbCutMode = ThumbnailMode.Cut,
                Thumbs = new List<string> { "200x300", "400x200" },
            };

            var result = await _fileUploadService.UploadImageAsync(param, cancellationToken);

            return View(result);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}