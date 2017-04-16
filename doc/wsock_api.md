# WebSocket API documentation

...

The websocket system works through "bundles" that are sent back and forth.
Bundles are JSON objects that contain metadata and are sent on one line.
Bundles are used over raw data because they can contain additional information

There are different types of bundles for different kinds of request/response patterns:

## Request/Response bundles:

These types of bundles are used in client-initiated exchanges of data:

Client makes a request:

```json
{
    "request": "ping",
    "data": {},
    "id": 1234567890
}
```

- `request` contains the name of the method being called
- `data` contains a JSON value or object
- `id` is an ID that is guaranteed to be unique. It is used for matching the response to the request.

Server responds:

```json
{
    "data": "pong",
    "type": "response",
    "request": "ping",
    "id": 1234567890
}
```

The `id` in the server will match the ID of the request that was processed to create this response.

## Push bundles (server-initiated push)

Push bundles allow the server to stream data in realtime to the client. They can contain arbitrary data
and are only sent to authenticated users.

They look like this:

```json
{
    "data": {
        "some": "thing",
        "some-other": "other-thing"
    },
    "type": "push"
}
```

The `type` field and the absence of `id` can be used to differentiate push notifications from responses.

## Connecting to a user's realtime endpoint:

Connect to `ws://your-server/ws` (or `wss://`) to connect to the websocket,
and send the user's `apikey` to authenticate as that user and to subscribe
to push notifications for that user.
