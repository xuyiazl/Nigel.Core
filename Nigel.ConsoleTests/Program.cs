using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using Nigel.Core.Redis;
using Nigel.Json;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using Nigel.Helpers;
using Nigel.Threading.Asyncs;
using Nigel.Extensions;
using Nigel.Configs;
using RedLockNet.SERedis;
using RedLockNet.SERedis.Configuration;
using System.Net;
using RedLockNet;
using MessagePack;
using System.Linq;

namespace Nigel.ConsoleTests
{
    [MessagePackObject]
    public class User
    {
        [Key(0)]
        public string Id { get; set; }
        [Key(1)]
        public string Name { get; set; }
        [Key(2)]
        public Dictionary<string, object> Dict { get; set; }
    }

    class Program
    {
        static void Main(string[] args)
        {
            User user = new User
            {
                Id = "1",
                Name = "张三1111"
            };
            user.Dict = new Dictionary<string, object>();
            user.Dict.Add("Id", 1);
            user.Dict.Add("Name", "张三");

            var jj = user.ToMsgPackJson();

            var _dict = user.ToMsgPackBytes();

            var _dict1 = _dict.ToMsgPackObject<User>();

            var _user1 = user.ToMsgPackBytes().ToMsgPackObject<User>();

            var _user2 = user.ToMsgPackJson().ToMsgPackBytesFromJson().ToMsgPackObject<User>();

            //var dict = new Dictionary<string, object>();
            //dict.Add("Id", 1);
            //dict.Add("Name", "张三");





            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            var configuration = ConfigHelper.GetJsonConfig("appsettings.Test.json");

            IRedisService redisService = new RedisServiceProvider(configuration, null);

            string hashId = "User";

            //redisService.HashDelete(hashId, user.Id);

            //redisService.HashSet<User>(hashId, user.Id, user);

            var res = redisService.HashGet<User>(hashId, user.Id, serializer: new MessagePackRedisSerializer());

            string hashId2 = "User2";

            //redisService.HashDelete(hashId2, "2");

            //redisService.HashSet(hashId2, "2", "2");

            var res1 = redisService.HashGet<string>(hashId2, "2", serializer: new MessagePackRedisSerializer());

            string hashId3 = "User3";

            //redisService.HashDelete(hashId3, "3");

            //redisService.HashSet(hashId3, "3", "3");

            var res3 = redisService.HashGet<string>(hashId3, "3", serializer: new MessagePackRedisSerializer());


            Console.Read();



            redisService.StringSet<long>("test", 10);


            var stackConnects = configuration.GetSection<List<StackExchangeConnectionSettings>>("StackExchangeConnectionSettings");

            var endPoints = stackConnects.Where(c => c.ConnectType == ConnectTypeEnum.Read).ForEach(item => new RedLockEndPoint()
            {
                EndPoint = new DnsEndPoint(item.EndPoint, item.Port.ToInt()),
                RedisDatabase = item.DefaultDb,
                Password = item.Password
            });

            IDistributedLockFactory _distributedLockFactory = RedLockFactory.Create(endPoints);

            string key = "key";
            string token = Environment.MachineName;

            Parallel.For(0, 100, async ndx =>
                {
                    // resource 锁定的对象
                    // expiryTime 锁定过期时间，锁区域内的逻辑执行如果超过过期时间，锁将被释放
                    // waitTime 等待时间,相同的 resource 如果当前的锁被其他线程占用,最多等待时间
                    // retryTime 等待时间内，多久尝试获取一次
                    using (var redLock = await _distributedLockFactory.CreateLockAsync(
                        resource: key,
                        expiryTime: TimeSpan.FromSeconds(5),
                        waitTime: TimeSpan.FromSeconds(1),
                        retryTime: TimeSpan.FromMilliseconds(20)))
                    {
                        if (redLock.IsAcquired)
                        {
                            var count = redisService.StringGet<long>("test");

                            if (count > 0)
                                await redisService.StringIncrementAsync("test", -1);

                            Console.WriteLine($"{count}：{DateTime.Now} {Thread.ThreadId}");
                        }
                        else
                        {
                            Console.WriteLine($"获取锁失败：{DateTime.Now} {Thread.ThreadId}");
                        }
                    }

                    //using (await _asyncLock.LockAsync())
                    //{
                    //    var s = await redisService.StringIncrementAsync("test");
                    //    Console.WriteLine($"{s}");
                    //}

                    //if (!await redisService.LockTakeAsync(key, token, 10))
                    //{
                    //    return;
                    //}

                    //try
                    //{
                    //    // 模拟执行的逻辑代码花费的时间
                    //    await Task.Delay(new Random().Next(100, 500);
                    //    if (stockCount > 0)
                    //    {
                    //        stockCount--;
                    //    }
                    //    //var s = await redisService.StringIncrementAsync("test");
                    //    Console.WriteLine($"{stockCount}");
                    //}
                    //catch (Exception ex)
                    //{
                    //}
                    //finally
                    //{
                    //    await redisService.LockReleaseAsync(key, token);
                    //}
                });

            Console.ReadKey();
        }

        public static async Task RedisLock(IRedisService redisService, string key, Action action, int lockTimeoutSeconds = 10, int replyCount = 20)
        {

            int lockCounter = 0;
            Exception logException = null;

            var lockToken = Id.GuidGenerator.Create().ToString();
            var lockName = key + "_lock";

            while (lockCounter < replyCount)
            {
                if (!await redisService.LockTakeAsync(lockName, lockToken, lockTimeoutSeconds))
                {
                    lockCounter++;
                    //System.Threading.Thread.Sleep(50);
                    continue;
                }

                try
                {
                    action.Invoke();
                }
                catch (Exception ex)
                {
                    logException = ex;
                }
                finally
                {
                    await redisService.LockReleaseAsync(lockName, lockToken);
                }
                break;
            }

            if (lockCounter >= replyCount || logException != null)
            {
                //log it
            }
        }
    }
}
