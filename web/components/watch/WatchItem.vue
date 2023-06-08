<template>
    <v-card>
        <v-card-title>{{ item.user.userName }}</v-card-title>
        <v-card-subtitle>{{ item.user.name }} {{ item.user.surname }}</v-card-subtitle>
        <v-card-text class="d-flex align-center">
            <v-progress-circular :rotate="360" :size="100" :width="15" :value="item.progress.stage / stages.length * 100"
                color="primary">
                {{ item.progress.stage }}/{{ stages.length }}
            </v-progress-circular>
            <div class="text--secondary ml-4 deep-purple--text">
                {{ stages[item.progress.stage].title }}
            </div>
        </v-card-text>
        <v-card-actions>
            <v-spacer></v-spacer>
            <v-chip>
                {{ stages[item.progress.stage].type }}
            </v-chip>
        </v-card-actions>
    </v-card>
</template>

<script>
export default {
    props: {
        item: {
            type: Object,
            required: true
        },
        total: {
            type: Number,
            required: true
        },
        stages: {
            type: Array,
            required: true
        }
    }
}
</script>