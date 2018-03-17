<template>
<div>
  <q-card inline style="width: 500px">
    <q-card-media>
      <img src="~assets/header.jpg">
    </q-card-media>
    <q-card-title>
      {{site.sitename}}
      <q-rating slot="subtitle" v-model="stars" :max="5" />
      <div slot="right" class="row items-center">
        <q-icon name="place" /> 250 ft
      </div>
    </q-card-title>
    <q-card-main>
      <p>$・Italian, Cafe</p>
      <p class="text-faded">Small plates, salads & sandwiches in an intimate setting.</p>
    </q-card-main>
    <q-card-separator />
    <q-card-actions>
      <q-btn flat round small><q-icon name="event" /></q-btn>
      <q-btn flat>5:30PM</q-btn>
      <q-btn flat>7:30PM</q-btn>
      <q-btn flat>9:00PM</q-btn>
      <q-btn flat color="primary">Reserve</q-btn>
    </q-card-actions>
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
  QModal
} from 'quasar'

export default {
  props: ['siteid'],
  name: 'sitemap',
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
    QModal
  },
  mounted () {
    this.GetSite()
  },
  methods: {
    GetSite () {
      fetch('api/systeminfo/site/' + this.siteid, {
        headers: {
          'Accept': 'application/json',
          'cache-control': 'no-cache'
        },
        credentials: 'same-origin',
        method: 'GET'
      }).then(response => {
        return response.json()
      }).then(json => {
        this.site = json
      })
    }
  },
  data () {
    return {
      site: {}
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
