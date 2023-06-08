<template>
    <div>
        <Header title='Генерировать код' />
        <v-main class="pa-2 mt-4">
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
            <v-text-field :value="stage.code || 'default'" v-model="stage.code" @input="$v.stage.code.$touch()"
                @blur="$v.stage.code.$touch()" :error-messages="toErrors" solo label="Кодовое слово" />
            <div>
                <h2 class="text-body-1 my-2">Текущий код</h2>
                <v-card>
                    <v-card-text>
                        <qr-code :size="512" ref="qrRef" :text="stage.code"></qr-code>
                        <v-btn download="qr.png" :href="qr" fab color="warning" small class="download">
                            <v-icon>mdi-download</v-icon>
                        </v-btn>
                    </v-card-text>
                </v-card>
                <v-card-actions class="pa-0 mt-4">
                    <v-spacer></v-spacer>
                    <v-btn @click="$router.go(-1)" text>Назад</v-btn>
                    <v-btn @click="setStage" color="primary">Сохранить</v-btn>
                </v-card-actions>
            </div>
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
import { mapState } from 'vuex'
import { validationMixin } from 'vuelidate'
import { required, minLength } from 'vuelidate/lib/validators'
import qrCode from 'vue-qr-generator'
export default {
    mixins: [validationMixin],
    validations: {
        stage: {
            code: { required, minLength: minLength(2) }
        }
    },
    components: {
        Header,
        qrCode
    },
    created() {
        this.id = this.$route.params.id
        this.quest = { ...this.$store.getters['create/getCurrentQuest'] }
        this.stage = { ...this.$store.getters['create/getCurrentStage'] }

        // if(this.stage && this.stage.type !='QR') this.$router.go(-1)
    },
    data() {
        return {
            stage: '',
            qr: '',
            id: 0,
            codeWord: '',
            snackbar: false
        }
    },
    methods: {
        setStage() {
            this.$v.stage.$touch()
            if (this.$v.stage.$anyError) return
            this.quest.stages[this.id] = this.stage
            this.$store.commit('create/setCurrentQuest', this.quest)
            this.snackbar = true
        },

    },
    watch: {
        'stage.code'(val) {
            this.$nextTick(() => {
                this.qr = this.$refs.qrRef?.$el.getElementsByTagName("img")[0].src
            })
        }
    },
    computed: {
        ...mapState('create', ['currentStage']),
        toErrors() {
            const errors = []
            if (!this.$v.stage.code.$dirty) return errors
            !this.$v.stage.code.minLength && errors.push('Слишком короткое кодовое слово')
            !this.$v.stage.code.required && errors.push('Кодовое слово обязательно')
            return errors
        }
    }
}
</script>

<style lang="scss">
.qr {
    position: relative;
}

.download {
    position: absolute;
    bottom: 10px;
    right: 10px;
}
</style>


