<template>
    <div>
        <v-text-field v-model="answer" solo :readonly="flag" label="Впишите ответ"></v-text-field>
    </div>
</template>

<script>
export default {
    data() {
        return {
            answer: '',
        }
    },
    props: {
        question: {
            type: Object,
            required: true
        }
    },
    watch: {
        answer(val) {
            if (val) {
                this.$emit('questionAnswer', [val])
            }
        }
    }
}
</script>

