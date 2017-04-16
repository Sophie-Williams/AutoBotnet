class SpeercsApi {
  constructor(endpoint, token = null) {
    this.endpoint = endpoint;
    this.username = null;
    this.tokenValid = false;
    this.serverInfo = null;
    this.token = token;
    this.program = null;
    this.entities = null;
    this.globalEntities = null;
    this.wsId = new Date().getTime();
    this.wsIds = {
      'auth': {
        'action':this.doAuthRequest
      },
      'ping': {
        'action':this.doPingRequest
      }
    }
    this.wsendpoint = endpoint.replace("http://","ws://").replace("https://","wss://")+"/ws";
    this.axios = axios.create({
      baseURL: this.endpoint + "/a",
      headers: {Authorization: this.token},
      responseType: 'json'
    });

    this.getMeta();
    if (this.token) {
      this.getUserInfo();
      this.getUserCode();
    }
  }

  getMeta() {
    return new Promise((sucess, error) => {
      axios.get(this.endpoint + "/meta").then((res) => {
        this.serverInfo = res.data;
        sucess();
      }).catch((err) => {
        error(err);
      });
    });
  }

  getUserInfo() {
    return new Promise((sucess, error) => {
      this.axios.get("/game/umeta/me", { responseType: 'text' }).then((res) => {
        if (res.status != 200) return error(new SpeercsErrors.CredentialError());
        this.username = res.data;
        sucess();
      }).catch((err) => {
        error(err);
      });
    });
  }

  getUserCode() {
    return new Promise((sucess, error) => {
      this.axios.get("/game/code/get").then((res) => {
        if (res.status != 200) return error(new SpeercsErrors.WtfError());
        this.program = res.data.source;
        sucess();
      }).catch((err) => {
        error(err);
      });
    });
  }

  getUserEntities() {
    return new Promise((sucess, error) => {
      if (!this.tokenValid) return error(new SpeercsErrors.KeyError());
      this.axios.get("/game/units").then((res) => {
        if (res.status != 200) return error(new SpeercsErrors.WtfError());
        this.entities = res.data;
        sucess();
      }).catch((err) => {
        error(err);
      });
    });
  }

  getRoom(x, y) {
    return new Promise((sucess, error) => {
      if (!this.tokenValid) return error(new SpeercsErrors.KeyError());
      this.axios.get("/game/map/room", {
        params: {
          x: x,
          y: y
        }
      }).then((res) => {
        if (res.status != 200) return error(new SpeercsErrors.WtfError());
        sucess(res.data);
      }).catch((err) => {
        error(err);
      })
    });
  }

  login(username, password) {
    return new Promise((sucess, error) => {
      this.axios.post("/auth/login", {
        username: username,
        password: password
      }).then((res) => {
        if (res.status != 200) return error(new SpeercsErrors.CredentialError());
        this.token = res.data.token;
        this.tokenValid = true;
        this.username = res.data.username;
        sucess();
      }).catch((err) => {
        error(err);
      });
    });
  }

  openWS() {
    return new Promise((sucess, error) => {
      if (!this.tokenValid) return error(new SpeercsErrors.KeyError());
      this.websocket = new WebSocket(this.wsendpoint);
      this.websocket.onopen = (event) => {
        var thisReqId = this.wsId++;
        this.wsIds.auth[thisReqId] = [sucess, error];
        this.websocket.send(JSON.stringify({
          "request": "auth",
          "data": this.token,
          "id": authReqId
        }));
      }
      this.websocket.onmessage = this.onWsRecive;
    });
  }

  register(username, password, invitekey = false) {
    return new Promise((sucess, error) => {
      if (serverInfo.inviterequired && !invitekey) return error(new SpeercsErrors.NoInviteError());
      this.axios.post("/auth/register", {
        username: username,
        password: password,
        invitekey: invitekey
      }).then((res) => {
        if (res.status != 200) return error(new SpeercsErrors.WtfError());
        this.token = res.data.token;
        this.tokenValid = true;
        this.username = res.data.username;
        this.GetUserCode();
        sucess();
      }).catch((err) => {
        error(err);
      });
    });
  }

  updateUserCode(code) {
    return new Promise((sucess, error) => {
      if (!this.tokenValid) return error(new SpeercsErrors.KeyError());
      this.axios.post("/game/code/deploy", {
        Source: code
      }).then((res) => {
        if (res.status != 200) return error(new SpeercsErrors.WtfError());
        this.code = code;
        sucess();
      }).catch((err) => {
        error(err);
      });
    });
  }

  onWsRecive(data) {
    data = JSON.parse(data);
    if (!data.id) return false; // TODO: Stuff with this
    for (key in Object.keys(wsIds)) {
      if (data.id in wsIds[key]) {
        wsIds[key].action(data);
        delete wsIds[key][data.id];
      }
    }
  }
  
  doAuthRequest(data) {
    if (data.data != true) {
      this.wsIds.auth[data.id][1]();
      return;
    }
    this.wsIds.auth[data.id][0]();
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
    this.message = "Your error message is in annother castle.";
    this.name = "ErrSomethingHappened";
  }
  static CredentialError() {
    this.message = "Invalid credentials provided";
    this.name = "ErrInvalidCredentials";
  }
  static KeyError() {
    this.message = "Token is not set or invalid";
    this.name = "ErrToken";
  }
}