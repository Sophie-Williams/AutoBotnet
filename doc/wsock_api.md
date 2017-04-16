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
