using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using Nigel.Core.Redis;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Threading;
using Nigel.Helpers;
using Nigel.Threading.Asyncs;
using Nigel.Extensions;
using RedLockNet.SERedis;
using RedLockNet.SERedis.Configuration;
using System.Net;
using RedLockNet;
using System.Linq;

namespace Nigel.ConsoleTests
{
    class Program
    {
        static void Main(string[] args)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.Test.json").Build();

            var services = new ServiceCollection();

            services.AddRedisService();

            IRedisService redisService = new RedisServiceProvider(configuration);

            string key = "key";
            string token = Environment.MachineName;

            AsyncLock _asyncLock = new AsyncLock();
            int stockCount = 10;

            var stackConnects = new List<StackExchangeConnectionSettings>();
            configuration.GetSection("StackExchangeConnectionSettings").Bind(stackConnects);
            var endPoints = new List<RedLockEndPoint>();
            stackConnects.Where(c => c.ConnectType == ConnectTypeEnum.Read).ForEach(item =>
            {
                endPoints.Add(new RedLockEndPoint()
                {
                    EndPoint = new DnsEndPoint(item.EndPoint, item.Port.ToInt()),
                    RedisDatabase = item.DefaultDb,
                    Password = item.Password
                });
            });

            IDistributedLockFactory _distributedLockFactory = RedLockFactory.Create(endPoints);

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
                            // 模拟执行的逻辑代码花费的时间
                            await Task.Delay(new Random().Next(200, 500));
                            if (stockCount > 0)
                                stockCount--;
                            Console.WriteLine($"{stockCount}：{DateTime.Now}");
                        }
                        else
                        {
                            Console.WriteLine($"获取锁失败：{DateTime.Now}");
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
                    //    await Task.Delay(new Random().Next(100, 500));
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
