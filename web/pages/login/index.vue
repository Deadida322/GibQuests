<template>
  <div>
    <Header title='Авторизация' />
    <v-main class="pa-2 d-flex h-80 justify-center align-center mt-4">
      <div class="d-flex justify-center align-center">
        <LoginForm class="w-100" :error="error" @submit="submit" />
      </div>
    </v-main>
  </div>
</template>


<script>
import Header from '~/components/UI/Header'
import Quest from '~/components/Quest'
import LoginForm from '~/components/auth/loginForm'
export default {
  components: {
    Header,
    Quest,
    LoginForm
  },
  data() {
    return {
      error: false
    }
  },
  layout: 'nolog',
  methods: {
    submit(user) {
      console.log(user)
      this.$store.dispatch('auth/login', user)
        .then(user => {
          this.error = false
          this.$router.push('/')
        })
        .catch(err => {
          this.error = err
        })
    }
  },
}
</script>

<style>
.wv-100 {
  width: 100vw;
}

.h-80 {
  height: 80vh !important;
}
</style>
