# Client-side library documentation for **Speercs**

### Initializing an instance:

```js
var user = new SpeercsApi("<Server Url>", "[API Key]");
```

\<Server Url\> is the root of the server (For example: "http://speercs.joonatoona.me/") (***MAKE SURE YOU INCLUDE A SLASH AT THE END***)

[API Key] is an optional argument for the users API key. Using `.login()` instead is recomended.

### Logging in an instance

```js
user.login("<username>","<password>").then(() => { Do whatever });
```

### Registering a user

```js
user.register("<username>","<password>","[InviteKey]").then(() => { Do whatever });
```

[InviteKey] is an optional parameter, and is required if the server has invite only required.

### Getting a room

```js
user.getRoom(<x>, <y>).then((room) => { Do whatever with room });
```

### Refreshing user code

```js
user.getUserCode().then(() => { Do whatever });
```

You can access user code with

```js
user.program;
```

### Updating user code

```js
user.updateUserCode("<code>").then(() => { Do whatever });
```

### Getting user owned entities

```js
user.getUserEntities().then(() => { Do whatever });
```

Entities can be accessed with

```js
user.entities;
```

### Initializing WebSocket

```js
user.openWS().then(() => { Do whatever });
```

WebSocket usage is currently unavailible, as it hasn't been implemented server-side yet