
# API Spec for **Speercs** AutoBotnet Server

## Request Index

Key

- `[u]` - the request (and all children) are user requests, append the `Authorization: <apikey>` header to all user requests.

- [Server Info](#server-info)
  - [Info](#info)
  - [Meta](#meta)
- [Auth](#auth)
  - [Register](#register)
  - [Login](#login)
  - [Delete](#delete)
  - [Change password](#change-password)
  - [Reauth](#reauth)
  - [Regenerate API Key](#regenerate-api-key)
- [Game](#game) [u]
  - [Deploy Code](#deploy-code)


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

## Game

### Deploy Code

#### Get

> Retrieve currently deployed code

`GET /a/game/code/get`

RESPONSE:

```json
// TODO
```

#### Reload

> Request a reinitialization of the engine hosting the user's program

`PATCH /a/game/code/reload`

RESPONSE:

Status code `2xx`.

#### Deploy

> Deploy a new program for the user

`POST /a/game/code/deploy`

REQUEST:

```json
{
    "source": "<js program source>"
}
```

RESPONSE:

Status code `2xx`.
