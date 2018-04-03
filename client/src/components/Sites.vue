<template>
<div>
  <q-transition
    appear
    enter="fadeIn"
    leave="fadeOut">
    <div class="row justify-center"> 
      <q-card class="card" :key="index" v-for="(site, index) in sites" inline style="width: 300px">
        <div class="flip-container">
          <div class="flipper">
            <div class="front"> 
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
            <div class="back">
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
                <div class="row justify-center" style="padding-top: 5%">
                  Rooms Count: {{site.roomcount}}
                </div>
                <div class="row justify-center">
                  Machine Count: {{site.machinecount}}
                </div>
              </div>
            </div>
          </div>
        </div>
        <div class="q-card-primary q-card-container row no-wrap" style="padding-top: 4px;">
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
      position: {},
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
.flip-container {
  -webkit-perspective: 1000;
    -moz-perspective: 1000;
    -o-perspective:1000;
    -ms-perspective: 1000;
  perspective: 1000;
    -ms-transform: perspective(1000px);
    -moz-transform: perspective(1000px);
    -moz-transform-style: preserve-3d; 
    -ms-transform-style: preserve-3d; 
    position: relative;	
    height: 230px;
}
  /* for IE */
.flip-container:hover .back, .flip-container.hover .back {
  -webkit-transform: rotateY(0deg);
    -moz-transform: rotateY(0deg);
    -o-transform: rotateY(0deg);
    -ms-transform: rotateY(0deg);
    transform: rotateY(0deg);
}
.flip-container:hover .front, .flip-container.hover .front {
  -webkit-transform: rotateY(180deg);
    -moz-transform: rotateY(180deg);
    -o-transform: rotateY(180deg);
  transform: rotateY(180deg);
}

/* END: for IE */
.flipper {
  -webkit-transition: 0.6s;
  -webkit-transform-style: preserve-3d;
    -ms-transition: 0.6s;
    -moz-transition: 0.6s;
    -moz-transform: perspective(1000px);
    -moz-transform-style: preserve-3d;
    -ms-transform-style: preserve-3d;
  transition: 0.6s;
  transform-style: preserve-3d;
  position: relative;
  top: 0;
  left: 0;
  width: 300px;
  height: 230px;
  -webkit-transition: all 0.4s ease-in-out;
    -moz-transition: all 0.4s ease-in-out;
    -o-transition: all 0.4s ease-in-out;
}

.front, .back {
  -webkit-backface-visibility: hidden;
    -moz-backface-visibility: hidden;
    -ms-backface-visibility: hidden;
  backface-visibility: hidden;
  -webkit-transition: 0.6s;
  -webkit-transform-style: preserve-3d;
    -moz-transition: 0.6s;
    -moz-transform-style: preserve-3d;
    -o-transition: 0.6s;
    -o-transform-style: preserve-3d;
    -ms-transition: 0.6s;
    -ms-transform-style: preserve-3d;
  transition: 0.6s;
  transform-style: preserve-3d;
  position: absolute;
  top: 0;
  left: 0;
  width: 300px;
  height: 230px;
}	
.front {
  -webkit-transform: rotateY(0deg);
    -ms-transform: rotateY(0deg);
  z-index: 2;
}
.back {
  padding-top: 7%;
  background: white;
  -webkit-transform: rotateY(-180deg);
    -moz-transform: rotateY(-180deg);
    -o-transform: rotateY(-180deg);
    -ms-transform: rotateY(180deg);
    transform: rotateY(-180deg);
}

</style>