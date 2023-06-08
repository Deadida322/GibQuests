<template>
  <v-app dark>
    <nuxt v-if="isLoggedIn" />
  </v-app>
</template>

<script>
import { mapState } from "vuex"

export default {
  created() {
    this.$store.dispatch('auth/tryToPair')
  },
  computed: {
    ...mapState("auth", ['isLoggedIn'])
  }
}
</script>
