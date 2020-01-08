﻿using System.IO;
using Nigel.IO;
using Xunit;
using Xunit.Abstractions;

namespace Nigel.Tests.IO
{
    /// <summary>
    /// 目录操作辅助类测试
    /// </summary>
    public class DirectoryUtilTest:TestBase
    {
        public DirectoryUtilTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void Test_GetFileNames()
        {
            var result = DirectoryHelper.GetFileNames(Directory.GetCurrentDirectory());
            foreach (var item in result)
            {
                Output.WriteLine(item);
            }
        }
    }
}
