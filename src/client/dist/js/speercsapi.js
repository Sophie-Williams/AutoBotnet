class SpeercsApi {
  constructor(endpoint, apiKey = null) {
    this.endpoint = endpoint;
    this.username = null;
    this.apiKeyValid = false;
    this.serverInfo = null;
    this.apiKey = apiKey;
    this.axios = axios.create({
      baseURL: this.endpoint + "/a",
      params: {
        apikey: this.apiKey
      },
      responseType: 'json'
    });
    this.UpdateMeta();
  }

  UpdateMeta() {
    return new Promise((sucess, error) => {
      axios.get(this.endpoint + "/meta").then((res) => {
        this.serverInfo = res.data;
        sucess();
      }).catch((err) => {
        error(err);
      });
    });
  }

  UpdateUserInfo() {
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

  Login(username, password) {
    return new Promise((sucess, error) => {
      this.axios.post("/auth/login", { username: username, password: password }).then((res) => {
        if (res.status != 200) return error(new SpeercsErrors.CredentialError());
        this.apiKey = res.data.apikey;
        this.apiKeyValid = true;
        this.username = res.data.username;
        sucess();
      }).catch((err) => {
        error(err);
      });
    });
  }

  Register(username, password, invitekey = false) {
    return new Promise((sucess, error) => {
      if (serverInfo.inviterequired && !invitekey) return error(new SpeercsErrors.NoInviteError());
      this.axios.post("/auth/register").then((res) => {
        if (res.status != 200) return error(new SpeercsErrors.WtfError());
        this.apiKey = res.data.apiKey;
        this.apiKeyValid = true;
        this.username = res.data.username;
        sucess();
      }).catch((err) => {
        error(err);
      });
    });
  }

  GetRoom(x, y) {
    return new Promise((sucess, error) => {
      if (!this.apiKey) return error(new SpeercsErrors.KeyError());
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
}

class SpeercsErrors {
  static NoInviteError() {
    this.message = "Invite key is required, but not provided.";
    this.name = "ErrNoInvite";
  }
  static WtfError() {
    this.message = "No clue. Something bad happened I guess?";
    this.name = "ErrSomethingHappened";
  }
  static CredentialError() {
    this.message = "Invalid credentials provided";
    this.name = "ErrInvalidCredentials";
  }
  static KeyError() {
    this.message = "API key is not set or invalid";
    this.name = "ErrApiKey";
  }
}