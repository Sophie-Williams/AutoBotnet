using System.IO;
using Speercs.Server.Models.Math;

namespace Speercs.Server.Utilities {
    public static class BinarySerializationExtensions {
        public static void Write(this BinaryWriter writer, Point point) {
            writer.Write(point.x);
            writer.Write(point.y);
        }
    }
}