<template>
<div>
  <div class="full-card-wrapper">
    <q-card inline style="width: 100%;">
      <q-card-media>
        <gmap-map v-if="JSON.stringify(site) !== '{}'"
          :center="siteLocation"
          :zoom="18"
          map-type-id="satellite"
          style="width: 100%; height: 300px;">
        </gmap-map>
      <!-- Cool Pano View
      <gmap-street-view-panorama class="pano" :position="{lat: 40.7551118, lng: -73.4295374}"
        :pov="pov" :zoom="1" @pano_changed="updatePano" @pov_changed="updatePov">
      </gmap-street-view-panorama> -->
      </q-card-media>
      <q-card-title>
        {{site.sitename}}
        <div slot="right" class="row items-center">
          <q-btn @click="getDriveInfo(siteLocation.lat, siteLocation.lng)" flat color="primary">
          <q-icon name="place" />{{distance}}
        </q-btn>
        </div>
      </q-card-title>
      <q-card-main>
        <p>Building # {{siteid}}</p>
        <p>Room Count </p>
        <p>Technicians </p>
        <p>Machines </p>
        <p>Tickets </p>
      </q-card-main>
      <q-card-separator />
      <q-card-actions>
        <q-btn flat round small><q-icon name="event" /></q-btn>
        <q-btn flat color="primary">View Rooms</q-btn>
        <q-btn flat color="primary" @click="getDistance">View Inventory</q-btn>
      </q-card-actions>
    </q-card>
  </div>
</div>
</template>

<script>
/* global google */
import {
  Toast,
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
    this.GeoLocation()
  },
  data () {
    return {
      site: {},
      siteLocation: {},
      currentLocation: {},
      distance: '',
      duration: ''
    }
  },
  methods: {
    getDriveInfo (lat, lng) {
      Toast.create({
        html: this.duration,
        icon: 'directions car',
        timeout: 5000,
        color: '#f8c1c1',
        button: {
          label: 'Open In Google Maps',
          handler () {
            window.location.href = 'https://www.google.com/maps/?q=' + lat + ',' + lng
          },
          color: 'white'
        }
      })
    },
    GeoLocation () {
      navigator.geolocation.getCurrentPosition((position) => {
        this.currentLocation = {
          lat: position.coords.latitude,
          lng: position.coords.longitude
        }
        this.getDistance()
      })
    },
    getDistance () {
      let self = this
      var convertOrigin = new google.maps.LatLng(this.currentLocation.lat, this.currentLocation.lng)
      var convertDest = new google.maps.LatLng(this.siteLocation.lat, this.siteLocation.lng)
      var service = new google.maps.DistanceMatrixService()
      service.getDistanceMatrix({
        origins: [convertOrigin],
        destinations: [convertDest],
        travelMode: google.maps.TravelMode.DRIVING,
        unitSystem: google.maps.UnitSystem.IMPERIAL,
        avoidHighways: false,
        avoidTolls: false
      }, function (response, status) {
        if (status !== google.maps.DistanceMatrixStatus.OK) {
          alert('Error was: ' + status)
        }
        else {
          self.distance = response.rows[0].elements[0].distance.text
          self.duration = response.rows[0].elements[0].duration.text
        }
      })
    },
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
        this.siteLocation = {
          lat: json.latitude,
          lng: json.longitude
        }
      })
    }
  }
}
</script>

<style lang="stylus">
.q-toolbar {
  background-color: #007471;
}
.full-card-wrapper {
  display: flex;
  justify-content: center;
  align-items: center;

}
.caption {
  font-size: 40px;
}
.pano {
  width: 500px;
  height: 300px;
}
</style>
