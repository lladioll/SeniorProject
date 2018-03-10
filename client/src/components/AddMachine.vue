<template>
<div>
  <!-- Get Machine Button -->
  <div class="row justify-center">
    <q-btn style icon="my_location" class="button is-primary" @click="GetCurrentMachineInfo">Get Current Machine</q-btn>
  </div>
  <!-- Display Machine Card -->
  <q-card class="card" v-if="JSON.stringify(machine) !== '{}'">
    <q-card-title style="background-color: #007471; text-align: center; color: white">
    Machine Information
    </q-card-title>
    <q-card-separator />
    <q-card-main icon="desktop windows" style="padding-left: 10%; padding-right:10%" >
      <q-field>
        <q-input v-model="machine.computerName" float-label="Machine Name" />
      </q-field>
      <q-field icon="settings system daydream
">
        <q-input v-model="machine.operatingSystem" float-label="Operating System" />
      </q-field>
      <q-field icon="hd">
        <q-input v-model="machine.hdd"  float-label="HDD Size" />
      </q-field>
      <q-field icon="equalizer">
        <q-input v-model="machine.usedhdd"  float-label="HDD Used" />
      </q-field>
      <q-field icon="memory">
        <q-input v-model="machine.ram"  float-label="RAM" />
      </q-field>
      <q-field icon="border_horizontal">
        <q-input v-model="machine.architecture"  float-label="Site Latitude" />
      </q-field>
    </q-card-main>
    <div class="row justify-center">
      <q-btn style="margin-top:2%" @click="AddMachine">Add MAchine</q-btn>
    </div>
  </q-card>
  <!-- Show Software Card -->
  <q-card class="card" v-if="JSON.stringify(machine) !== '{}'">
    <q-card-title style="background-color: #007471; text-align: center; color: white">
      Current Software List
    </q-card-title>
    <q-card-separator />
    <q-card-main  style="padding-left: 5%; padding-right:5%" >
      <q-data-table
      :data="software"
      :config="config"
      :columns="columns"> 
      </q-data-table>
    </q-card-main>
    <div class="row justify-center">
      <q-btn style="margin-top:2%" @click="AddMachine">Add Site</q-btn>
    </div>
  </q-card>
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
  QPopover,
  QCarousel,
  QTabs,
  QTab,
  QTabPane,
  QTransition,
  QInput,
  QField,
  QCardMain,
  QCardSeparator,
  QDataTable
} from 'quasar'

export default {
  name: 'addmachine',
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
    QCardSeparator,
    QDataTable
  },
  data () {
    return {
      machine: {},
      localIP: '',
      software: [{
        software: ''
      }],
      config: {
        title: 'Data Table',
        bodyStyle: {
          maxHeight: '500px'
        }
      },
      columns: [{
        label: 'Software',
        field: 'software',
        filter: true,
        sort: true,
        type: 'string',
        width: '500px'
      }]
    }
  },
  mounted () {
  },
  methods: {
    GetCurrentMachineInfo () {
      fetch('api/systeminfo/machine', {
        headers: {
          'Accept': 'application/json',
          'cache-control': 'no-cache'
        },
        credentials: 'same-origin',
        method: 'GET'
      }).then(response => {
        return response.json()
      }).then(json => {
        this.machine = json
        this.GetCurrentMachineSoftware()
      })
    },
    GetCurrentMachineSoftware () {
      fetch('api/systeminfo/software', {
        headers: {
          'Accept': 'application/json',
          'cache-control': 'no-cache'
        },
        credentials: 'same-origin',
        method: 'GET'
      }).then(response => {
        return response.json()
      }).then(json => {
        this.software = json
      })
    },
    AddMachine () {
      fetch('/api/systeminfo/machine', {
        method: 'POST',
        credentials: 'same-origin',
        headers: {
          'Content-Type': 'application/json'
        },
        body: JSON.stringify(this.machine)
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