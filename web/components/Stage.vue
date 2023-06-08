<template>
    <v-card>
        <v-card-text class="d-flex align-center">
            <v-btn v-ripple="false" class="text-h5" dark fab color="green">
                {{ idx + 1 }}
            </v-btn>
            <div class="ml-4 text-body-1">
                {{ stage.title }}
            </div>
        </v-card-text>
    </v-card>
</template>

<script>
export default {
    props: {
        idx: {
            type: Number,
            required: true
        },
        stage: {
            type: Object,
            required: true
        }
    }
}
</script>

