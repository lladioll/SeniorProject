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
      redirect: '/welcome',
      component: load('Layout'),
      children: [
        {
          path: 'welcome',
          component: load('Welcome')
        }, {
          path: 'sitemap',
          component: load('SiteMap'),
          meta: {
            requiresAuth: true,
            requiresAdmin: true
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
            requiresAuth: true,
            requiresAdmin: true
          }
        }, {
          path: 'addtechnician',
          component: load('AddTechnician'),
          meta: {
            requiresAuth: true,
            requiresAdmin: true
          }
        }, {
          path: 'adminticketview',
          component: load('AdminTicketView'),
          meta: {
            requiresAuth: true,
            requiresAdmin: true
          }
        }, {
          path: 'techticketview',
          component: load('TechTicketView'),
          meta: {
            requiresAuth: true,
            techAuth: true
          }
        }, {
          path: '/site/:siteid',
          props: true,
          name: 'site',
          component: load('Site'),
          meta: {
            requiresAuth: true,
            techAuth: true
          }
        }, {
          path: 'sites',
          component: load('Sites'),
          meta: {
            requiresAuth: true,
            techAuth: true
          }
        }, {
          path: 'admindashboard',
          component: load('AdminDashboard'),
          meta: {
            requiresAuth: true,
            requiresAdmin: true
          }
        }, {
          path: 'addsites',
          component: load('AddSites'),
          meta: {
            requiresAuth: true,
            techAuth: true
          }
        }, {
          path: 'viewtickets',
          component: load('ViewTickets'),
          meta: {
            requiresAuth: true
          }
        }, {
          path: 'addmachine',
          component: load('AddMachine'),
          meta: {
            requiresAuth: true,
            techAuth: true
          }
        }, {
          path: 'machines',
          component: load('Machines'),
          meta: {
            requiresAuth: true,
            techAuth: true
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
  let requiresAdmin = to.matched.some(record => record.meta.adminAuth)
  let requiresTech = to.matched.some(record => record.meta.techAuth)
  if (requiresAuth) {
    if (!currentUser) {
      next('welcome')
    }
    else if (requiresTech) {
      var userId = currentUser.uid
      firebase.database().ref('/users/' + userId).once('value').then(function (snapshot) {
        var role = (snapshot.val() && snapshot.val().role) || 'Unauthorized'
        console.log(role)
        if (role === 'Admin' || role === 'Technician') {
          next()
        }
      })
    }
    else if (requiresAdmin) {
      firebase.database().ref('/users/' + userId).once('value').then(function (snapshot) {
        var role = (snapshot.val() && snapshot.val().role) || 'Unauthorized'
        console.log(role)
        if (role === 'Admin') {
          next()
        }
      })
    }
    else next()
  }
  else next()
})

export default router
