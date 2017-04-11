namespace Speercs.Server.Modules
{
    public class MetadataModule : SBaseModule
    {
        public MetadataModule() : base("/meta")
        {
            Get("/", _ => "Speercs Server Pre-Alpha");
        }
    }
}