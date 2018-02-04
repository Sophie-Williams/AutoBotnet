# API Spec for **Speercs**

REST API route descriptions:

- Auth
  - Register
  - Login
  - Delete
  - Change password
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
    "username": "<username>",
    "email": "<email [optional]>",
    "password": "<password>",
    "invitekey": "<invite key> [only required if `requireInvite` is true]>"
}
```

RESPONSE:

```json
{
    "username": "<username>",
    "email": "<email>",
    "apikey": "<api key [alphanumeric string]>"
}
```

### Login

> Get an API key from credentials

`POST /a/auth/login`

REQUEST:

```json
{
    "username": "<username>",
    "password": "<password>"
}
```

RESPONSE:

```json
{
    "username": "<username>",
    "email": "<email>",
    "apikey": "<api key [alphanumeric string]>"
}
```


### Delete

> Delete a user account

`DELETE /a/auth/delete`

REQUEST:

```json
{
    "username": "<username>",
    "password": "<password>"
}
```

RESPONSE:

Status code `2xx`.

### Change password

> Change the password for a user. Resets the API key

`PATCH /a/auth/changepassword`

REQUEST:

```json
{
    "username": "<username>",
    "oldpassword": "<old_password>",
    "oewpassword": "<new_password>"
}
```

RESPONSE:

Status code `2xx`.

### Reauth

> Validate an API key by using it to log in as a user

`PATCH /a/auth/reauth`

REQUEST:

```json
{
    "username": "<username>",
    "apikey": "<apikey>"
}
```

RESPONSE:

```json
{
    "username": "<username>",
    "email": "<email>",
    "apikey": "<api key [alphanumeric string]>"
}
```

### Regenerate API Key

> Generate a new API key. Requires a password-backed login to obtain the new key

`PATCH /a/auth/newkey`

REQUEST:

```json
{
    "username": "<username>",
    "apikey": "<apikey>"
}
```

RESPONSE:

Status code `2xx`.

## Server Info

### Info

> Get information about the server

`GET /a/info`

RESPONSE:


```json
{
    "userCount": 1337, // Haha I wish
    "tickrate": 1000,
    "mapSize": 80085, // lol ikr
    "inviteRequired": false
}
```

## Server Meta

### Meta

> Get metadata about the server

`GET /a/meta`

RESPONSE:


```json
{
    "name": "CookieEaters Official",
    "motd": "Welcome to AutoBotnet server v0.1",
    "version": "0.1"
}
```

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
