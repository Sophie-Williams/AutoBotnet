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
    this.websocket = false;
    this.wsId = new Date().getTime();
    this.onclose = (() => { console.log("Connection Closed") });
    this.wsPushListener = ((data) => { console.log(JSON.stringify(data)) });
    this.wsIds = {
      
    }
    this.authPromise;
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

  /* SECTION HELPERS */

  promiseFromGETRequest(endpoint, params={}, includeData=true, options={}) {
    return new Promise((resolve, reject) => {
      if (!this.apiKeyValid) return reject(new SpeercsErrors.KeyError());
      options['params'] = params;
      this.axios.get(endpoint, options).then((res) => {
        if (res.status != 200) return reject(new SpeercsErrors.WtfError());
        if (includeData) return resolve(res.data);
        resolve();
      }).catch((err) => {
        reject(err);
      })
    });
  }

  regenAxios() {
    this.axios = axios.create({
      baseURL: this.endpoint + "/a",
      headers: { Authorization: this.apiKey },
      responseType: 'json'
    });
  }

  /* SECTION GET ENDPOINTS */

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
    return this.promiseFromGETRequest("/game/umeta/me", {}, true, { responseType: 'text' });
  }

  getUserCode() {
    return this.promiseFromGETRequest("/game/code/get");
  }

  getUserEntities() {
    return this.promiseFromGETRequest("/game/units");
  }

  getRoom(x, y) {
    return this.promiseFromGETRequest("/game/map/room", { x: x, y: y });
  }

  /* SECTION AUTH */

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

  /* SECTION WEBSOCKETS */

  openWS() {
    return new Promise((resolve, reject) => {
      if (!this.apiKeyValid) return reject(SpeercsErrors.KeyError());
      this.websocket = new WebSocket(this.wsendpoint);
      this.websocket.parent = this;
      this.websocket.onopen = (event) => {
        this.authPromise = [resolve, reject];
        this.websocket.send(this.apiKey+"\n");
      }
      this.websocket.onmessage = this.onWsRecive;
      this.websocket.onclose = () => { console.log("F") };
    });
  }

  pingWS() {
    return new Promise((resolve, reject) => {
      if (!this.websocket || this.websocket.readyState != 1) return reject(SpeercsErrors.WSError());
      this.websocket.parent = this;
      let thisReqId = this.wsId++;
      this.wsIds[thisReqId] = [resolve,reject];
      this.websocket.send(JSON.stringify({
        request: "ping",
        data: {},
        id: thisReqId
      })+"\n");
    });
  }

  sendWs(data, type) {
    return new Promise((resolve, reject) => {
      if (!this.websocket || this.websocket.readyState != 1) return reject(SpeercsErrors.WSError());
      this.websocket.parent = this;
      let thisReqId = this.wsId++;
      this.wsIds[thisReqId] = [resolve,reject];
      this.websocket.send(JSON.stringify({
        request: type,
        data: data,
        id: thisReqId
      })+"\n");
    });
  }

  onWsRecive(data) {
    if (data.data == "true") return this.parent.authPromise[0]();
    if (data.data == "false") return this.parent.authPromise[1]();
    data = JSON.parse(data.data);
    if (!data.id) { // Is `PUSH` notif, do stuff with this.
      return this.parent.wsPushListener(data.data);
    }
    this.parent.wsIds[data.id][0](data.data);
  }

  /* SECTION POST ENDPOINTS */

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
  static WSError() {
    this.message = "WebSocket is not initialized or connecter";
    this.name = "ErrNoWS";
  }
}