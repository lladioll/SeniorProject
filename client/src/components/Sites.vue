<template>
<div>
  <q-transition
    appear
    enter="fadeIn"
    leave="fadeOut">
    <div class="row justify-center"> 
      <q-card :key="index" v-for="(site, index) in sites" class="card" inline style="width: 300px">
        <div class="side">
         
            <div class="flipframe">
              <div class="side"> 
                <div class="q-card-media relative-position">
                  <gmap-map
                    :center="markers[index].position"
                    :zoom="12"
                    map-type-id="terrain"
                    style="width: 100%; height: 220px">
                    <gmap-marker
                      :position="markers[index].position"
                      @click="showing = true">
                    </gmap-marker>
                  </gmap-map>
                </div>
              </div>
              <div class="side back">
                <div class="column justify-center"> 
                  <div class="row justify-center"> 
                    <p style="text-align:center;font-size: 20px"><b>Site Statistics</b></p>
                  </div>
                  <div class="row justify-center"> 
                    <q-knob
                      v-model="site.closedtickets"
                      size="100px"
                      style="font-size: 2rem"
                      color="secondary"
                      track-color="red"
                      line-width="5px"
                      :min="0"
                      :max="site.totaltickets">{{site.totaltickets}}
                      <q-icon name="description" />
                    </q-knob>
                  </div>
                  <div class="row justify-center" style="padding-top: 10%">
                    Rooms Count: {{site.roomcount}}
                  </div>
                  <div class="row justify-center">
                    Machine Count: {{site.machinecount}}
                  </div>
                </div>
              </div>
            </div>
         
          <div class="q-card-primary q-card-container row no-wrap" style="padding-top: 3px;">
            <div class="col column">
              <div class="q-card-title" style="text-align:center;">{{site.sitename}}</div>
                <div class="q-card-subtitle">
                  <router-link :to="{ name: 'site', params: { siteid: site.siteid }}">
                    <q-btn color="primary" class="full-width">View Site</q-btn>
                  </router-link>
                </div>
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
  QKnob,
  QPopover,
  QCarousel,
  QTabs,
  QTab,
  QTabPane,
  QTransition,
  QInput,
  QField
} from 'quasar'

export default {
  name: 'sites',
  components: {
    QLayout,
    QToolbar,
    QToolbarTitle,
    QBtn,
    QKnob,
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
    QField
  },
  data () {
    return {
      sites: [],
      newsite: {
        sitename: '',
        longitude: 0,
        latitude: 0
      },
      min: 0,
      center: {},
      markers: []
    }
  },
  computed: {
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

<style lang="stylus" scoped>

.card {
  box-shadow: 0 4px 8px 0 rgba(0,0,0,0.2);
  transition: 0.3s;
  height: 325px;
}
.card:hover {
  box-shadow: 0 10px 25px 0 rgba(0,0,0,0.2);
}
.card .flipframe {
  height: 230px;
  transform-style: preserve-3d;
  transition: all 1s ease-in-out;
  width: 300px;
}
.flipframe:hover {
  transform: rotateY(180deg);
}
.flipframe .side {
  backface-visibility: hidden;
  border-radius: 2px;
  height: 230px;
  position: absolute;
  width: 300px;
}
.flipframe .back {
  background: white;
  transform: rotateY(180deg);
  display: flex;
  align-items: center;
  justify-content: center;
}

</style>