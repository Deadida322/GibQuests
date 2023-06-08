<template>
    <v-container class="pa-0" @click="handleTestChange">
        <v-card class="mt-3 mb-6" v-for="(item, index) in questions" :key="index" shaped>
            <v-card-title class="d-flex justify-space-between">
                Вопрос {{ index + 1 }}
                <v-icon @click="removeQuestion(index)" color="red">mdi-close</v-icon>
            </v-card-title>
            <v-card-text>
                <v-text-field v-model="item.contain" label="Cодержание вопроса" hint="Содержание вопроса" clearable solo
                    filled dense required></v-text-field>
                <v-select v-model="item.type" item-value="value" item-text="label" :items="types" label="Тип вопроса"
                    required></v-select>
                <v-card-subtitle>
                    Варианты ответа
                </v-card-subtitle>
                <v-divider dark class="mb-3"></v-divider>
                <div v-if="item.type == 'single' || item.type == 'multiple'" class="selections">
                    <v-row v-for="(answer, j) in item.answers" :key="j">
                        <v-text-field class="ml-3" clearable solo :hint="`Вариант ${j + 1}`" :label="`Вариант ${j + 1}`"
                            v-model="item.answers[j]">
                        </v-text-field>
                        <v-btn @click="removeAnswer(index, j)" class="ma-2" fab dark x-small color="red">
                            <v-icon dark>
                                mdi-minus
                            </v-icon>
                        </v-btn>
                    </v-row>
                    <v-row>
                        <v-spacer></v-spacer>
                        <v-btn @click="addAnswer(index)" class="mb-4" fab dark small color="success">
                            <v-icon dark>
                                mdi-plus
                            </v-icon>
                        </v-btn>
                        <v-spacer></v-spacer>
                    </v-row>
                    <v-select :items="item.answers" label="Верный ответ" color="success" outlined multiple
                        v-model="item.rightAnswer" v-if="item.type == 'multiple'"></v-select>
                    <v-select :items="item.answers" label="Верный ответ" color="success" outlined v-else
                        v-model="item.rightAnswer[0]"></v-select>
                </div>
                <div v-if="item.type == 'insert'" class="selections">
                    <v-text-field :items="item.answers" label="Верный ответ" color="success" outlined
                        v-model="item.rightAnswer[0]"></v-text-field>
                </div>
                <div @click="setCurrentDrag(index)" v-if="item.type == 'order'" class="selections">
                    <Container @drop="onDrop({ index, ...$event })" orientation='vertical'>
                        <Draggable v-for="(answer, j) in item.answers" :key="j">
                            <div class="draggable-item">
                                <v-row>
                                    <v-text-field class="ml-1" clearable solo :hint="`Вариант ${j + 1}`"
                                        :label="`Вариант ${j + 1}`" v-model="item.answers[j]">
                                    </v-text-field>
                                    <v-btn @click="removeAnswer(index, j)" class="my-2 ml-2  curp" fab dark x-small
                                        elevation="2" color="red">
                                        <v-icon x-small dark>
                                            mdi-minus
                                        </v-icon>
                                    </v-btn>
                                </v-row>
                            </div>
                        </Draggable>
                    </Container>
                    <v-row>
                        <v-spacer></v-spacer>
                        <v-btn @click="addAnswer(index)" class="mb-4" fab dark small color="success">
                            <v-icon dark>
                                mdi-plus
                            </v-icon>
                        </v-btn>
                        <v-spacer></v-spacer>
                    </v-row>
                </div>
            </v-card-text>
        </v-card>
        <Add class="mt-4">
            <v-list>
                <v-list-item-group>
                    <v-list-item v-for="(type, idx) in types" :key="idx" @click="addQuestion(type.value)" color="primary">
                        <v-list-item-title>{{ type.label }}</v-list-item-title>
                    </v-list-item>
                </v-list-item-group>
            </v-list>
        </Add>
    </v-container>
</template>


<script>
import { Container, Draggable } from "vue-dndrop";
import { applyDrag } from "@/helpers/applyDrag.js";
import Add from '~/components/UI/Add'
export default {
    components: {
        Container,
        Draggable,
        Add
    },
    props: {
        quests: Array
    },
    data() {
        return {
            types: [
                {
                    label: 'Множественный выбор',
                    value: 'multiple'
                },
                {
                    label: 'По порядку',
                    value: 'order'
                },
                {
                    label: 'Вставить слово',
                    value: 'insert'
                }, {
                    label: 'Единственный вариант',
                    value: 'single'
                }
            ],
            typeRules: [val => (val || '').length > 0 || 'This field is required'],
            questions: [],
            currentDrag: 0,
            tt: 0
        }
    },
    beforeMount() {
        this.questions = JSON.parse(JSON.stringify(this.quests))
    },
    methods: {
        handleTestChange() {
            this.$emit('testChange', this.questions)
        },
        addAnswer(i) {
            this.questions[i].answers.push(`Ёлки иголки`)
        },
        removeAnswer(i, j) {
            this.questions[i].answers.splice(j, 1)
        },
        onDrop(dropResult) {
            let i = dropResult.index
            this.questions[i].answers = applyDrag(this.questions[i].answers, dropResult);
            this.questions[i].rightAnswer = this.questions[i].answers
            console.log(this.questions[i])
        },
        setCurrentDrag(index) {
            console.log(index)
            this.currentDrag = index
            this.questions[index].rightAnswer = this.questions[index].answers
        },
        addQuestion(type) {
            this.questions.push({
                number: this.questions.length ? this.questions.length : 0,
                contain: 'При чём здесь ёжики?',
                answers: [
                    'Смешарики',
                    'Лошарики',
                    'Кикорики'
                ],
                type: type,
                rightAnswer: ['Смешарики']
            })
        },
        removeQuestion(i) {
            this.questions.splice(i, 1)
        }
    },
    watch: {
        questions: {
            handler: function () {
                for (let item of this.questions) {
                    if (item.type == 'Расположить по порядку') {
                        item.answers = item.rightAnswer
                    }
                }
                this.$emit('testChange', this.questions)
            },
            deep: true
        }
    }
}
</script>

<style lang="scss">
.draggable-item {
    .v-input {
        margin-left: 0;
        max-width: 80%;
    }

    input {}

    padding: 10px !important
}
</style>

