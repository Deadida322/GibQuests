<template>
    <v-card elevation="4">
        <v-img height="150" gradient="to top right, rgba(0,0,0,.2), rgba(25,32,72,.5)"
            class="d-flex align-center text-center justify-center" dark :src="item.img">
            <p v-html="`${item.description.slice(0, 130)}...`" class="text-body-1 pa-2" />
        </v-img>
        <v-card-title class="d-flex align-center justify-space-between text--secondary pb-0">
            {{ item.title }}
            <QuestMenu @remove="$emit('remove')" v-if="edit" :id="item.id" />
        </v-card-title>
        <div v-if="item.users && item.users.length">
            <v-card-subtitle class="py-0 pb-2 text--secondary">Сейчас проходят</v-card-subtitle>
            <v-card-text>
                <v-avatar color="green" class="mr-2 white--text" v-for="(item, key) in item.users"
                    :key="key">{{ item.username.slice(0, 2).toUpperCase() }}</v-avatar>
            </v-card-text>
        </div>
        <v-card-actions>
            <v-spacer />
            <v-btn :to="`/view/${item.id}`" color="primary">Просмотреть</v-btn>
        </v-card-actions>
    </v-card>
</template>

<script>
import QuestMenu from '~/components/create/QuestMenu'
export default {
    data() {
        return {

        }
    },
    components: {
        QuestMenu
    },
    props: {
        edit: {
            type: Boolean,
            default: false
        },
        item: {
            type: Object,
            required: true
        }
    }
}
</script>
