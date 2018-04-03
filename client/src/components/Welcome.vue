<template>
<div>
  <q-carousel arrows dots actions class="text-white">
    <div slot="slide" class="bg-primary centered">
      {{message}}
    </div>
    <div slot="slide" class="bg-secondary centered">
      Slide 2
    </div>
    <div slot="slide" class="bg-tertiary centered">
      Slide 3
    </div>
  </q-carousel>
  <q-card inline>
    <q-card-media>
      <img src="~assets/conklin.jpg"/>
      <q-card-title slot="overlay">
        Farmingdale Help Desk
        <span slot="subtitle">Subtitle</span>
      </q-card-title>
    </q-card-media>
    <q-card-main>
      {{message}}
    </q-card-main>
  </q-card>
</div>
</template>

<script>
import {
  Alert,
  QField,
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
  QPopover,
  QInput,
  QCard,
  QCardTitle,
  QCardActions,
  QCardMedia,
  QRating,
  QCardMain,
  QCardSeparator,
  QModal,
  QCarousel
} from 'quasar'

export default {
  inject: ['layout'],
  name: 'welcome',
  components: {
    Alert,
    QField,
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
    QPopover,
    QInput,
    QCard,
    QCardTitle,
    QCardActions,
    QCardMedia,
    QRating,
    QCardMain,
    QCardSeparator,
    QModal,
    QCarousel
  },
  data () {
    return {
      message: ''
    }
  },
  mounted () {
  },
  created () {
    this.getMotD()
  },
  methods: {
    getMotD () {
      fetch('api/systeminfo/motd/', {
        headers: {
          'Accept': 'application/json',
          'cache-control': 'no-cache'
        },
        credentials: 'same-origin',
        method: 'GET'
      }).then(response => {
        return response.json()
      }).then(json => {
        this.message = json.message
      })
    }
  }
}
</script>

<style lang="stylus">
.q-toolbar {
  background-color: #007471;
}
.wrapper {
  margin:5%
}
.caption {
  font-size: 40px;
}
</style>
