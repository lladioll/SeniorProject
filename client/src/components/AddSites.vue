<template>
<div>
  <q-card class="card">
    <q-card-title style="background-color: #007471; text-align: center; color: white">
      Add a New Site
    </q-card-title>
    <q-card-separator />
    <q-card-main style="padding-left: 10%; padding-right:10%">
      <q-field>
        <q-input v-model="searchAddressInput" float-label="Enter Site Address" placeholder="Enter Address" @change="searchLocation()" />
      </q-field>
      <div class="row justify-center">
    <q-btn style ="margin-bottom: 2%" icon="my_location" class="button is-primary" @click="geolocation()">Get Current Location</q-btn>
    </div>
    <gmap-map
      :center="{lat:currentLocation.lat, lng:currentLocation.lng}" 
      :zoom="17"
      map-type-id="terrain"
      style="width: 100%; height: 300px">
      <gmap-info-window :options="infoOptions" :position="infoWindowPos" :opened="infoWinOpen" @closeclick="infoWinOpen=false">
        {{infoContent}}
      </gmap-info-window>
      <gmap-marker
      :key="i" v-for="(m,i) in markers"
       @click="toggleInfoWindow(m,i)"
      :position="{lat:currentLocation.lat, lng:currentLocation.lng}"
      :clickable="true"
      :draggable="true"
       @dragend="getMarkerPosition($event.latLng)">
      </gmap-marker>
    </gmap-map>
    <q-field icon="edit_location">
      <q-input v-model="newsite.sitename" float-label="Enter Site Name" />
    </q-field>
    <q-field
      icon="border_vertical">
    <q-input v-model="newsite.longitude" float-label="Site Longitutde" />
    </q-field>
    <q-field
    icon="border_horizontal">
    <q-input v-model="newsite.latitude" float-label="Site Latitude" />
    </q-field>
    </q-card-main>
    <div class="row justify-center">
    <q-btn class="button is-primary" style="margin-bottom:4%" @click="AddSite()">Add Site</q-btn>
    </div>
  </q-card>
</div>
</template>

<script>
/* global google */
import 'quasar-extras/animate/fadeIn.css'
import 'quasar-extras/animate/fadeOut.css'
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
  name: 'addsites',
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
    QTransition,
    QInput,
    QField,
    QCardMain,
    QCardSeparator
  },
  data () {
    return {
      newsite: {
        sitename: '',
        longitude: null,
        latitude: null
      },
      infoContent: '',
      infoWindowPos: {
        lat: 0,
        lng: 0
      },
      infoWinOpen: false,
      currentMidx: null,
      infoOptions: {
        pixelOffset: {
          width: 0,
          height: -35
        }
      },
      markers: [{
        position: {
          lat: 0,
          lng: 0
        },
        infoText: 'Marker 1'
      }],
      address: '',
      currentLocation: {
        lat: 0,
        lng: 0
      },
      searchAddressInput: ''
    }
  },
  mounted () {
    this.geolocation()
  },
  methods: {
    getMarkerPosition (marker) {
      this.markers = []
      var geocoder = new google.maps.Geocoder()
      geocoder.geocode({'location': marker}, (results) => {
        var positions = {
          position: {
            lat: marker.lat(),
            lng: marker.lng()
          },
          infoText: results[0].formatted_address
        }
        this.markers.push(positions)
      })
      this.newsite.latitude = marker.lat()
      this.newsite.longitude = marker.lng()
      this.currentLocation.lat = marker.lat()
      this.currentLocation.lng = marker.lng()
    },
    toggleInfoWindow (marker, idx) {
      this.infoWindowPos = marker.position
      this.infoContent = marker.infoText
      if (this.currentMidx === idx) {
        this.infoWinOpen = !this.infoWinOpen
      }
      else {
        this.infoWinOpen = true
        this.currentMidx = idx
      }
    },
    searchLocation () {
      var geocoder = new google.maps.Geocoder()
      geocoder.geocode({'address': this.searchAddressInput}, (results, status) => {
        if (status === 'OK') {
          this.address = results[0].formatted_address
          this.currentLocation.lat = results[0].geometry.location.lat()
          this.currentLocation.lng = results[0].geometry.location.lng()
        }
        var positions = {
          position: {
            lat: this.currentLocation.lat,
            lng: this.currentLocation.lng
          },
          infoText: this.address
        }
        this.markers = []
        this.markers.push(positions)
      })
    },
    geolocation () {
      navigator.geolocation.getCurrentPosition((position) => {
        this.currentLocation = {
          lat: position.coords.latitude,
          lng: position.coords.longitude
        }
      })
      var positions = {
        position: {
          lat: this.currentLocation.lat,
          lng: this.currentLocation.lng
        },
        infoText: this.address
      }
      this.markers = []
      this.markers.push(positions)
    },
    AddSite () {
      fetch('/api/systeminfo/sites', {
        method: 'POST',
        credentials: 'same-origin',
        headers: {
          'Content-Type': 'application/json'
        },
        body: JSON.stringify(this.newsite)
      })
    }
  }
}
</script>

<style lang="stylus">
.card-wrapper {
  padding: 2%;
}
.card {
    box-shadow: 0 4px 8px 0 rgba(0,0,0,0.2);
    transition: 0.3s;
}
.card:hover {
    box-shadow: 0 10px 25px 0 rgba(0,0,0,0.2);
}
</style>