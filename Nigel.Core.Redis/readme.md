﻿
### appSettings.json

**新增节点**

```javascript
  "RedisConnection": [
    {
      "ConnectionName": "db-read",
      "ConnectType": "Read",
      "EndPoint": "host",
      "Port": 6379,
      "DefaultDb": "18",
      "Password": "password"
    },
    {
      "ConnectionName": "db-write",
      "ConnectType": "Write",
      "EndPoint": "host",
      "Port": 6379,
      "DefaultDb": "18",
      "Password": "password"
    }
  ]
```

**Conf注册服务**

services.AddRedisService();

**构造函数**

public Class1(IRedisService redisService){

}