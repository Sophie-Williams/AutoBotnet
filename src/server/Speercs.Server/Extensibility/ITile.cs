using ImageSharp;

namespace Speercs.Server.Extensibility {
    public interface ITile {
        bool IsWalkable();

        bool IsMinable();

        char GetTileChar();

        Rgba32 GetColor();
    }
}