<template>
    <v-card>
        <v-card-title class="deep-purple--text">
            {{ update.title }}
        </v-card-title>
        <v-card-text class="d-flex align-center">
            <v-btn class="ma-2" rounded fab x-large outlined color="indigo">
                <v-icon large>{{ update.icon }}</v-icon>
            </v-btn>
            <div class="ml-4">
                {{ update.description }}
            </div>
        </v-card-text>
    </v-card>
</template>

<script>
export default {
    data() {
        return {

        }
    },
    props: {
        update: {
            type: Object,
            required: true
        }
    }
}
</script>