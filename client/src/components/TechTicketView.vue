<template>
<div>
  <q-data-table
  :data="tickets"
  :config="config"
  :columns="columns"
  @refresh="refresh"
    >
    <!-- Custom renderer for "message" column -->
    <div slot="col-message" slot-scope="cell">
        <span class="light-paragraph">{{cell.data}}</span>
    </div>

    <!-- Custom renderer for "source" column -->
    <div slot="col-status" slot-scope="cell">
        <span v-if="cell.data === 'Open'" class="my-label text-white bg-primary">{{cell.data}}</span>
        <span v-else-if="cell.data === 'Closed'" class="my-label text-white bg-negative">{{cell.data}}</span>
        <span v-else class="my-label text-black bg-warning">{{cell.data}}</span>
    </div>

    <!-- Custom renderer for "action" column with button for custom action -->
    <div slot='col-action' slot-scope='cell'>
        <q-btn color="primary" @click='doSomethingMethod(cell.row.id)'>View</q-btn>
    </div>

    <!-- Custom renderer when user selected one or more rows -->
    <div slot="selection" slot-scope="selection">
      <q-btn color="primary" @click="editTicket(selection)">
        <i>Edit</i>
        </q-btn>
        <q-btn color="primary" @click="editTicket(selection)">
        <i>Close</i>
        </q-btn>
    </div>
  </q-data-table>

  <q-modal ref="layoutModal" v-model="ticketModal" :content-css="{minWidth: '40vw', minHeight: '85vh'}">
    <q-modal-layout>
      <q-toolbar slot="header">
        <q-btn flat @click="$refs.layoutModal.close()">
          <q-icon name="keyboard_arrow_left" />
        </q-btn>
        <div class="q-toolbar-title">
          Edit Ticket # {{ticketinfo.ticket}}
        </div>
      </q-toolbar>
      <q-toolbar slot="header">
      </q-toolbar>
      <div class="layout-padding">
        <h1>Edit Ticket {{ticketinfo.ticket}}</h1>
        <p class="caption">
           <q-select
            v-model="ticketinfo.status"
            float-label="Ticket Status"
            radio
          :options="selectOptions"
          />
          <q-input
            v-model="ticketinfo.notes"
            type="textarea"
            float-label="Notes"
            :max-height="100"
            :min-rows="7"
          />
        </p>
        <div class="actions" style="align-text: right;">
          <q-btn color="primary" @click="UpdateTicketInfo()">Update</q-btn>
          <q-btn color="primary" @click="$refs.layoutModal.close()">Close</q-btn>
        </div>
      </div>
      <q-toolbar slot="footer">
        <div class="q-toolbar-title">
          Footer
        </div>
      </q-toolbar>
    </q-modal-layout>
  </q-modal>
</div>
</template>

<script>
import * as firebase from 'firebase'
import 'quasar-extras/animate/fadeIn.css'
import 'quasar-extras/animate/fadeOut.css'
import {
  Toast,
  QLayout,
  QSelect,
  QModalLayout,
  QToolbar,
  QToolbarTitle,
  QBtn,
  QIcon,
  QList,
  QListHeader,
  QItem,
  QModal,
  QItemSide,
  QItemMain,
  QCollapsible,
  QCard,
  QCardTitle,
  QPopover,
  QTooltip,
  QCarousel,
  QTabs,
  QTab,
  QTabPane,
  QTransition,
  QInput,
  QField,
  QCardMain,
  QDataTable,
  QCardSeparator
} from 'quasar'

export default {
  inject: [ 'layout' ],
  name: 'techticketview',
  components: {
    QLayout,
    QSelect,
    QModalLayout,
    QDataTable,
    QModal,
    QTooltip,
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
      tickets: [],
      ticketModal: false,
      ticketinfo: {
        ticketnum: '',
        notes: '',
        status: ''
      },
      selectOptions: [
        {
          label: 'Open',
          value: 'Open'
        },
        {
          label: 'In Progress',
          value: 'In Progress'
        },
        {
          label: 'Closed',
          value: 'Closed'
        }
      ],
      config: {
        title: 'Data Table',
        rowHeight: '50px',
        refresh: true,
        noHeader: false,
        responsive: true,
        selection: 'single',
        pagination: {
          rowsPerPage: 15,
          options: [5, 10, 15, 30, 50, 500]
        },
        columnPicker: true,
        bodyStyle: {
          maxHeight: '500px'
        }
      },
      columns: [{
        label: 'Ticket#',
        field: 'ticketnum',
        width: '70px'
      }, {
        label: 'Technician',
        field: 'technician'
      }, {
        label: 'Requester',
        field: 'requester'
      }, {
        label: 'Machine',
        field: 'machine'
      }, {
        label: 'Room#',
        field: 'room',
        width: '70px'
      }, {
        label: 'Message',
        field: 'description'
      }, {
        label: 'Creation Date',
        field: 'requestdate',
        format (value, row) {
          return new Date(value).toLocaleString()
        }
      }, {
        label: 'Completion Date',
        field: 'completedate',
        format (value, row) {
          if (value !== null) {
            return new Date(value).toLocaleString()
          }
        }
      }, {
        label: 'Notes',
        field: 'notes'
      }, {
        label: 'Status',
        field: 'status',
        sort: true,
        type: 'string'
      }]
    }
  },
  mounted () {
    this.GetAssignedTickets()
    this.toggleDrawers()
  },
  methods: {
    refresh (done) {
      this.timeout = setTimeout(() => {
        this.GetAssignedTickets()
        done()
      }, 1000)
    },
    toggleDrawers () {
      this.layout.hideLeft()
      this.layout.hideRight()
    },
    beforeDestroy () {
      clearTimeout(this.timeout)
    },
    editTicket (props) {
      this.ticketModal = true
      this.ticketinfo.ticketnum = props.rows[0].data.ticketnum
    },
    deleteRow (props) {
      props.rows.forEach(row => {
        this.table.splice(row.index, 1)
      })
    },
    UpdateTicketInfo () {
      fetch('api/systeminfo/updateticket/', {
        method: 'PUT',
        credentials: 'same-origin',
        headers: {
          'Content-Type': 'application/json'
        },
        body: JSON.stringify(this.ticketinfo)
      }).then(response => {
        this.ticketinfo = {}
        this.ticketModal = false
        this.refresh()
        Toast.create.positive({
          html: 'Ticket Closed Successfully!',
          timeout: 2000
        })
      })
    },
    GetAssignedTickets () {
      var user = firebase.auth().currentUser
      fetch('api/systeminfo/ticketsbytech/' + user.uid, {
        headers: {
          'Accept': 'application/json'
        },
        credentials: 'same-origin',
        method: 'GET'
      }).then(response => {
        return response.json()
      }).then(json => {
        this.tickets = json
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
.my-label {
  padding: 7px;
  border-radius 3px;
  text-align: center;
  display: inline-block;
}
.card {
  box-shadow: 0 4px 8px 0 rgba(0,0,0,0.2);
  transition: 0.3s;
}
.card:hover {
  box-shadow: 0 10px 25px 0 rgba(0,0,0,0.2);
}
</style>