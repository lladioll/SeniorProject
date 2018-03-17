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
    <div slot="col-completedate" slot-scope="cell">
        <span v-if="cell.data === null" class="my-label text-white bg-primary">Open</span>
        <span v-else class="my-label text-white bg-negative">{{cell.data}}</span>
    </div>

    <!-- Custom renderer for "action" column with button for custom action -->
    <div slot='col-action' slot-scope='cell'>
        <q-btn color="primary" @click='doSomethingMethod(cell.row.id)'>View</q-btn>
    </div>

    <!-- Custom renderer when user selected one or more rows -->
    <div slot="selection" slot-scope="selection">
        <q-btn color="primary" @click="changeMessage(selection)">
        <i>edit</i>
        </q-btn>
        <q-btn color="primary" @click="deleteRow(selection)">
        <i>delete</i>
        </q-btn>
    </div>
    </q-data-table>
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
  name: 'techticketview',
  components: {
    QLayout,
    QDataTable,
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
      config: {
        title: 'Data Table',
        rowHeight: '50px',
        refresh: true,
        noHeader: false,
        responsive: true,
        selection: 'multiple',
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
      }]
    }
  },
  mounted () {
    this.GetAssignedTickets()
  },
  methods: {
    refresh (done) {
      this.timeout = setTimeout(() => {
        done()
      }, 5000)
    },
    selection (number, rows) {
      console.log(`selected ${number}: ${rows}`)
    },
    rowClick (row) {
      console.log('clicked on a row', row)
    },
    beforeDestroy () {
      clearTimeout(this.timeout)
    },
    changeMessage (props) {
      props.rows.forEach(row => {
        row.data.message = 'Gogu'
      })
    },
    deleteRow (props) {
      props.rows.forEach(row => {
        this.table.splice(row.index, 1)
      })
    },
    GetAssignedTickets () {
      var user = firebase.auth().currentUser
      fetch('api/systeminfo/ticketsbytech/' + user.uid, {
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