<template>
<div>
  <div class="full-card-wrapper">
   <q-card inline style="width: 100%">
      <q-card-media>
        <img src="~assets/Banner2.jpg">
      </q-card-media>
      <q-list>
        <q-item>
          <q-item-side>
            <q-item-tile color="primary" icon="local bar" />
          </q-item-side>
          <q-item-main>
            <q-item-tile label>Do this</q-item-tile>
            <q-item-tile sublabel>Do That</q-item-tile>
          </q-item-main>
        </q-item>
        <q-item>
          <q-item-side>
            <q-item-tile color="red" icon="local gas station" />
          </q-item-side>
          <q-item-main>
            <q-item-tile label>Test</q-item-tile>
            <q-item-tile sublabel>Test</q-item-tile>
          </q-item-main>
        </q-item>
        <q-item>
          <q-item-side>
            <q-item-tile color="amber" icon="local movies" />
          </q-item-side>
          <q-item-main>
            <q-item-tile label>Test</q-item-tile>
            <q-item-tile sublabel>Test</q-item-tile>
          </q-item-main>
        </q-item>
      </q-list>
    </q-card>
  </div>

    <q-card inline color="secondary" style="width: 100%">
      <q-card-title>
        Unassigned Ticket
        <span slot="subtitle">Subtitle</span>
        <q-icon slot="right" name="alarm" />
      </q-card-title>
      <q-card-main>
       
      </q-card-main>
      <q-card-separator />
      <q-card-actions>
        <q-btn flat>Assign</q-btn>
        <q-btn flat>View Ticket</q-btn>
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
  QCardActions,
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
  QItemTile,
  QCardSeparator,
  QCardMedia
} from 'quasar'

export default {
  name: 'admindashboard',
  components: {
    QLayout,
    QCardMedia,
    QCardActions,
    QItemTile,
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
    }
  },
  mounted () {
  },
  methods: {
  }
}
</script>

<style lang="stylus" scoped>
.card-wrapper {
  padding: 2%;
}

.full-card-wrapper {
  display: flex;
  justify-content: center;
  align-items: center;

}
.card {
  box-shadow: 0 4px 8px 0 rgba(0,0,0,0.2);
  transition: 0.3s;
}
.card:hover {
  box-shadow: 0 10px 25px 0 rgba(0,0,0,0.2);
}
</style>