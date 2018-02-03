using ImageSharp;

namespace Speercs.Server.Extensibility {
    public interface ITile {
        bool isWalkable();

        bool isMinable();

        char getTileChar();

        Rgba32 getColor();
    }
}