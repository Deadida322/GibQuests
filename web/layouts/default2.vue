<template>
  <div>
    <v-app>
      adsanmdsasnmdan
      <nuxt v-if="show"/>
    </v-app>
  </div>
</template>

<script>
import { getItem } from '~/helpers/persistanceStorage'
import Header from '~/components/UI/Header.vue'
export default {
  name: "creating",

  components: {
    Header
  },
  created(){
    const user = getItem('user')
    if(user){
      this.$store.dispatch('auth/logIn', user)
        .then(()=>this.show=true)
        .catch(()=>{
          this.show=true
        })
    }else{
      this.show=true
    }
  },
  data () {
    return {
      show: false
    }
  }
}
</script>
