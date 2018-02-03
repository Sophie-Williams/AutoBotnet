using LiteDB;
using Speercs.Server.Models.Game.Map;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Speercs.Server.Configuration {
    public class SConfigurator {
        internal static SContext createContext(SConfiguration config) {
            // load the parameters
            config.baseDirectory = Directory.GetCurrentDirectory();
            var context = new SContext(config);
            return context;
        }

        public const string STATE_STORAGE_KEY = "state";

        public static bool serializationMappersRegistered { get; private set; }

        public static void loadState(SContext serverContext, string stateStorageFile) {
            if (!serializationMappersRegistered) {
                BsonMapper.Global.RegisterType<WorldMap>(
                    serialize: map => BsonMapper.Global.ToDocument(map.roomDict),
                    deserialize: bson => new WorldMap {
                        roomDict = (Dictionary<string, Room>) BsonMapper.Global
                            .ToObject(typeof(Dictionary<string, Room>), bson.AsDocument)
                    }
                );
                serializationMappersRegistered = true;
            }

            // Load the Server State into the context. This object also includes the OsmiumMine Core state
            var database = new LiteDatabase(stateStorageFile);
            var stateStorage = database.GetCollection<SAppState>(STATE_STORAGE_KEY);
            var savedState = stateStorage.FindAll().FirstOrDefault();
            if (savedState == null) {
                // Create and save new state
                savedState = new SAppState();
                stateStorage.Upsert(savedState);
            }

            // Update context
            savedState.persistenceMedium = stateStorage;
            savedState.persist = (forcePersist) => {
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