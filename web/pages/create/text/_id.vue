<template>
    <div>
        <Header title='Редактор текста' />
        <v-row class="my-3 d-flex justify-space-between text-center align-center">
            <v-col no-gutters class="col col-2">
                <v-icon @click="$router.go(-1)" large>
                    mdi-arrow-left
                </v-icon>
            </v-col>
            <v-col class="col secondary--text text-h6 col-6">
                {{ quest.id }} / {{ id }}
            </v-col>
            <v-col class="col col-2"></v-col>
        </v-row>
        <v-main class="pa-2 mt-4">
            <v-card @click="$v.stage.text.$touch()" :class="{ 'redBorder': textErrors.length }">
                <wysiwyg v-model="stage.text" />
            </v-card>
            <v-card v-if="stage.text" class="mt-4 pa-2" v-html="stage.text"></v-card>
            <v-card-actions class="pa-0 mt-4">
                <v-spacer></v-spacer>
                <v-btn @click="$router.go(-1)" text>Назад</v-btn>
                <v-btn @click="setStage" color="primary">Сохранить</v-btn>
            </v-card-actions>
        </v-main>
        <v-snackbar v-model="snackbar">
            Изменения сохранены
            <template v-slot:action="{ attrs }">
                <v-btn class="primary--text" text small @click="snackbar = !snackbar">
                    Закрыть
                </v-btn>
            </template>
        </v-snackbar>
    </div>
</template>

<script>
import Header from '~/components/UI/Header'
import { validationMixin } from 'vuelidate'
import { required, minLength } from 'vuelidate/lib/validators'
export default {
    mixins: [validationMixin],
    validations: {
        stage: {
            text: { required, minLength: minLength(10) }
        }
    },
    components: {
        Header
    },
    created() {
        this.id = this.$route.params.id
        this.quest = { ...this.$store.getters['create/getCurrentQuest'] }
        this.stage = { ...this.$store.getters['create/getCurrentStage'] }
        if (this.stage.stageAction && this.stage.stageAction.text) {
            this.stage.text = this.stage.stageAction.text
        }

        // if(this.stage && this.stage.type !='Текст') this.$router.go(-1)
    },
    data() {
        return {
            snackbar: false,
            stage: {},
            quest: {},
            text: '',
            id: 0,
        }
    },
    methods: {
        setStage() {
            this.$v.stage.$touch()
            if (this.$v.stage.$anyError) return
            this.quest.stages[this.id] = this.stage
            this.$store.commit('create/setCurrentQuest', this.quest)
            this.snackbar = true
        }
    },
    computed: {
        textErrors() {
            const errors = []
            if (!this.$v.stage.text.$dirty) return errors
            !this.$v.stage.text.required && errors.push('Введите текст')
            !this.$v.stage.text.minLength && errors.push('Текст должен быть содержательным')
            return errors
        }
    }
}
</script>

