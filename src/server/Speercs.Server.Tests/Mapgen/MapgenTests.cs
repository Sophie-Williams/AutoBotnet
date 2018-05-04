using Xunit;
using Xunit.Abstractions;

namespace Speercs.Server.Tests.Mapgen {
    public class MapgenTests {
        private readonly ITestOutputHelper _output;

        public MapgenTests(ITestOutputHelper outputHelper) {
            _output = outputHelper;
        }

        [Fact]
        public void trueIsTrue() {
            _output.WriteLine("true is always true lol");
            Assert.True(true);
        }
    }
}