using System;
using System.IO;
using System.IO.Compression;
using System.Text;
using Speercs.Server.Configuration;
using Speercs.Server.Extensibility.Map;
using Speercs.Server.Models.Map;
using Speercs.Server.Utilities;

namespace Speercs.Server.Models.Events {
    public class TileDeltaEvent {
        public static string pack(ISContext context, RoomPosition pos, Tile tile) {
            var result = new StringBuilder();
            using (var output = new MemoryStream())
            using (var compressedOutput = new GZipStream(output, CompressionMode.Compress))
            using (var bw = new BinaryWriter(compressedOutput)) {
                // write room position
                bw.Write(pos.roomPos);
                bw.Write(pos.pos);
                
                // write tile
                Room.writeTile(context, bw, tile);
                
                bw.Flush();
                result.Append(Convert.ToBase64String(output.ToArray()));
            }

            return result.ToString();
        }
    }
}