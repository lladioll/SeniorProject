<template>
<div>

<q-tabs v-model="selectedTab" align="center">
  <q-tab name="xtab-1" label="Open Tickets" icon="message" slot="title" />
  <q-tab name="xtab-2" label="Closed Tickets" icon="account_box" slot="title" />

  <q-tab-pane name="xtab-1">
    <div class="tickets">
  <q-card inline style="width: 600px" :key="ticket.ticketnum" v-for="ticket in openTickets">
    <q-card-title style="background-color: #007471; text-align: center; color: white;">
      Ticket # {{ticket.ticketnum}}
      <span slot="subtitle" style="color:white;">Ticket Information</span>
      <q-icon slot="right" name="more_vert">
        <q-popover ref="popover">
          <q-list link class="no-border">
            <q-item @click="$refs.popover.close()">
              <q-item-main label="Remove Card" />
            </q-item>
            <q-item @click="$refs.popover.close()">
              <q-item-main label="Send Feedback" />
            </q-item>
            <q-item @click="$refs.popover.close()">
              <q-item-main label="Share" />
            </q-item>
          </q-list>
        </q-popover>
      </q-icon>
    </q-card-title>
    <q-card-separator />
    <q-card-main>
      <q-field>
        <q-input v-model="ticket.technician" float-label="Technician" />
      </q-field>
      <q-field>
        <q-input v-model="ticket.requester" float-label="Requester" />
      </q-field>
      <q-field>
        <q-input v-model="ticket.requestdate" float-label="Date of Request" />
      </q-field>
      <q-field>
        <q-input v-model="ticket.machine" float-label="Machine" />
      </q-field>
      <q-field>
        <q-input v-model="ticket.description" float-label="Issue Description" />
      </q-field>
    </q-card-main>
    <q-card-separator />
    <q-card-actions>
      <q-btn flat>Edit Ticket</q-btn>
      <q-btn flat>Action 2</q-btn>
    </q-card-actions>
  </q-card>
</div>
</q-tab-pane>
  <q-tab-pane name="xtab-2"><div class="tickets" >
  <q-card inline style="width: 600px" :key="ticket.ticketnum" v-for="ticket in closedTickets">
    <q-card-title style="background-color: maroon; text-align: center; color: white;">
      Ticket # {{ticket.ticketnum}}
      <span slot="subtitle" style="color:white">Ticket Information</span>
      <q-icon slot="right" name="more_vert">
        <q-popover ref="popover">
          <q-list link class="no-border">
            <q-item @click="$refs.popover.close()">
              <q-item-main label="Remove Card" />
            </q-item>
            <q-item @click="$refs.popover.close()">
              <q-item-main label="Send Feedback" />
            </q-item>
            <q-item @click="$refs.popover.close()">
              <q-item-main label="Share" />
            </q-item>
          </q-list>
        </q-popover>
      </q-icon>
    </q-card-title>
    <q-card-separator />
    <q-card-main>
      <q-field>
        <q-input v-model="ticket.technician" float-label="Technician" />
      </q-field>
      <q-field>
        <q-input v-model="ticket.requester" float-label="Requester" />
      </q-field>
      <q-field>
        <q-input v-model="ticket.requestdate" float-label="Date of Request" />
      </q-field>
      <q-field>
        <q-input v-model="ticket.machine" float-label="Machine" />
      </q-field>
      <q-field>
        <q-input v-model="ticket.description" float-label="Issue Description" />
      </q-field>
       <q-field>
        <q-input v-model="ticket.completedate" float-label="Date Completed" />
      </q-field>
       <q-field>
        <q-input v-model="ticket.notes" float-label="Technician Notes" />
      </q-field>
    </q-card-main>
    <q-card-separator />
  </q-card>
</div></q-tab-pane>
</q-tabs>


</div>
</template>

<script>
import * as firebase from 'firebase'
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
  QCardActions,
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
  name: 'viewtickets',
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
    QCardActions,
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
      openTickets: [],
      closedTickets: [],
      selectedTab: 'xtab-1'
    }
  },
  mounted () {
    this.GetOpenTicketInfo()
    this.GetClosedTicketInfo()
  },
  methods: {
    GetOpenTicketInfo () {
      var user = firebase.auth().currentUser
      fetch('api/systeminfo/opentickets/' + user.uid, {
        headers: {
          'Accept': 'application/json',
          'cache-control': 'no-cache'
        },
        credentials: 'same-origin',
        method: 'GET'
      }).then(response => {
        return response.json()
      }).then(json => {
        this.openTickets = json
        console.log(json)
      })
    },
    GetClosedTicketInfo () {
      var user = firebase.auth().currentUser
      fetch('api/systeminfo/closedtickets/' + user.uid, {
        headers: {
          'Accept': 'application/json',
          'cache-control': 'no-cache'
        },
        credentials: 'same-origin',
        method: 'GET'
      }).then(response => {
        return response.json()
      }).then(json => {
        this.closedTickets = json
        console.log(json)
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
.active {
  color: "red";
}
.subtitle {
  color: white;
}
.tickets {
  display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: center;
}
</style>
