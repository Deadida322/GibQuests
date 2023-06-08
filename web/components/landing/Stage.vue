<template>
    <v-card hover class="container">
        <v-card-title class="deep-purple--text">
            {{ stage.title }}
        </v-card-title>
        <v-card-text class="d-flex align-center">
            <v-chip class="chip">{{ stage.status }}</v-chip>
            <v-btn class="ma-2" rounded fab x-large outlined color="indigo">
                <v-icon large>{{ stage.icon }}</v-icon>
            </v-btn>
            <div class="ml-4">
                {{ stage.description }}
            </div>
        </v-card-text>

    </v-card>
</template>

<script>
export default {
    data() {
        return {}
    },
    props: {
        stage: {
            type: Object,
            required: true
        }
    }
}
</script>

<style scoped>
.container {
    position: relative;
}

.chip {
    position: absolute;
    top: 0;
    right: 0;
    transform: translateY(-50%)
}
</style>