using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using Speercs.Server.Configuration;
using Speercs.Server.Models.Entities;
using Speercs.Server.Models.Map;
using Speercs.Server.Models.Math;

namespace Speercs.Server.Services.EventPush {
    public class EventPushService : DependencyObject {
        public EventPushService(ISContext serverContext) : base(serverContext) { }

        public const string EVENTPUSH_ENTITY = "entity";
        public const string EVENTPUSH_ROOM_TILE = "room_tile";

        private void allTeamsInRoom(HashSet<string> recipients, Point roomPos) {
            foreach (var nearEntity in serverContext.appState.entities.getByRoom(roomPos)) {
                if (!recipients.Contains(nearEntity.teamId)) {
                    recipients.Add(nearEntity.teamId);
                }
            }
        }

        public bool pushEvent(string item, string type, object sender, object data) {
            // handle built-in events
            // TODO: Extensible handlers-based interface
            var recipients = new HashSet<string>();
            switch (item) {
                case EVENTPUSH_ENTITY: {
                    var senderEntity = (GameEntity) sender;
                    // the user should get their own entity
                    recipients.Add(senderEntity.teamId);
                    // any other teams with entities in the area should also receive it.
                    // use the spatial hash to look for other teams
                    allTeamsInRoom(recipients, senderEntity.position.roomPos);
                    // append the entity ID
                    item += $"|{senderEntity.id}";
                    break;
                }
                case EVENTPUSH_ROOM_TILE: {
                    var pos = (RoomPosition) sender;
                    allTeamsInRoom(recipients, pos.roomPos);
                    break;
                }
            }
            // send the event
            var bundle = new JObject(
                new JProperty("item", item),
                new JProperty("type", type),
                new JProperty("data", JToken.FromObject(data))
            );
            foreach (var recipient in recipients) {
                var pushTask = serverContext.notificationPipeline.pushMessageAsync(bundle, "event", recipient, false);
            }
            // it was delivered successfully
            return true;
        }
    }
}