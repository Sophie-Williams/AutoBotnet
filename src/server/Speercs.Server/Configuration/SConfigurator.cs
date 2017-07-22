using LiteDB;
using Speercs.Server.Models.Game.Map;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Speercs.Server.Configuration
{
    public class SConfigurator
    {
        internal static SContext CreateContext(SConfiguration config)
        {
            // load the parameters
            config.BaseDirectory = Directory.GetCurrentDirectory();
            var context = new SContext(config);
            return context;
        }

        public const string StateStorageKey = "state";

        public static bool SerializationMappersRegistered { get; private set; }

        public static void LoadState(SContext serverContext, string stateStorageFile)
        {
            if (!SerializationMappersRegistered)
            {
                BsonMapper.Global.RegisterType<WorldMap>(
                    serialize: map => BsonMapper.Global.ToDocument(map.RoomDict),
                    deserialize: bson => new WorldMap
                    {
                        RoomDict = (Dictionary<string, Room>)BsonMapper.Global
                            .ToObject(typeof(Dictionary<string, Room>), bson.AsDocument)
                    }
                );
                SerializationMappersRegistered = true;
            }
            // Load the Server State into the context. This object also includes the OsmiumMine Core state
            var database = new LiteDatabase(stateStorageFile);
            var stateStorage = database.GetCollection<SAppState>(StateStorageKey);
            var savedState = stateStorage.FindAll().FirstOrDefault();
            if (savedState == null)
            {
                // Create and save new state
                savedState = new SAppState();
                stateStorage.Upsert(savedState);
            }
            // Update context
            savedState.PersistenceMedium = stateStorage;
            savedState.Persist = (forcePersist) =>
            {
                // If needed...
                if (forcePersist || savedState.PersistNeeded)
                {
                    savedState.PersistAvailable = false;
                    // Update in database
                    stateStorage.Upsert(savedState);
                    // And unset needed flag
                    savedState.PersistNeeded = false;
                    savedState.PersistAvailable = true;
                }
            };
            // Save the state
            savedState.Persist(true);
            // Update references
            serverContext.AppState = savedState;
            var timedPersistTask = StartTimedPersistAsync(serverContext, savedState);
        }

        private static async Task StartTimedPersistAsync(SContext serverContext, SAppState state)
        {
            while (true)
            {
                if (state.PersistAvailable)
                {
                    await Task.Delay(serverContext.Configuration.PersistenceInterval);
                    state.Persist(false);
                }
            }
        }
    }
}
