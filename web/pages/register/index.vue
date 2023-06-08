<template>
  <div>
    <Header title='Авторизация' />
    <v-main class="pa-2 d-flex h-80 align-center mt-4">
      <div class="d-flex justify-center align-center">
        <RegForm class="w-100" :error="error" @submit="submit" />
      </div>
    </v-main>
  </div>
</template>


<script>
import Header from '~/components/UI/Header'
import Quest from '~/components/Quest'
import RegForm from '~/components/auth/regForm'
import { mapState } from 'vuex'
export default {
  components: {
    Header,
    Quest,
    RegForm
  },
  layout: 'nolog',
  data() {
    return {
      error: false
    }
  },
  methods: {
    submit(user) {
      this.$store.dispatch('auth/register', user)
        .then((res) => {
          this.error = false
          this.$router.push('/login')
        })
        .catch(err => this.error = err)
    }
  },
  computed: {
    ...mapState('quests', ['quests'])
  }
}
</script>

<style>
.h-80 {
  height: 80vh !important;
}
</style>
