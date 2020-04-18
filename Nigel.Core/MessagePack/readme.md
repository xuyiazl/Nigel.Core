###MessagePack API输出

Startup注入MessagePackFormatters

```csharp

services
    .AddControllers()
    .AddMessagePackFormatters()

```

在控制器头部添加MessagePackProduces标签

```csharp

[Route("api/[controller]/[Action]")]
[ApiController]
[MessagePackProduces]
public class MessagePackController : ControllerBase
{
    [MessagePackProduces] //或者在API中添加标签头
    public User Get()
    {
        return new User { Id = 1, Name = "test", CreateTime = DateTime.Now };
    }


    [HttpPost]
    public User Add([FromBody] User user)
    {
        user.Name = "哈哈";
        user.CreateTime = DateTime.Now;

        return user;
    }
}

```

请求API
通过使用HttpService请求自动解析

StartUp 注入 HttpService

```csharp

services.AddHttpService<HttpService>("msgpack", "http://localhost:57802");

```

在构造函数中引入 IHttpService

```csharp

private readonly IHttpService _httpService;

public HomeController(IHttpService httpService)
{
    _httpService = httpService;
}

```

实体类

```csharp

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

```

使用并请求相应API

```csharp

var url = UrlArguments.Create("msgpack", "api/messagepack/get");

var res = await _httpService.GetMsgPackAsync<User>(url, cancellationToken);

var postUrl = UrlArguments.Create("msgpack", "api/messagepack/add");

var res1 = await _httpService.PostMsgPackAsync<User, User>(postUrl, res, cancellationToken);

```