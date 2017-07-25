# API Spec for **Speercs**

REST API route descriptions:

- Auth
  - Register
  - Login
  - Delete
  - Change Password
- Server Info
  - Info
  - Meta

## Auth

### Register

> Register a new user

`POST /a/auth/register`

REQUEST:

```json
{
    "Username": "<USERNAME>",
    "Email": "<EMAIL [OPTIONAL]>",
    "Password": "<PASSWORD>",
    "InviteKey": "<INVITE KEY> [ONLY REQIRED IF `inviteRequired` IS TRUE]>"
}
```

RESPONSE:

```json
{
    "username": "joonatoona",
    "email": "",
    "apikey": "jfgshiuoreb7c6w8s76ctbrsiu5su464",
    "analyticsEnabled": false
}
```

### Login

> Get an API key from credentials

`POST /a/auth/login`

REQUEST:

```json
{
    "Username": "<USERNAME>",
    "Password": "<PASSWORD>"
}
```

RESPONSE:

```json
{
    "username": "joonatoona",
    "email": "",
    "apikey": "jfgshiuoreb7c6w8s76ctbrsiu5su464",
    "analyticsEnabled": false
}
```

### Delete

> Delete a user account

`DELETE /a/auth/delete`

REQUEST:

```json
{
    "Username": "<USERNAME>",
    "Password": "<PASSWORD>"
}
```

RESPONSE:

`200`

### Change Password

> Change a password for a user

`PATCH /a/auth/changepassword`

REQUEST:

```json
{
    "Username": "<USERNAME>",
    "OldPassword": "<OLD_PASSWORD>",
    "NewPassword": "<NEW_PASSWORD>"
}
```

RESPONSE:

`200`

## Server Info

# Info

> Get static server information

`GET /a/info`

RESPONSE:


```json
{
    "name": "CookieEaters Official",
    "motd": "AutoBotnet server v1.0.0.0\nThis message is configurable by the server admins.",
    "version": "1.0.0.0",
    "inviteRequired": false
}
```

# Websockets

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
