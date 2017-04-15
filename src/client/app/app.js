class SpeercsApi {
  constructor(endpoint, apiKey) {
    this.endpoint = endpoint;
    this.serverInfo = { 
      name: "Loading...", motd: "Loading..." 
    };
    this.apiKey = apiKey;
    this.axios = axios.create({
      baseURL: this.endpoint,
      params: {
        apikey: this.apiKey
      },
      responseType: 'json'
    })

    
  }
}