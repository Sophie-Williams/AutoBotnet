using Speercs.Server.Configuration;
using Speercs.Server.Game.MapGen;
using Speercs.Server.Models.Map;
using Xunit;
using Xunit.Abstractions;

namespace Speercs.Server.Tests.Mapgen {
    public class MapgenTests : IClassFixture<MapgenTestsFixture> {
        private readonly MapgenTestsFixture _fixture;
        private readonly ITestOutputHelper _output;

        public MapgenTests(MapgenTestsFixture fixture, ITestOutputHelper outputHelper) {
            _fixture = fixture;
            _output = outputHelper;
        }

        [Fact]
        public void roomHasExits() {
            var room = _fixture.testRoom;
            var hasNorthExit = room.northExit.high - room.northExit.low;
            Assert.True(hasNorthExit > 0);
            var hasEastExit = room.eastExit.high - room.eastExit.low;
            Assert.True(hasEastExit > 0);
            var hasSouthExit = room.southExit.high - room.southExit.low;
            Assert.True(hasSouthExit > 0);
            var hasWestExit = room.westExit.high - room.westExit.low;
            Assert.True(hasWestExit > 0);
        }
    }

    public class MapgenTestsFixture : DependencyFixture {
        public MapgenTestsFixture() : base(new SConfiguration()) {
            mapGenerator = new MapGenerator(serverContext, new MapGenParameters());
            testRoom = mapGenerator.generateRoom(0, 0);
        }

        public MapGenerator mapGenerator;
        public Room testRoom;
    }
}