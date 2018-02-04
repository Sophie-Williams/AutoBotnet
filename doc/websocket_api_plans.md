
## Websockets

Websocket API route descriptions:

"<--": incoming  
"-->": outgoing

- Map
  - Fetch map <--
  - Build building -->
- Player status
  - Creep locations <--
  - Attack/Mine/Resources/etc. <--


How we'll use 2-way websocket requests/events:

This will be similar to JSON-RPC:

Request schema:

```json
{
  "request": <string>, // request command
  "data": <object/value>, // request data
  "id": <int> // request id
}
```

Response schema:

```json
{
  "id": <int>, // request
  "data": <object/value> // response data
}
```

Request example:

```json
{
  "request": "command",
  "data": "speercs.units['bob'].memory['a']",
  "id": 123948709
}
```

(If `id` is passed, then a reply is expected. Otherwise, it is a "notification")

Response example:

```json
{
  "id": 123948709,
  "value": 42
}
```
