
import Landing from './pages/Landing.vue'
import Login from './pages/Login.vue'
import Logout from './pages/Logout.vue'
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
    path: '/logout',
    name: 'logout',
    component: Logout
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
