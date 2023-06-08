<template>
    <div>
        <Header title='Ваши квесты' />
        <v-main class="pa-2 mt-4">
            <Search :placeholder="'Найти среди своих'" @search="search" />
            <v-row>
                <v-col cols="0" md="2"></v-col>
                <v-col cols="12" md="8" class="pa-4 pa-md-0">
                    <div v-if="quests.length" class="d-flex justify-space-between align-center flex-wrap">
                        <Quest @remove="remove(item)" class="mb-2 w-50-100" edit v-for="(item, key) in quests" :key="key"
                            :item="item" />
                        <div class="mt-5 w-50-100">
                            <Add @click="createNew">
                                <v-list>
                                    <v-list-item-group>
                                        <v-list-item @click="createNew" color="primary">
                                            <v-list-item-title>Новый квест</v-list-item-title>
                                        </v-list-item>
                                    </v-list-item-group>
                                </v-list>
                            </Add>
                        </div>

                    </div>

                </v-col>
                <v-col cols="0" md="2"></v-col>
            </v-row>

        </v-main>
    </div>
</template>

<script>
import Header from '~/components/UI/Header.vue'
import Search from '~/components/UI/Search.vue'
import Quest from '~/components/Quest.vue'
import Add from '~/components/UI/Add'
import { mapState } from 'vuex'
export default {
    components: {
        Header,
        Search,
        Quest,
        Add
    },
    data() {
        return {
            quests: {}
        }
    },
    created() {
        if (!this.isLoggedIn) this.$router.push('/login')
        this.$store.commit('create/removeCurrentQuest')
        this.fetchQuests()
    },
    methods: {
        search(e) {
        },
        fetchQuests() {
            this.$request({
                url: "/GenerateQuest/GetFilteredQuests",
                method: "POST",
                data: {
                    id: this.user.id
                },
            }).then(res => {
                this.quests = res
            })
        },
        createNew() {
            this.$store.commit('create/removeCurrentQuest')
            this.$router.push(`/create/new`)
        },
        remove({ id }) {
            this.$request({
                url: `/GenerateQuest/DeleteQuest/${id}`,
                method: "DELETE",
                headers: { 'Authorization': 'Bearer ' + this.token }
            }).then(res => {
                this.$showSuccess()
                this.fetchQuests()
            })

        }
    },
    computed: {
        ...mapState('auth', ['user', 'isLoggedIn', "token"])
    }
}
</script>

