using SixLabors.ImageSharp.PixelFormats;

namespace Speercs.Server.Extensibility.Map {
    public interface ITile {
        bool isWalkable();
        bool isMinable();
        char getTileChar();
        Rgba32 getColor();
        string name { get; set; }
    }
}