<template>
<div> 
  <div class="top row justify-center"> 
    <h2>Create Ticket</h2>
  </div>

  <div class="row justify-center" style="width: 100%; padding: 20px">
      <q-field
        icon="report problem"
        label="Select Issue Type To Begin">
        <q-dialog-select
            v-model="issueType"
            :options="issueOptions"
            ok-label="Done"
            cancel-label="Cancel"
            title="Select Issue"/>
        </q-field>
      </div>

  

<div class="row justify-center" style="margin-top: 2%"> 
  <q-stepper v-if="issueType === 'Hardware' || issueType === 'Software' || issueType === 'Networking'" :vertical="vertical" class="stepper" color="secondary" ref="stepper" alternative-labels>
    <q-step default name="first" title="Choose a Location">
      <q-select
      v-model="ticket.site"
      float-label="Choose a Location"
      radio
      :options="siteOptions"
      @change="GetRooms(ticket.site), GetTechs(ticket.site)"/>

      <q-select v-if="ticket.site"
      v-model="ticket.room"
      float-label="Choose a Room"
      radio
      :options="roomOptions"
      @change="GetMachines(ticket.site, ticket.room)"/>
      <!-- Navigation for this step at the end of QStep-->
      <q-stepper-navigation>
        <q-btn color="secondary" @click="$refs.stepper.next()">Continue</q-btn>
      </q-stepper-navigation>
    </q-step> 
    <q-step name="second" title="Choose Malfunctioning Machine">
      <q-select
        v-model="ticket.machine"
        float-label="Choose a Machine"
        radio
      :options="machineOptions"/>
      <q-stepper-navigation>
        <q-btn color="secondary" @click="$refs.stepper.next()">Next</q-btn>
        <q-btn color="secondary" flat @click="$refs.stepper.previous()">Back</q-btn>
      </q-stepper-navigation>
    </q-step>
    <q-step v-if="role==='Faculty'" name="third" title="Choose a Technician">
      <q-select
        v-model="ticket.technician"
        float-label="Choose a Technician"
        radio
      :options="techOptions"/>
      <q-stepper-navigation>
        <q-btn color="secondary" @click="$refs.stepper.next()">Next</q-btn>
        <q-btn color="secondary" flat @click="$refs.stepper.previous()">Back</q-btn>
      </q-stepper-navigation>
    </q-step>
    <q-step name="fifth" title="Explain Issue">
      <q-input
        v-model="ticket.description"
        type="textarea"
        float-label="Textarea"
        :max-height="100"/>
      <q-stepper-navigation>
        <q-btn color="secondary" @click="$refs.stepper.next()">Next</q-btn>
        <q-btn color="secondary" flat @click="$refs.stepper.previous()">Back</q-btn>
      </q-stepper-navigation>
    </q-step>
    <q-step name="fourth" title="Review and Finalize">
      <div>Tech: {{ticket.technician}}</div>
      <div>Room: {{ticket.room}}</div>
      <div>Machine: {{ticket.machine}}</div>
      <div>Issue: {{ticket.description}}</div>
      <q-stepper-navigation>
        <q-btn color="secondary" @click="CreateTicket">Submit</q-btn>
        <q-btn color="secondary" @click="$refs.stepper.goToStep('first')">Restart</q-btn>
        <q-btn color="secondary" flat @click="$refs.stepper.previous()">Back</q-btn>
      </q-stepper-navigation>
    </q-step>
  </q-stepper>

  <q-stepper v-else-if="issueType === 'Email' || issueType === 'Blackboard'" :vertical="vertical" class="stepper" color="secondary" ref="stepper" alternative-labels>
    <q-step default name="first" title="Describe Your Issue">
      <q-input
        v-model="ticket.description"
        type="textarea"
        float-label="Textarea"
        :max-height="100"/>
      <!-- Navigation for this step at the end of QStep-->
      <q-stepper-navigation>
        <q-btn color="secondary" @click="$refs.stepper.next()">Continue</q-btn>
      </q-stepper-navigation>
    </q-step> 
    <q-step name="Second" title="Review and Finalize">
      <div>Tech: {{ticket.technician}}</div>
      <div>Room: {{ticket.room}}</div>
      <div>Machine: {{ticket.machine}}</div>
      <div>Issue: {{ticket.description}}</div>
      <q-stepper-navigation>
        <q-btn color="secondary" @click="CreateTicket">Submit</q-btn>
        <q-btn color="secondary" @click="$refs.stepper.goToStep('first')">Restart</q-btn>
        <q-btn color="secondary" flat @click="$refs.stepper.previous()">Back</q-btn>
      </q-stepper-navigation>
    </q-step>
  </q-stepper>

