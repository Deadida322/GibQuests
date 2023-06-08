<template>
    <v-menu bottom offset-y transition="slide-y-transition">
        <template v-slot:activator="{ on, attrs }">
            <v-btn icon v-bind="attrs" v-on="on">
                <v-avatar v-if="!isLoggedIn && !user.userName" color="blue">ЛК</v-avatar>
                <v-avatar v-else color="blue">{{ user.userName.slice(0, 2) }}</v-avatar>
            </v-btn>
        </template>
        <v-list v-if="!isLoggedIn">
            <v-list-item-group>
                <v-list-item to="/login" color="primary">
                    <v-list-item-title>Вход</v-list-item-title>
                </v-list-item>
                <v-list-item to="/register" color="error" class="primary--text">
                    <v-list-item-title prepend-icon="mdi-delete">Регистрация</v-list-item-title>
                </v-list-item>
            </v-list-item-group>
        </v-list>
        <v-list v-else>
            <v-list-item-group>
                <v-list-item to="/lk" color="primary">
                    <v-list-item-title>Личный кабинет</v-list-item-title>
                </v-list-item>
                <v-list-item to="/yours" color="primary">
                    <v-list-item-title>Мои квесты</v-list-item-title>
                </v-list-item>
                <v-list-item @click="logout" to="/" color="error" class="primary--text">
                    <v-list-item-title prepend-icon="mdi-delete">Выйти</v-list-item-title>
                </v-list-item>
            </v-list-item-group>
        </v-list>
    </v-menu>
</template>


<script>
import { mapState } from 'vuex'
export default {
    computed: {
        ...mapState('auth', ['isLoggedIn', 'user'])
    },
    methods: {
        logout() {

            this.$store.commit('auth/logout')
        }
    }
}
</script>

