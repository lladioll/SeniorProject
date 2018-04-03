<template>
<div>
   <!-- Cards -->
  <q-transition
    appear
    enter="fadeIn"
    leave="fadeOut">

    <div class="row justify-center" style="padding:2%;"> 
      <q-card inline :key="index" v-for="(tech, index) in technicians">
      <q-item>
        <img src="~assets/boy-avatar.png" alt="Avatar" class="avatar">
        <q-item-main>
          <div class="row">
            <div>{{tech.firstname}} {{tech.lastname}}</div>
            <div class="status"> </div>
          </div>
          <div class="row">
            <div>Technician</div>
          </div>
        </q-item-main>
      </q-item>
       <q-card-separator />

       <q-card-main>
        <q-card-title>
      <b>Technician Service Details</b>
      <!--    <q-btn style="float:right">
        Click to Edit 
          <q-icon name="edit" />
        </q-btn> -->
      </q-card-title>
  
        <q-field 
          icon="school"
          label="Buildings">
        <q-option-group
          color="secondary"
          type="checkbox"
          v-model="select[index]"
          :options="siteOptions"/>
        </q-field>
        <q-field v-for="(building, buildingIndex) in allRooms" :key="building.buildingid"
          icon="school"
          :label="building.buildingname">
          <q-checkbox v-for="(rooms, roomIndex) in allRooms[buildingIndex].roomnum" :key="roomIndex" v-model="roomselect[index][0]" :label="rooms" />
        </q-field>
     </q-card-main>
       <q-card-separator />
      <q-card-actions>
        <q-btn flat>Assign Locations</q-btn>
        <q-btn flat>Assign Tickets</q-btn>
        <q-btn flat round color="primary">
        <q-icon name="settings power" />
      </q-btn>
      </q-card-actions>
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
  QField,
  QIcon,
  QCheckbox,
  QOptionGroup,
  QList,
  QToggle,
  QListHeader,
  QItem,
  QItemSide,
  QItemMain,
  QCollapsible,
  QCard,
  QCardSeparator,
  QDialogSelect,
  QCardTitle,
  QCardMain,
  QPopover,
  QCarousel,
  QSelect,
  QTabs,
  QTab,
  QInput,
  QTabPane,
  QTransition,
  QItemTile,
  QCardMedia,
  QCardActions
} from 'quasar'

export default {
  name: 'technicians',
  components: {
    QLayout,
    QCheckbox,
    QCardSeparator,
    QToolbar,
    QOptionGroup,
    QField,
    QToolbarTitle,
    QBtn,
    QToggle,
    QCardMain,
    QDialogSelect,
    QIcon,
    QInput,
    QList,
    QListHeader,
    QSelect,
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
    QItemTile,
    QCardMedia,
    QCardActions
  },
  data () {
    return {
      technicians: [],
      service: [],
      sites: [],
      select: [],
      roomselect: [],
      rooms: [],
      deactive: false
    }
  },
  computed: {
    allRooms () {
      var rooms = JSON.parse(JSON.stringify(this.rooms))
      var output = rooms.reduce(function (o, cur) {
        var occurs = o.reduce(function (n, item, i) {
          return (item.buildingid === cur.buildingid) ? i : n
        }, -1)
        if (occurs >= 0) {
          o[occurs].roomnum = o[occurs].roomnum.concat(cur.roomnum)
        }
        else {
          var obj = {buildingname: cur.buildingname, buildingid: cur.buildingid, roomnum: [cur.roomnum]}
          o = o.concat([obj])
        }
        return o
      }, [])
      return output
    },
    siteOptions () {
      var sites = []
      this.sites.forEach(element => {
        sites.push({label: element.sitename, value: element.siteid})
      })
      return sites
    },
    mergedRooms () {
      var rooms = JSON.parse(JSON.stringify(this.service))
      var output = rooms.reduce(function (o, cur) {
        var occurs = o.reduce(function (n, item, i) {
          return (item.userid === cur.userid) ? i : n
        }, -1)
        if (occurs >= 0) {
          o[occurs].roomnum = o[occurs].roomnum.concat(cur.roomnum)
        }
        else {
          var obj = {userid: cur.userid, buildingid: [cur.buildingid], roomnum: [cur.roomnum]}
          o = o.concat([obj])
        }
        return o
      }, [])
      return output
    }
  },
  methods: {
    GetSites () {
      fetch('api/systeminfo/sitelist', {
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
      })
    },
    GetRooms () {
      fetch('api/systeminfo/rooms', {
        headers: {
          'Accept': 'application/json',
          'cache-control': 'no-cache'
        },
        credentials: 'same-origin',
        method: 'GET'
      }).then(response => {
        return response.json()
      }).then(json => {
        this.rooms = json
      })
    },
    GetAllTechs () {
      fetch('api/systeminfo/technicians', {
        headers: {
          'Accept': 'application/json',
          'cache-control': 'no-cache'
        },
        credentials: 'same-origin',
        method: 'GET'
      }).then(response => {
        return response.json()
      }).then(json => {
        this.technicians = json
        for (var i = 0; i < this.technicians.length; i++) {
          var techFilter = this.service.filter(tech => tech.userid === this.technicians[i].userid)
          this.select.push([...new Set(techFilter.map(tech => tech.buildingid))])
        }
        for (var j = 0; j < this.mergedRooms.length; j++) {
          this.roomselect.push([...new Set(this.mergedRooms[j].roomnum)])
        }
      })
    },
    GetService () {
      fetch('api/systeminfo/service', {
        headers: {
          'Accept': 'application/json',
          'cache-control': 'no-cache'
        },
        credentials: 'same-origin',
        method: 'GET'
      }).then(response => {
        return response.json()
      }).then(json => {
        this.service = json
        this.GetAllTechs()
      })
    }
  },
  created () {
    this.GetSites()
    this.GetService()
    this.GetRooms()
  }
}
</script>

<style lang="stylus" scoped>
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
img.avatar {
    vertical-align: middle;
    width: 40px;
    height: 40px;
    border-radius: 50%;
    margin-right: 10px;
}
.status {
  height: 20px;
  width: 20px;
  background-color: #42f44e;
  border-radius: 50%;
  right:20px; 
  position: absolute; 
  margin-top: 1%
}
</style>