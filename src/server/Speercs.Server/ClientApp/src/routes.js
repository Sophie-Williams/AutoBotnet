
import Landing from './pages/Landing.vue'
import Login from './pages/Login.vue'
import Register from './pages/Register.vue'
import Logout from './pages/Logout.vue'
import Dashboard from './pages/Dashboard.vue'
import CodeEditor from './pages/CodeEditor.vue'
import WorldMap from './pages/WorldMap.vue'
import Account from './pages/Account.vue'

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
    path: '/register',
    name: 'register',
    component: Register
  },
  {
    path: '/logout',
    name: 'logout',
    component: Logout
  },
  {
    path: '/dashboard',
    name: 'dashboard',
    component: Dashboard,
    meta: { requiresAuth: true }
  },
  {
    path: '/g/map',
    name: 'worldmap',
    component: WorldMap,
    meta: { requiresAuth: true }
  },
  {
    path: '/g/editor',
    name: 'codeeditor',
    component: CodeEditor,
    meta: { requiresAuth: true }
  },
  {
    path: '/u',
    name: 'account',
    component: Account,
    meta: { requiresAuth: true }
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
