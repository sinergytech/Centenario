import Vue from 'vue'
import VueRouter from 'vue-router'
import Rentas from '../views/Rentas.vue'
import DesarrolloUrbano from '../views/DesarrolloUrbano.vue'
import Cobranzas from '../views/Cobranzas.vue'

Vue.use(VueRouter)

const routes = [
  {
    path: '/',
    name: 'Rentas',
    component: Rentas
  },
  {
    path: '/desarrollo',
    name: 'Desarrollo',
    component: DesarrolloUrbano
  },
  {
    path: '/cobranzas',
    name: 'Cobranzas',
    component: Cobranzas
  },
  {
    path: '/about',
    name: 'About',
    // route level code-splitting
    // this generates a separate chunk (about.[hash].js) for this route
    // which is lazy-loaded when the route is visited.
    component: () => import(/* webpackChunkName: "about" */ '../views/About.vue')
  }
]

const router = new VueRouter({
  routes
})

export default router
