<template>
    <v-stepper v-model="count" vertical elevation="0">
        <Fragment :key="idx" v-for="(question, idx) in stage.questions">
            <v-stepper-step :step="idx + 1">
                {{ question.title }}
            </v-stepper-step>
            <v-stepper-content :step="idx + 1">
                <Multiple @questionAnswer="e => onAnswer(question, e)" v-if="question.type == 'multiple'"
                    :question="question" />
                <Once @questionAnswer="e => onAnswer(question, e)" v-if="question.type == 'single'" :question="question" />
                <Insert @questionAnswer="e => onAnswer(question, e)" v-if="question.type == 'insert'"
                    :question="question" />
                <Order v-if="count == idx + 1 && question.type == 'order'" @questionAnswer="e => onAnswer(question, e)"
                    :question="question" />
                <div class="d-flex">
                    <v-spacer />
                    <v-btn color="primary" :disabled="!permission" @click="nextQuestion(idx)">Следующий</v-btn>
                </div>
            </v-stepper-content>
        </Fragment>
    </v-stepper>
</template>

<script>
import { Fragment } from 'vue-fragment'
import Multiple from '~/components/Go/Test/Multiple'
import Once from '~/components/Go/Test/Once'
import Insert from '~/components/Go/Test/Insert'
import Order from '~/components/Go/Test/Order'
export default {
    components: {
        Fragment,
        Multiple,
        Once,
        Insert,
        Order
    },
    data() {
        return {
            count: 1,
            permission: false
        }
    },
    props: {
        stage: {
            type: Object,
            required: true
        }
    },
    methods: {
        nextQuestion(idx) {
            this.permission = false
            this.count = idx + 2
        },
        onAnswer(stage, answer) {
            stage.rightAnswer = answer
            this.permission = true
        },
    },
    watch: {
        count(val) {
            if (this.count == this.stage.questions.length + 1) {
                this.$emit('stageComplete')
            }
        }
    }

}
</script>
