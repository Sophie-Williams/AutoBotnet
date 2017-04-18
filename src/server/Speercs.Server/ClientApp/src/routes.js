
import Landing from './pages/Landing.vue'
import Login from './pages/Login.vue'
import Dashboard from './pages/Dashboard.vue'

const main = [
  {
    path: '/',
    name: 'landing',
    component: Landing
  },
  {
    path: '/login',
    name: 'login',
    component: Login
  },
  {
    path: '/dashboard',
    name: 'dashboard',
    component: Dashboard
  }
]

const error = [
//   {
//     path: '*',
//     name: 'error',
//     component: NotFound
//   }
]

export default [].concat(main, error)
