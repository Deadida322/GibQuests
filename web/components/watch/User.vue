<template>
    <v-card>
        <v-card-text class="d-flex align-center">
            <v-progress-circular :rotate="360" :size="100" :width="15" :value="(now || user.now) / (all || user.all) * 100"
                color="primary">
                {{ now || user.now }}/{{ user.all }}
            </v-progress-circular>
            <div class="text-h5 ml-4">
                {{ user.username }}
                <v-card-subtitle>
                    {{ user.name + ' ' + user.surname }}
                </v-card-subtitle>
            </div>
        </v-card-text>
    </v-card>
</template>

<script>
export default {
    props: {
        user: {},
        now: 0,
        all: 0
    }
}
</script>

