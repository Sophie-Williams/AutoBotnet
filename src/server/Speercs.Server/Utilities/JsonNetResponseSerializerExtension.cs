using Nancy;
using Newtonsoft.Json;

namespace Speercs.Server.Utilities {
    public static class JsonNetResponseSerializerExtension {
        public static Response asJsonNet<T>(this IResponseFormatter formatter, T instance) {
            var responseData = JsonConvert.SerializeObject(instance);
            return formatter.AsText(responseData, "application/json");
        }
    }
}