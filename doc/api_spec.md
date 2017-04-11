
# API Spec for **Speercs**

REST API routes:

- Auth
  - Register
  - Login
- Player code
  - Get current code
  - Deploy new code
- Info requests
  - Request some player data from identifier

Websocket API routes:



How we'll use 2-way websocket requests/events:

This will be similar to JSON-RPC:

Request schema:

```
{
  "request": <string>, // request command
  "data": <object/value>, // request data
  "id": <int> // request id
}
```

Response schema:

```
{
  "id": <int>, // request
  "data": <object/value> // response data
}
```

Request example:

```json
{
  "request": "console_command",
  "data": "speercs.creeps['bob'].memory['a']",
  "id": 123948709
}
```

(If `id` is passed, then a reply is expected. Otherwise, it is a "notification")

Response example:

```json
{
  "id": 123948709,
  "data": 42
}
```
