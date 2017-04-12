namespace Speercs.Server.Modules
{
    public class MetadataModule : SBaseModule
    {
        public MetadataModule() : base("/meta")
        {
            Get("/", _ => "Speercs Server Pre-Alpha");
            Get("/version", _ => string.Format("Speercs Server v{0} running on .NET core v{1}","1.0.0","1.0.1")); // TODO: Get version numbers automagically
        }
    }
}