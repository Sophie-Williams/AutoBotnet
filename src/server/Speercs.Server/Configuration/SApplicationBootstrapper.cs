using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using LiteDB;
using Speercs.Server.Extensibility.Map;
using Speercs.Server.Models.Entities;
using Speercs.Server.Models.Map;
using Speercs.Server.Models.Math;

namespace Speercs.Server.Configuration {
    public static class SConfigurator {
        internal static SContext createContext(SConfiguration config) {
            // load the parameters
            config.baseDirectory = Directory.GetCurrentDirectory();
            var context = new SContext(config);
            return context;
        }

        private const string state_storage_key = "state";

        private static bool serializationMappersRegistered { get; set; }

        public static void loadState(SContext serverContext, string stateStorageFile) {
            if (!serializationMappersRegistered) {
                BsonMapper.Global.RegisterType(
                    serialize: map => BsonMapper.Global.ToDocument(map.rooms),
                    deserialize: bson => new WorldMap {
                        rooms = (Dictionary<string, Room>) BsonMapper.Global
                            .ToObject(typeof(Dictionary<string, Room>), bson.AsDocument)
                    }
                );
                // TODO: custom serializers for tile data
//                BsonMapper.Global.RegisterType<ITile>(
//                    serialize: tile => tile.GetType().Name,
//                    deserialize: bson =>
//                        (ITile) Activator.CreateInstance(serverContext.extensibilityContainer.resolveTypes<ITile>()
//                            .First(x => x.Name == bson.AsString))
//                );
                BsonMapper.Global.RegisterType<ITile[,]>(
                    serialize: tileArray => {
                        var doc = new BsonDocument();
                        var d = tileArray.GetLength(0);
                        for (var i = 0; i < d; i++) {
                            for (var j = 0; j < d; j++) {
                                var tile = tileArray[i, j];
                                doc.Add((i * d + j).ToString(), new BsonValue(tile.GetType().Name));
                            }
                        }

                        return doc;
                    },
                    deserialize: bson => {
                        var d = Room.MAP_EDGE_SIZE;
                        var tileArray = new ITile[d, d];
                        for (var i = 0; i < d; i++) {
                            for (var j = 0; j < d; j++) {
                                var tileTypeName = (bson.AsDocument[(i * d + j).ToString()].AsString);
                                var tile = (ITile) Activator.CreateInstance(serverContext.extensibilityContainer
                                    .resolveTypes<ITile>()
                                    .First(x => x.Name == tileTypeName));
                                tileArray[i, j] = tile;
                            }
                        }

                        return tileArray;
                    }
                );
                BsonMapper.Global.RegisterType(
                    serialize: room => new BsonDocument(new Dictionary<string, BsonValue> {
                        [nameof(Room.x)] = new BsonValue(room.x),
                        [nameof(Room.y)] = new BsonValue(room.y),
                        [nameof(Room.spawn)] = BsonMapper.Global.ToDocument(room.spawn),
                        [nameof(Room.creationTime)] = new BsonValue(room.creationTime.ToString()),
                        [nameof(Room.tiles)] = BsonMapper.Global.ToDocument(room.tiles),
                        [nameof(Room.northExit)] = BsonMapper.Global.ToDocument(room.northExit),
                        [nameof(Room.eastExit)] = BsonMapper.Global.ToDocument(room.eastExit),
                        [nameof(Room.southExit)] = BsonMapper.Global.ToDocument(room.southExit),
                        [nameof(Room.westExit)] = BsonMapper.Global.ToDocument(room.westExit)
                    }),
                    deserialize: bson =>
                        new Room(bson.AsDocument[nameof(Room.x)].AsInt32, bson.AsDocument[nameof(Room.y)].AsInt32) {
                            spawn = BsonMapper.Global.ToObject<Point>(bson.AsDocument[nameof(Room.spawn)].AsDocument),
                            creationTime = ulong.Parse(bson.AsDocument[nameof(Room.creationTime)].AsString),
                            tiles =
                                BsonMapper.Global.ToObject<ITile[,]>(bson.AsDocument[nameof(Room.tiles)].AsDocument),
                            northExit = BsonMapper.Global.ToObject<Room.Exit>(bson.AsDocument[nameof(Room.northExit)]
                                .AsDocument),
                            eastExit = BsonMapper.Global.ToObject<Room.Exit>(bson.AsDocument[nameof(Room.eastExit)]
                                .AsDocument),
                            southExit = BsonMapper.Global.ToObject<Room.Exit>(bson.AsDocument[nameof(Room.southExit)]
                                .AsDocument),
                            westExit = BsonMapper.Global.ToObject<Room.Exit>(bson.AsDocument[nameof(Room.westExit)]
                                .AsDocument)
                        });
                BsonMapper.Global.RegisterType(
                    serialize: point => new BsonDocument(new Dictionary<string, BsonValue> {
                        ["x"] = new BsonValue(point.x),
                        ["y"] = new BsonValue(point.y)
                    }),
                    deserialize: bson =>
                        new Point(bson.AsDocument["x"].AsInt32, bson.AsDocument["y"].AsInt32)
                );
                BsonMapper.Global.RegisterType(
                    serialize: exit => new BsonDocument(new Dictionary<string, BsonValue> {
                        [nameof(Room.Exit.low)] = new BsonValue(exit.low),
                        [nameof(Room.Exit.high)] = new BsonValue(exit.high)
                    }),
                    deserialize: bson =>
                        new Room.Exit(bson.AsDocument[nameof(Room.Exit.low)].AsInt32,
                            bson.AsDocument[nameof(Room.Exit.high)].AsInt32)
                );
                serializationMappersRegistered = true;
            }

            // Load the Server State into the context. This object also includes the OsmiumMine Core state
            var database = new LiteDatabase(stateStorageFile);
            var stateStorage = database.GetCollection<SAppState>(state_storage_key);
            var savedState = stateStorage.FindAll().FirstOrDefault();
            if (savedState == null) {
                // Create and save new state
                savedState = new SAppState();
                stateStorage.Upsert(savedState);
            }

            // Update context
            savedState.persistenceMedium = stateStorage;
            savedState.persist = forcePersist => {
                // If needed...
                if (forcePersist || savedState.persistNeeded) {
                    savedState.persistAvailable = false;
                    // Update in database
                    stateStorage.Upsert(savedState);
                    // And unset needed flag
                    savedState.persistNeeded = false;
                    savedState.persistAvailable = true;
                }
            };
            // Save the state
            savedState.persist(true);
            // Update references
            serverContext.appState = savedState;
            var timedPersistTask = startTimedPersistAsync(serverContext, savedState);
        }

        private static async Task startTimedPersistAsync(SContext serverContext, SAppState state) {
            while (true) {
                if (state.persistAvailable) {
                    await Task.Delay(serverContext.configuration.persistenceInterval);
                    state.persist(false);
                }
            }
        }
    }
}