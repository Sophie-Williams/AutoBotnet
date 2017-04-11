using Xunit;
using Xunit.Abstractions;

namespace Speercs.Server.Tests
{
    public class MapgenTests
    {
        private readonly ITestOutputHelper output;

        public MapgenTests(ITestOutputHelper outputHelper)
        {
            output = outputHelper;
        }
        
        [Fact]
        public void TrueIsTrue()
        {
            output.WriteLine("true is always true lol");
            Assert.True(true);
        }
    }
}
