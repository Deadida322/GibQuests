<template>
    <v-card-text class="text-body-1" v-html="stage.text"></v-card-text>
</template>

<script>
export default {
    mounted() {
        this.$emit('stageComplete')
    },
    props: {
        stage: {
            required: true,
            type: Object
        }
    }
}
</script>

