/**
 * Speercs Client
 * SPA app client version
 */

import axios from 'axios'

export class SpeercsApi {
  constructor(endpoint, key = null) {
    this.ep = endpoint + '/a'
    this.init(key)
  }

  static create(endpoint, apikey = null) {
    return new Promise((resolve, reject) => {
      let api = new SpeercsApi(endpoint, apikey)
      resolve(api)
    })
  }

  init(key = null) {
    this.key = key
    this.username = null
    this.ax()
  }

  ax() {
    this.axios = axios.create({
      baseURL: this.ep,
      headers: {
        Authorization: this.key
      }
    })
  }

  /* actions */

  login(un, pw) {
    return new Promise((resolve, reject) => {
      this.ax()
      this.axios.post('/auth/login', {
        username: un,
        password: pw
      }).then((res) => {
        this.key = res.data.apikey
        this.username = res.data.username
        resolve()
      }).catch((err) => {
        if (err.response && err.response.data) {
          reject(new SpeercsApiErrors.CredentialError(err, err.response.data))
        } else {
          reject(new SpeercsApiErrors.CredentialError(err))
        }
      })
    })
  }

  reauth(un, key) {
    return new Promise((resolve, reject) => {
      this.ax()
      this.axios.post('/auth/reauth', {
        username: un,
        apikey: key
      }).then((res) => {
        this.key = res.data.apikey
        this.username = res.data.username
        resolve()
      }).catch((err) => {
        reject(new SpeercsApiErrors.CredentialError(err))
      })
    })
  }

  register(un, pw, i = null) {
    return new Promise((resolve, reject) => {
      this.ax()
      this.axios.post('/auth/register', {
        username: un,
        password: pw,
        invitekey: i
      }).then((res) => {
        this.key = res.data.apikey
        this.username = res.dataname
        resolve()
      }).catch((err) => {
        if (err.response && err.response.data) {
          reject(new SpeercsApiErrors.CredentialError(err, err.response.data))
        } else {
          reject(new SpeercsApiErrors.CredentialError(err))
        }
      })
    })
  }

  logout() {
    return new Promise((resolve, reject) => {
      this.key = null
      this.init()
      resolve()
    })
  }

  regenApiKey() {
    this.ax()
    return this.axios.patch('/auth/newkey')
  }

  changePassword(old, newp) {
    this.ax()
    return this.axios.patch('/auth/changepassword', {
      username: this.username,
      oldPassword: old,
      newPassword: newp
    })
  }

  /* information */
  getMeta() {
    this.ax()
    return this.axios.get('/meta')
  }

  getInfo() {
    this.ax()
    return this.axios.get('/info')
  }

  /* -------- game -------- */

  /* user code */

  getUserCode() {
    this.ax()
    return this.axios.get('/game/code/get')
  }

  reloadUserCode() {
    return this.axios.patch('/game/code/reload')
  }

  deployUserCode(src) {
    this.ax()
    return this.axios.post('/game/code/deploy', {
      source: src
    })
  }

  /* getters */

  getKey() { return this.key }

}

class SpeercsError {
  constructor(data = null, dsc, msg = null) {
    this.data = data
    this.description = dsc
    this.message = msg
  }
}

class SpeercsApiErrors {
  static CredentialError(d, m) {
    return new SpeercsError(d, 'invalid credentials', m)
  }
  static KeyError(d, m) {
    return new SpeercsError(d, 'invalid api key', m)
  }
}