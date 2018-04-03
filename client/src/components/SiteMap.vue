<template>
<div>
  <gmap-map
    :center="center"
    :zoom="18"
    :tilt="45"
    map-type-id="satellite"
    style="width: 100%; height: calc(100vh - 50px);">
    <gmap-marker
      :key="index"
      v-for="(m, index) in markers"
      :position="m.position"
      :clickable="true"
      @click="showing = true">
    </gmap-marker>
  </gmap-map>
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
    this.GetSites()
  },
  methods: {
    GetSites () {
      fetch('api/systeminfo/sites', {
        headers: {
          'Accept': 'application/json',
          'cache-control': 'no-cache'
        },
        credentials: 'same-origin',
        method: 'GET'
      }).then(response => {
        return response.json()
      }).then(json => {
        this.sites = json
        var sites = json
        this.center.lat = this.sites[0].latitude
        this.center.lng = this.sites[0].longitude
        for (var site in sites) {
          var positions = {
            position: {
              lat: sites[site].latitude,
              lng: sites[site].longitude
            }
          }
          this.markers.push(positions)
        }
      })
    }
  },
  data () {
    return {
      showing: false,
      sites: [],
      center: {
        lat: 0,
        lng: 0
      },
      markers: []
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
.vue-map {
  overflow: visible
}
</style>