</div>

</div>
</template>

<script>
import * as firebase from 'firebase'
import 'quasar-extras/animate/bounceInDown.css'
import 'quasar-extras/animate/bounceOutUp.css'
import {
  Alert,
  Platform,
  Toast,
  QField,
  QDialogSelect,
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
  QStep,
  QStepper,
  QStepperNavigation,
  QSelect
} from 'quasar'

export default {
  name: 'createticket',
  components: {
    Platform,
    Alert,
    Toast,
    QField,
    QLayout,
    QDialogSelect,
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
    QStep,
    QStepper,
    QStepperNavigation,
    QSelect
  },
  data () {
    return {
      role: null,
      platform: this.$q.platform.is,
      ticket: {
        technician: '',
        site: '',
        room: '',
        machine: '',
        description: ''
      },
      issueType: '',
      issueOptions: [{
        label: 'Email',
        value: 'Email'
      }, {
        label: 'Blackboard',
        value: 'Blackboard'
      }, {
        label: 'Hardware',
        value: 'Hardware'
      }, {
        label: 'Software',
        value: 'Software'
      }, {
        label: 'Networking',
        value: 'Neworking'
      }],
      techs: [],
      sites: [],
      machines: [],
      rooms: [],
      vertical: false
    }
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
      })
    },
    GetRooms (site) {
      fetch('api/systeminfo/rooms/' + site, {
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
    GetMachines (site, room) {
      fetch('api/systeminfo/machines/' + site + '/' + room, {
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
    GetTechs (site) {
      fetch('api/systeminfo/technicians/' + site, {
        headers: {
          'Accept': 'application/json',
          'cache-control': 'no-cache'
        },
        credentials: 'same-origin',
        method: 'GET'
      }).then(response => {
        return response.json()
      }).then(json => {
        this.techs = json
      })
    },
    CreateTicket () {
      var user = firebase.auth().currentUser
      fetch('/api/systeminfo/ticket/' + user.uid, {
        method: 'POST',
        credentials: 'same-origin',
        headers: {
          'Content-Type': 'application/json'
        },
        body: JSON.stringify(this.ticket)
      }).then(response => {
        // this.createdTicketAlert()
        this.ticket = {}
        Toast.create.positive({
          html: 'Ticket Created Successfully!',
          timeout: 2000,
          onDismiss: this.$refs.stepper.goToStep('first')
        })
      })
    },
    createdTicketAlert () {
      Alert.create({
        enter: 'bounceInDown',
        leave: 'bounceOutUp',
        html: 'Your ticket has been created!',
        icon: 'thumb_up',
        color: 'positive',
        position: 'top'
      })
    }
  },
  computed: {
    siteOptions () {
      var sites = []
      this.sites.forEach(element => {
        sites.push({label: element.sitename, value: element.siteid})
      })
      return sites
    },
    roomOptions () {
      var rooms = []
      this.rooms.forEach(element => {
        rooms.push({label: element.roomnum, value: element.roomnum})
      })
      return rooms
    },
    machineOptions () {
      var machines = []
      this.machines.forEach(element => {
        machines.push({label: element.computerName, value: element.computerid})
      })
      return machines
    },
    techOptions () {
      var techs = []
      this.techs.forEach(element => {
        techs.push({label: element.firstname + ' ' + element.lastname, value: element.userid, text: element.firstname + ' ' + element.lastname})
      })
      return techs
    }
  },
  mounted () {
    this.GetSites()
    var user = firebase.auth().currentUser
    firebase.database().ref('/users/' + user.uid).once('value').then(snapshot => {
      this.role = (snapshot.val() && snapshot.val().role)
    })
    var isMobile = {
      Android: function () {
        return navigator.userAgent.match(/Android/i)
      },
      iOS: function () {
        return navigator.userAgent.match(/iPhone|iPad|iPod/i)
      },
      Windows: function () {
        return navigator.userAgent.match(/IEMobile/i)
      },
      any: function () {
        return (isMobile.Android() || isMobile.iOS() || isMobile.Windows())
      }
    }
    if (isMobile.iOS() || isMobile.Android()) {
      this.vertical = true
    }
  }
}
</script>

<style scoped>
.top {
  padding-top: 1%
}
.stepper {
  width: 90%
}
</style>

