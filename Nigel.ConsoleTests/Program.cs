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

            Parallel.For(0, 100, async ndx =>
            {

                using (await _asyncLock.LockAsync())
                {
                    var s = await redisService.StringIncrementAsync("test");
                    Console.WriteLine($"{s}");
                }

                //await RedisLock(redisService, key,async () =>
                //{
                //    var s = await redisService.StringIncrementAsync("test");
                //    Console.WriteLine($"{s}");
                //});
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
