import Vue from 'vue'
import VueRouter from 'vue-router'
import requireAuth from '../helpers/requireAuth';
import Home from '../components/Index.vue'
import MetronomeMap from '../components/Map2.vue'
import { longStackSupport } from 'q';
import Logout from '../components/Logout.vue';

Vue.use(VueRouter)

const routes = [
  {
    path: '/',
    name: 'home',
    component: Home
  },
  {
    path: '/map',
    name: 'map',
    component: MetronomeMap,
    beforeEnter: requireAuth
  },
  {
    path: '/Logout',
    name: 'Logout',
    component: Logout,
    beforeEnter : requireAuth
  }
]

const router = new VueRouter({
  mode: 'history',
  base: process.env.BASE_URL,
  routes
})

export default router
