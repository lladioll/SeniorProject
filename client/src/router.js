import Vue from 'vue'
import Router from 'vue-router'
import firebase from 'firebase'

Vue.use(Router)

function load (component) {
  // '@' is aliased to src/components
  return () => import(`@/${component}.vue`)
}

let router = new Router({

  mode: 'hash',
  scrollBehavior: () => ({ y: 0 }),

  routes: [
    {
      path: '/',
      component: load('Layout'),
      children: [
        {
          path: 'welcome',
          component: load('Welcome')
        }, {
          path: 'sitemap',
          component: load('SiteMap'),
          meta: {
            requiresAuth: true
          }
        }, {
          path: 'createticket',
          component: load('CreateTicket'),
          meta: {
            requiresAuth: true
          }
        }, {
          path: 'profile',
          component: load('Profile'),
          meta: {
            requiresAuth: true
          }
        }, {
          path: 'technicians',
          component: load('Technicians'),
          meta: {
            requiresAuth: true
          }
        }, {
          path: 'sites',
          component: load('Sites'),
          meta: {
            requiresAuth: true
          }
        }, {
          path: 'sites/:siteid',
          component: load('SiteMap'),
          meta: {
            requiresAuth: true
          }
        }, {
          path: 'addsites',
          component: load('AddSites'),
          meta: {
            requiresAuth: true
          }
        }, {
          path: 'ticketinfo',
          component: load('TicketInfo'),
          meta: {
            requiresAuth: true
          }
        }, {
          path: 'addmachine',
          component: load('AddMachine'),
          meta: {
            requiresAuth: true
          }
        }
      ]
    },
    // Always leave this last one
    { path: '*', component: load('Error404') } // Not found
  ]
})

router.beforeEach((to, from, next) => {
  let currentUser = firebase.auth().currentUser
  let requiresAuth = to.matched.some(record => record.meta.requiresAuth)

  if (requiresAuth && !currentUser) next('welcome')
  else if (!requiresAuth && currentUser) next('profile')
  else next()
})

export default router
