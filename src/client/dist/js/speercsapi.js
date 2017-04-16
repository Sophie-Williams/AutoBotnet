class SpeercsApi {
  constructor(endpoint, apiKey = null) {
    this.endpoint = endpoint;
    this.username = null;
    this.apiKeyValid = false;
    this.serverInfo = null;
    this.apiKey = apiKey;
    this.program = null;
    this.entities = null;
    this.globalEntities = null;
    this.websocket = null;
    this.wsId = new Date().getTime();
    this.wsPushListener = ((data) => { console.log(JSON.stringify(data)) })
    this.wsIds = {
      'auth': [],
      'ping': {
        'action': this.doPingRequest
      }
    }
    this.wsendpoint = endpoint.replace("http://", "ws://").replace("https://", "wss://") + "ws";
    this.axios = axios.create({
      baseURL: this.endpoint + "/a",
      headers: { Authorization: this.apiKey },
      responseType: 'json'
    });

    this.getMeta();
    if (this.apiKey) {
      this.getUserInfo();
      this.getUserCode();
    }
  }

  getMeta() {
    return new Promise((resolve, reject) => {
      axios.get(this.endpoint + "/meta").then((res) => {
        this.serverInfo = res.data;
        resolve();
      }).catch((err) => {
        reject(err);
      });
    });
  }

  getUserInfo() {
    return new Promise((resolve, reject) => {
      this.axios.get("/game/umeta/me", { responseType: 'text' }).then((res) => {
        if (res.status != 200) return reject(new SpeercsErrors.CredentialError());
        this.username = res.data;
        resolve();
      }).catch((err) => {
        reject(err);
      });
    });
  }

  getUserCode() {
    return new Promise((resolve, reject) => {
      this.axios.get("/game/code/get").then((res) => {
        if (res.status != 200) return reject(new SpeercsErrors.WtfError());
        this.program = res.data.source;
        resolve();
      }).catch((err) => {
        reject(err);
      });
    });
  }

  getUserEntities() {
    return new Promise((resolve, reject) => {
      if (!this.apiKeyValid) return reject(new SpeercsErrors.KeyError());
      this.axios.get("/game/units").then((res) => {
        if (res.status != 200) return reject(new SpeercsErrors.WtfError());
        this.entities = res.data;
        resolve();
      }).catch((err) => {
        reject(err);
      });
    });
  }

  getRoom(x, y) {
    return new Promise((resolve, reject) => {
      if (!this.apiKeyValid) return reject(new SpeercsErrors.KeyError());
      this.axios.get("/game/map/room", {
        params: {
          x: x,
          y: y
        }
      }).then((res) => {
        if (res.status != 200) return reject(new SpeercsErrors.WtfError());
        resolve(res.data);
      }).catch((err) => {
        reject(err);
      })
    });
  }

  login(username, password) {
    return new Promise((resolve, reject) => {
      this.axios.post("/auth/login", {
        username: username,
        password: password
      }).then((res) => {
        if (res.status != 200) return reject(SpeercsErrors.CredentialError());
        this.apiKey = res.data.apikey;
        this.apiKeyValid = true;
        this.username = res.data.username;
        this.regenAxios();
        resolve();
      }).catch((err) => {
        reject(err);
      });
    });
  }

  regenAxios() {
    this.axios = axios.create({
      baseURL: this.endpoint + "/a",
      headers: { Authorization: this.apiKey },
      responseType: 'json'
    });
  }

  openWS() {
    return new Promise((resolve, reject) => {
      if (!this.apiKeyValid) return reject(SpeercsErrors.KeyError());
      this.websocket = new WebSocket(this.wsendpoint);
      this.websocket.onopen = (event) => {
        console.log("WS Open");
        this.wsIds.auth = [resolve, reject];
        this.websocket.send(this.apiKey+"\n");
      }
      this.websocket.onmessage = this.onWsRecive;
      this.websocket.onclose = () => { console.log("F") };
    });
  }

  register(username, password, invitekey = false) {
    return new Promise((resolve, reject) => {
      if (serverInfo.inviterequired && !invitekey) return reject(SpeercsErrors.NoInviteError());
      this.axios.post("/auth/register", {
        username: username,
        password: password,
        invitekey: invitekey
      }).then((res) => {
        if (res.status != 200) return reject(SpeercsErrors.WtfError());
        this.apiKey = res.data.apikey;
        this.apiKeyValid = true;
        this.username = res.data.username;
        this.regenAxios();
        this.GetUserCode();
        resolve();
      }).catch((err) => {
        reject(err);
      });
    });
  }

  updateUserCode(code) {
    return new Promise((resolve, reject) => {
      if (!this.apiKeyValid) return reject(SpeercsErrors.KeyError());
      this.axios.post("/game/code/deploy", {
        Source: code
      }).then((res) => {
        if (res.status != 200) return reject(SpeercsErrors.WtfError());
        this.code = code;
        resolve();
      }).catch((err) => {
        reject(err);
      });
    });
  }

  onWsRecive(data) {
    console.log(JSON.stringify(data));
    if (data == "true") return this.wsIds.auth();
    if (data == "false") return this.wsIds.auth();
    data = JSON.parse(data);
    if (!data.id) { // Is `PUSH` notif, do stuff with this.
      return this.wsPushListener(data.data);
    }
    this.wsIds[data.request][data.id][0](data.data);
  }

  doPingRequest(data) {
    this.wsIds[data.id][0](data.data);
  }
}

class SpeercsErrors {
  static NoInviteError() {
    this.message = "Invite key is required, but not provided.";
    this.name = "ErrNoInvite";
  }
  static WtfError() {
    this.message = "Your reject message is in annother castle.";
    this.name = "ErrSomethingHappened";
  }
  static CredentialError() {
    this.message = "Invalid credentials provided";
    this.name = "ErrInvalidCredentials";
  }
  static KeyError() {
    this.message = "apiKey is not set or invalid";
    this.name = "ErrapiKey";
  }
}