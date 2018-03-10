<template>
<div>
  {{message}}
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
  QCardSeparator
} from 'quasar'

export default {
  name: 'addtechnician',
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
    QCardSeparator
  },
  data () {
    return {
      message: 'test'
    }
  },
  mounted () {
  },
  methods: {
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