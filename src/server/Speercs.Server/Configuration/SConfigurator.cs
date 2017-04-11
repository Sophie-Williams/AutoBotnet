using System.IO;
using System.Linq;
using LiteDB;

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

        public static void LoadState(SContext serverContext, string stateStorageFile)
        {
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
            savedState.Persist = () =>
            {
                // Update in database
                stateStorage.Upsert(savedState);
            };
            // TODO: Merge API keys, etc.
            // Save the state
            //savedState.PersistenceMedium.Upsert(savedState);
            savedState.Persist();
            // Update references
            serverContext.AppState = savedState;
        }
    }
}