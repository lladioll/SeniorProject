<template>
<div>
  <q-pull-to-refresh :handler="refresher">
    <q-card>
      <div slot="right"> 
        <div class="title">
          <label class="custom-file-upload">
            Edit Profile
           </label>
        </div>
      </div>
      <q-card-media>
        <img class="userPhoto" :src="img">
      
      </q-card-media>
      <q-card-title>
        <div class="row justify-center"> 
          {{user.role}}
        </div>
        <div class="row justify-center"> 
        {{user.firstname}} {{user.lastname}}
        </div>
        <div class="row justify-center"> 
          <q-rating slot="subtitle" v-model="stars" :max="5" />
        </div>
      </q-card-title>
      <q-card-main>
        <div class="row justify-center"> 
          <p class="text-faded">This is a test paragraph describing the technician.</p>
        </div>
      </q-card-main>
      <q-card-separator />
      <q-card-actions align="center">
        <q-btn flat round small><q-icon name="event" /></q-btn>
        <q-btn v-if="user.role === 'Admin'" flat>Assign Ticket</q-btn>
        <q-btn v-if="user.role === 'Student' || user.role === 'Faculty'" flat>View Open Tickets</q-btn>
        <q-btn  @click="open = true" flat>Upload Profile Photo</q-btn>
      </q-card-actions>
    </q-card>
  </q-pull-to-refresh>

  <q-modal  :content-css="{padding: '5px'}" v-model="open">
    <q-uploader :url="uploadDest" />
    <q-btn style="margin-top: 10px; align-text: right" color="primary" @click="open = false">Close</q-btn>
  </q-modal>

</div>
</template>

<script>
import * as firebase from 'firebase'
import {
  Alert,
  QUploader,
  QField,
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
  QPullToRefresh,
  QProgress,
  QSpinner,
  QSpinnerMat
} from 'quasar'

export default {
  name: 'profile',
  components: {
    Alert,
    QUploader,
    QField,
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
    QPullToRefresh,
    QProgress,
    QSpinner,
    QSpinnerMat
  },
  data () {
    return {
      stars: 4,
      img: '',
      uploadDest: null,
      user: {
        firstname: '',
        lastname: '',
        email: '',
        role: '',
        uid: ''
      },
      open: false
    }
  },
  methods: {
    refresher (done) {
      // this.img = require('./../assets/ProfilePics/' + user.uid + '/profile.jpg')
      this.$router.go()
      setTimeout(() => {
        done()
      }, 1000)
    },
    getUser () {
      var user = firebase.auth().currentUser
      if (user) {
        fetch('api/systeminfo/user/' + user.uid, {
          headers: {
            'Accept': 'application/json',
            'cache-control': 'no-cache'
          },
          credentials: 'same-origin',
          method: 'GET'
        }).then(response => {
          return response.json()
        }).then(json => {
          this.user = json
          if (json.profilepic === '') {
            this.img = require('./../assets/ProfilePics/DefaultPhoto.png')
          }
          else {
            this.img = require('./../assets/ProfilePics/' + user.uid + '/profile.jpg')
          }
        })
      }
    },
    uploadPhoto (file) {
      var user = firebase.auth().currentUser
      var photo = new FormData()
      console.log(file[0])
      photo.append('photo', file[0])
      fetch('/api/systeminfo/upload/' + user.uid, {
        method: 'POST',
        credentials: 'same-origin',
        headers: {
          'Accept': 'application/json'
        },
        body: photo
      })
    }
  },
  created () {
    var user = firebase.auth().currentUser
    this.getUser()
    this.uploadDest = '/api/systeminfo/upload/' + user.uid
  }
}
</script>

<style scoped>
.custom-file-upload {
  border: 1px solid #ccc;
  display: inline-block;
  padding: 6px 12px;
  cursor: pointer;
}

input[type="file"] {
  display: none;
}
.userPhoto {
  display: block;
  width: 250px;
  height: 250px;
  border-radius: 50%;
  object-fit: cover;
  margin-left: auto;
  margin-right: auto;
}
.title {
  padding: 2%;
  text-align: right;
}
</style>

