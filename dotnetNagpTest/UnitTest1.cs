using System;
using System.Collections.Generic;
using Xunit;

namespace dotnetNagpTest
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            List<String> list = new List<String>() { "vandna"};
            int result = list.Count;
            Assert.Equal(1, result);
        }
        [Fact]
        public void Test2()
        {
            List<String> list = new List<String>() { "vandna" };
            string result = list[0];
            Assert.Equal("vandna", result);
        }
    }
}
