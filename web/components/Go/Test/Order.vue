<template>
    <div>
        <Container @drop="onDrop" orientation='vertical'>
            <Draggable v-for="(item, idx) in answer" :key="idx">
                <v-text-field readonly solo :value="item"></v-text-field>
            </Draggable>
        </Container>
    </div>
</template>

<script>
import { Container, Draggable } from "vue-dndrop";
import { applyDrag } from "@/helpers/applyDrag.js";
export default {
    components: {
        Container,
        Draggable,
    },
    beforeMount() {
        function shuffleArray(array) {
            for (var i = array.length - 1; i > 0; i--) {
                var j = Math.floor(Math.random() * (i + 1));
                var temp = array[i];
                array[i] = array[j];
                array[j] = temp;
            }
        }
        this.answer = [...this.question.answers]
        shuffleArray(this.answer)
        console.log(this.answer)
    },
    data() {
        return {
            answer: []
        }
    },
    props: {
        question: {
            type: Object,
            required: true
        }
    },
    methods: {
        onDrop(dropResult) {
            this.answer = applyDrag(this.answer, dropResult);
            console.log(this.answer)
        },
    },
    watch: {
        answer(val) {
            if (val) {
                this.flag = true
                this.$emit('questionAnswer', val)
            }
        }
    }
}
</script>

