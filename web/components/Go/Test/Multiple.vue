<template>
    <div>
        <v-checkbox class="px-2" v-ripple="false" v-for="(item, idx) in question.answers" :key="idx" :readonly="flag"
            v-model="answer" :label="item" :value="item"></v-checkbox>
    </div>
</template>

<script>
export default {
    data() {
        return {
            answer: [],
            flag: false
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
                this.$emit('questionAnswer', val)
                this.flag = true
            }
        }
    }
}
</script>

