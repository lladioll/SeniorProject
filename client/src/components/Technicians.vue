<template>
<div>
   <!-- Cards -->
  <q-transition
    appear
    enter="fadeIn"
    leave="fadeOut">
    <div class="row justify-center" style="padding:2%;"> 
      <q-card :key="index" v-for="(tech, index) in technicians" class="card" inline style="width: 300px">
        <a href="google.com">
        <div class="q-card-media relative-position">
          <img src="~/assets/header.jpg">
        </div>
          </a>
        <div class="q-card-primary q-card-container row no-wrap">
          <div class="col column">
            <div class="q-card-title">
            {{tech.firstname}}
            </div>
            <div class="q-card-subtitle">
              <span>Newsletter</span>
            </div>
          </div>
          <div class="col-auto self-center q-card-title-extra"></div>
        </div>
      </q-card>
    </div>
  </q-transition>

</div>
</template>

<script>
import * as Firebase from 'firebase'
import 'quasar-extras/animate/fadeIn.css'
import 'quasar-extras/animate/fadeOut.css'
import {
  openURL,
  QLayout,
  QToolbar,
  QToolbarTitle,
  QBtn,
  QIcon,
  QList,
  QListHeader,
  QItem,
  QItemSide,
  QItemMain,
  QCollapsible,
  QCard,
  QCardTitle,
  QPopover,
  QCarousel,
  QTabs,
  QTab,
  QTabPane,
  QTransition
} from 'quasar'

export default {
  name: 'technicians',
  components: {
    QLayout,
    QToolbar,
    QToolbarTitle,
    QBtn,
    QIcon,
    QList,
    QListHeader,
    QItem,
    QItemSide,
    QItemMain,
    QCollapsible,
    QCard,
    QCardTitle,
    QPopover,
    QCarousel,
    QTabs,
    QTab,
    QTabPane,
    QTransition
  },
  data () {
    return {
      technicians: []
    }
  },
  computed: {
  },
  methods: {
    launch (url) {
      openURL(url)
    }
  },
  mounted () {
    var ref = Firebase.database().ref('technicians')
    this.technicians = []
    return ref.on('value', snapshot => {
      snapshot.forEach(userSnapshot => {
        this.technicians.push(userSnapshot.val())
      })
    })
  }
}
</script>

<style lang="stylus">
.card-wrapper {
  padding: 2%;
}
.q-carousel-track > div {
  min-height: 100px;
}
.card {
    box-shadow: 0 4px 8px 0 rgba(0,0,0,0.2);
    transition: 0.3s;
}
.card:hover {
    box-shadow: 0 10px 25px 0 rgba(0,0,0,0.2);
}
</style>