class SpeercsApi {
  constructor(endpoint, apiKey = null) {
    this.endpoint = endpoint;
    this.username = null;
    this.apiKeyValid = false;
    this.serverInfo = {
      name: "Loading...", motd: "Loading..."
    };
    this.apiKey = apiKey;
    this.axios = axios.create({
      baseURL: this.endpoint + "/a/",
      params: {
        apikey: this.apiKey
      },
      responseType: 'json'
    });
  }
  updateInfo() {
    axios.get(this.endpoint + "/meta").then((res) => {
      this.serverInfo = res.data;
    }).catch((err) => {
      throw err;
    });
    this.axios.get("/game/umeta/me", { responseType: 'text' }).then((res) => {
      if (res.status != 200) return apiKeyValid = false;
      this.username = res.data;
    })
  }
  login(username, password) {
    this.axios.post("/a/auth/login", {username: username, password: password}).then((res) => {
      this.apiKey = res.data.apikey;
      this.apiKeyValid = true;
      this.username = res.data.username;
    }).catch((err) => {
      throw err;
    })
  }
  register(username, password, invitekey = null) {

  }
}