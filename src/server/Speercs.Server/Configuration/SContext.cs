
using System;
using LiteDB;

namespace Speercs.Server.Configuration
{
    public class SContext : ISContext
    {
        // Configuration parameters
        public SConfiguration Configuration { get; }

        // Database access
        public LiteDatabase Database { get; private set; }

        public SContext(SConfiguration config)
        {
            Configuration = config;
        }

        public void ConnectDatabase()
        {
            // Create database
            Database = new LiteDatabase(Configuration.DatabaseConfiguration.FileName);
        }
    }
}