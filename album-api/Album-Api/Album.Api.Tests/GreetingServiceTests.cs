using System;
using System.Net;
using Xunit;
using Album.Api.Services;

namespace Album.Api.Tests
{
    public class GreetingServiceTests
    {
        private static bool IsEquals(string value)
        {
            string greeting = new GreetingService().Greeting(value);
            Assert.Equal($"Hello {value} from {Dns.GetHostName()} v2", greeting);
            return true;
        }

        private static bool IsNotEqual(string value)
        {
            string greeting = new GreetingService().Greeting(value);
            Assert.Equal($"Hello world from {Dns.GetHostName()} v2", greeting);
            return false;
        }

        [Theory]
        [InlineData("Guan")]
        [InlineData("Wu")]
        [InlineData("ww")]
        [InlineData("xx y")]
        [InlineData("xx yy")]
        public void ValidName(string value)
        {
            Assert.True(IsEquals(value),"success");
        }
        
        [Theory]
        [InlineData("W")]
        [InlineData("1")]
        [InlineData("5g")]
        [InlineData("P1")]
        [InlineData("p1")]
        [InlineData("x1 y")]
        [InlineData("x1 y2")]
        [InlineData("xx y2")]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("  ")]
        [InlineData(null)]
        public void InvalidName(string value)
        {
            Assert.False(IsNotEqual(value));
        }
    }
}