import Vue from 'vue'
import VueRouter from 'vue-router'
import Home from '../views/Home.vue'
import Logout from '../views/Logout.vue';
import requireAuth from '../helpers/requireAuth';
import MetronomeMap from '../views/About.vue';

Vue.use(VueRouter)

const routes = [
  {
    path: '*',
    name: '/',
    component: Home
  },
  {
    path: '/about',
    name: 'about',
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
  routes
})

export default router
