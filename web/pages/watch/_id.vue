<template>
    <div v-if="quest">
        <Header title='Отслеживание' />
        <v-row class="my-3 d-flex justify-space-between text-center align-center">
            <v-col no-gutters class="col col-2">
                <v-icon @click="$router.go(-1)" large>
                    mdi-arrow-left
                </v-icon>
            </v-col>
            <v-col class="col text-h5 col-4">
                <v-chip elevation="10" dark class="deep-purple rounded-pill">
                    {{ quest.title }}
                </v-chip>
            </v-col>
            <v-col class="col col-2"></v-col>
        </v-row>
        <v-main v-if="quest.stages" class="pa-2 mt-4">
            <v-row>
                <v-col cols="0" md="2"></v-col>
                <v-col cols="12" md="8" class="pa-4 pa-md-0">
                    <WatchItem :stages="quest.stages" class="mb-2" v-for="item in progresses" :key="item.user.id"
                        :item="item" :all="quest.stages.length" />
                </v-col>
                <v-col cols="0" md="2"></v-col>
            </v-row>

        </v-main>

    </div>
</template>


<script>
import { mapState } from 'vuex'
import Header from '~/components/UI/Header'
import User from '~/components/watch/User.vue'
import WatchItem from '~/components/watch/WatchItem.vue';
export default {
    created() {
        this.id = this.$route.params.id
        this.ws = new WebSocket(`ws://localhost:9007/check/${this.id}`);
        this.$request({
            url: `/GenerateQuest/GetQuest/${this.id}`,
            methods: "GET",
        }).then(res => {
            this.quest = res
        })
        this.ws.onopen = (e) => {
            this.ws.send(JSON.stringify("start"));
        };
        this.ws.onmessage = (event) => {
            this.progresses = JSON.parse(event.data)
        };
    },
    beforeDestroy() {
        this.ws.close()
    },
    data() {
        return {
            id: 0,
            quest: {},
            progresses: [],
            ws: false
        }
    },
    components: {
        Header,
        User,
        WatchItem
    },
    computed: {
        ...mapState('auth', ['isLoggedIn', 'user']),

    }
}
</script>
