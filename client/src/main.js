// === DEFAULT / CUSTOM STYLE ===
// WARNING! always comment out ONE of the two require() calls below.
// 1. use next line to activate CUSTOM STYLE (./src/themes)
require(`./themes/app.mat.styl`)
// 2. or, use next line to activate DEFAULT QUASAR STYLE
// require(`quasar/dist/quasar.mat.css`)
// ==============================

// Uncomment the following lines if you need IE11/Edge support
// require(`quasar/dist/quasar.ie`)
// require(`quasar/dist/quasar.ie.${__THEME}.css`)

import Vue from 'vue'
import Quasar from 'quasar'
import router from './router'
import * as VueGoogleMaps from 'vue2-google-maps'
import * as Firebase from 'firebase'

Vue.config.productionTip = false
Vue.use(Quasar) // Install Quasar Framework
let app
var config = {
  apiKey: '',
  authDomain: '',
  databaseURL: '',
  storageBucket: ''
}

Firebase.initializeApp(config)

Vue.use(VueGoogleMaps, {
  load: {
    key: '',
    libraries: 'places'
  }
})

if (__THEME === 'mat') {
  require('quasar-extras/roboto-font')
}
import 'quasar-extras/material-icons'
// import 'quasar-extras/ionicons'
// import 'quasar-extras/fontawesome'
// import 'quasar-extras/animate'

Quasar.start(() => {
  /* eslint-disable no-new */
  Firebase.auth().onAuthStateChanged(function (user) {
    if (!app) {
      app = new Vue({
        el: '#q-app',
        router,
        render: h => h(require('./App').default)
      })
    }
  })
})
