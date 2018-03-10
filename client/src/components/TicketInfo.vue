<template>
<div>
  <q-card inline style="width: 400px;" :key="ticket.ticketnum" v-for="ticket in tickets">
    <q-card-title style="background: ">
      Ticket # {{ticket.ticketnum}}
      <span slot="subtitle">Ticket Information</span>
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
        <q-input v-model="ticket.user" float-label="Requester" />
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
      <q-btn flat>Action 1</q-btn>
      <q-btn flat>Action 2</q-btn>
    </q-card-actions>
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
  name: 'ticketinfo',
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
      tickets: [],
      lorem: 'Some Message Goes Here For Testing'
    }
  },
  mounted () {
    this.GetTicketInfo()
  },
  methods: {
    GetTicketInfo () {
      fetch('api/systeminfo/tickets', {
        headers: {
          'Accept': 'application/json',
          'cache-control': 'no-cache'
        },
        credentials: 'same-origin',
        method: 'GET'
      }).then(response => {
        return response.json()
      }).then(json => {
        this.tickets = json
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
</style>
