
import Landing from './pages/Landing.vue'

const main = [
  {
    path: '/',
    name: 'landing',
    component: Landing
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
