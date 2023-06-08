<template>
    <div>
        <v-app-bar color="deep-purple accent-4" :fixed="fixed" dark height="64px">
            <v-app-bar-nav-icon @click.stop="drawer = !drawer" />
            <v-toolbar-title>{{ title }}</v-toolbar-title>
            <v-spacer />
            <LkMenu />
        </v-app-bar>
        <v-navigation-drawer v-model="drawer" absolute left temporary width="340px">
            <Menu />
        </v-navigation-drawer>
    </div>
</template>

<script>
import Menu from '~/components/UI/Menu'
import LkMenu from '~/components/UI/LkMenu'
export default {
    data() {
        return {
            drawer: false
        }
    },
    components: {
        Menu,
        LkMenu
    },
    props: {
        title: {
            type: String,
            default: 'Квесты'
        },
        fixed: {
            default: false,
            type: Boolean
        }
    }
}
</script>

