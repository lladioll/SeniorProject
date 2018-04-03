<template>
<div>
  <slick ref="slick" v-if="machines.length" :options="slickOptions">
 
    <q-card inline style="width: 300px;" v-for="machine in machines" :key="machine.computerName">
      <q-card-media>
        <img style="padding: 15px;" src="~assets/pc.png">
      </q-card-media>
      <q-card-title>
        {{machine.computerName}}
      </q-card-title>
      <q-card-main>
        <p>{{machine.operatingSystem}} {{machine.architecture}}</p>
        <p>RAM: {{machine.ram}}</p>
        <p>HDD: {{machine.usedhdd}}G / {{machine.hdd}}G</p>
        <p>{{machine.processor}}</p>
      </q-card-main>
      <q-card-separator />
      <q-card-actions>
        <q-btn flat round small><q-icon name="event" /></q-btn>
        <q-btn flat>Edit</q-btn>
        <q-btn flat>Delete</q-btn>
      </q-card-actions>
    </q-card>

</slick>
</div>
</template>

<script>
import Slick from 'vue-slick'
import 'slick-carousel/slick/slick.css'
import 'slick-carousel/slick/slick-theme.css'

import {
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
  QCardActions,
  QCardMedia,
  QRating,
  QCardTitle,
  QPopover,
  QCarousel,
  QTabs,
  QTab,
  QTabPane,
  QTransition,
  QInput,
  QField,
  QCardMain,
  QCardSeparator
} from 'quasar'

export default {
  name: 'machines',
  components: {
    Slick,
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
    QCardActions,
    QCardMedia,
    QRating,
    QCardTitle,
    QPopover,
    QCarousel,
    QTabs,
    QTab,
    QTabPane,
    QTransition,
    QInput,
    QField,
    QCardMain,
    QCardSeparator
  },
  data () {
    return {
      machines: [],
      slickOptions: {
        centerMode: true,
        slidesToShow: 3,
        dots: true,
        arrows: false,
        responsive: [
          {
            breakpoint: 768,
            settings: {
              arrows: false,
              centerMode: true,
              centerPadding: '40px',
              slidesToShow: 3
            }
          },
          {
            breakpoint: 480,
            settings: {
              arrows: false,
              centerMode: true,
              centerPadding: '40px',
              slidesToShow: 1
            }
          }
        ]
      }
    }
  },
  created () {
    this.GetMachines()
  },
  methods: {
    GetMachines () {
      fetch('api/systeminfo/machinelist', {
        headers: {
          'Accept': 'application/json',
          'cache-control': 'no-cache'
        },
        credentials: 'same-origin',
        method: 'GET'
      }).then(response => {
        return response.json()
      }).then(json => {
        this.machines = json
      })
    },
    next () {
      this.$refs.slick.next()
    },
    prev () {
      this.$refs.slick.prev()
    },
    reInit () {
      this.$nextTick(() => {
        this.$refs.slick.reSlick()
      })
    },
    handleAfterChange (event, slick, currentSlide) {
      console.log('handleAfterChange', event, slick, currentSlide)
    },
    handleBeforeChange (event, slick, currentSlide, nextSlide) {
      console.log('handleBeforeChange', event, slick, currentSlide, nextSlide)
    },
    handleBreakpoint (event, slick, breakpoint) {
      console.log('handleBreakpoint', event, slick, breakpoint)
    },
    handleDestroy (event, slick) {
      console.log('handleDestroy', event, slick)
    },
    handleEdge (event, slick, direction) {
      console.log('handleEdge', event, slick, direction)
    },
    handleInit (event, slick) {
      console.log('handleInit', event, slick)
    },
    handleReInit (event, slick) {
      console.log('handleReInit', event, slick)
    },
    handleSetPosition (event, slick) {
      console.log('handleSetPosition', event, slick)
    },
    handleSwipe (event, slick, direction) {
      console.log('handleSwipe', event, slick, direction)
    },
    handleLazyLoaded (event, slick, image, imageSource) {
      console.log('handleLazyLoaded', event, slick, image, imageSource)
    },
    handleLazeLoadError (event, slick, image, imageSource) {
      console.log('handleLazeLoadError', event, slick, image, imageSource)
    }
  }
}
</script>

<style lang="stylus">
    /* the slides */
  .slick-slide {
    margin: 0 10px;
  }
</style>