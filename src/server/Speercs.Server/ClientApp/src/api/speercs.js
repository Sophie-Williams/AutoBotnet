/**
 * Speercs Client
 * SPA app client version
 */

import axios from 'axios'

export class SpeercsApi {
  constructor (endpoint, key = null) {
    this.ep = endpoint
    this.init(key)
  }

  static create (endpoint, apikey = null) {
    return new Promise((resolve, reject) => {
      let api = new SpeercsApi(endpoint, apikey)
      resolve(api)
    })
  }

  init (key = null) {
    this.key = key
    this.username = null
    this.ax()
  }

  ax () {
    this.axios = axios.create({
      baseURL: this.ep,
      headers: {
        Authorization: this.key
      },
      responseType: 'json'
    })
  }

  /* actions */
  login (un, pw) {
    return new Promise((resolve, reject) => {
      this.ax()
      this.axios.post('/auth/login', {
        username: un,
        password: pw
      }).then((res) => {
        this.key = res.data.apikey
        this.username = res.data.user.username
        resolve()
      }).catch((err) => {
        reject(err)
      })
    })
  }

  reauth (un, key) {
    return new Promise((resolve, reject) => {
      this.ax()
      this.axios.post('/auth/reauth', {
        username: un,
        apikey: key
      }).then((res) => {
        this.key = res.data.apikey
        this.username = res.data.user.username
        resolve()
      }).catch((err) => {
        reject(err)
      })
    })
  }

  register (un, pw, i = null) {
    return new Promise((resolve, reject) => {
      this.ax()
      this.axios.post('/auth/register', {
        username: un,
        password: pw,
        invitekey: i
      }).then((res) => {
        this.login(un, pw)
          .then(() => {
            resolve()
          })
      }).catch((err) => {
        reject(err)
      })
    })
  }

  logout () {
    return new Promise((resolve, reject) => {
      this.key = null
      this.init()
      resolve()
    })
  }

  regenApiKey () {
    this.ax()
    return this.axios.patch('/auth/newkey')
  }

  changePassword (old, newp) {
    return new Promise((resolve, reject) => {
      this.ax()
      this.axios.patch('/auth/changepassword', {
        username: this.username,
        oldPassword: old,
        newPassword: newp
      })
        .then((res) => {
          resolve(res)
        })
        .catch((e) => reject(e))
    })
  }

  /* getters */
  getKey() { return this.key }

}

class SpeercsErrors {
  static CredentialError () {
    return new Error('invalid credentials')
  }
  static KeyError () {
    return new Error('invalid api key')
  }
}