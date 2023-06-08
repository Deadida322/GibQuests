<template>
    <div>
        <v-main>
            <v-row class="my-3 d-flex justify-space-between text-center align-center">
                <v-col no-gutters class="col col-2" />
                <v-col class="col secondary--text col-12 col-md-8 сol-lg-6 pa-4 pa-md-0">
                    <v-row class="ma-0 pa-0">
                        <v-col class="col-1 col">
                            <v-icon class="text-left justify-start flex-grow-1" @click="$router.push('/yours')" large>
                                mdi-arrow-left
                            </v-icon>
                        </v-col>
                        <v-col class="col-10 col text-h6">{{ quest.title }} </v-col>
                        <v-col class="col-1 col"></v-col>
                    </v-row>
                    <v-text-field @input="$v.quest.title.$touch()" @blur="$v.quest.title.$touch()" v-model="quest.title"
                        :error-messages="titleErrors" :counter="20" solo label="Название" />
                    <div class="text-h5">Описание</div>
                    <v-card v-ripple="false" @click="$v.quest.description.$touch()" @blur="$v.quest.description.$touch()"
                        class="mt-2 mb-2">
                        <wysiwyg v-model="quest.description" />
                    </v-card>
                    <v-alert border="top" color="red lighten-2" v-if="descriptionErrors.length" dark>
                        {{ descriptionErrors[0] }}
                    </v-alert>
                    <ImageInput @click="$v.quest.img.$touch()" :class="{ 'redBorder': imageErrors.length }"
                        @change="setImage" :background="background" />
                    <v-alert class="mt-2" border="top" color="red lighten-2" v-if="imageErrors.length" dark>
                        {{ imageErrors[0] }}
                    </v-alert>
                    <div class="mt-4 mb-8 d-flex align-center justify-space-between">
                        <div class="text-h5">Этапы</div>
                        <Shuffling v-if="quest.stages && quest.stages.length" @shuffleComplete="shuffleComplete"
                            :items="quest.stages" />
                    </div>
                    <wrapper :key="key">
                        <Stage v-for="(item, idx) in quest.stages" :key="idx" class="mb-7"
                            :class="{ 'redBorder': myStagesErrors[idx] }" :item="item" :idx="+idx"
                            @remove="removeStage(idx)" @createQR="createQR(idx)" @createText="createText(idx)"
                            @createTest="createTest(idx)" @createMap="createMap(idx)"
                            @stageChange="stageChange($event, idx)" />
                    </wrapper>
                    <v-alert class="mt-2" border="top" color="red lighten-2" v-if="stagesErrors.length" dark>
                        {{ stagesErrors[0] }}
                    </v-alert>
                    <v-alert class="mt-2" border="top" color="red lighten-2" v-if="myStagesErrors.length" dark>
                        Поля в этапах не могут быть пустыми
                    </v-alert>
                    <Add class="mt-4">
                        <v-list>
                            <v-list-item-group>
                                <v-list-item @click="addStage(item)" v-for="item in types" :key="item" color="primary">
                                    <v-list-item-title>{{ item }}</v-list-item-title>
                                </v-list-item>
                            </v-list-item-group>
                        </v-list>
                    </Add>
                    <v-card-actions class="pa-0 pt-2">
                        <v-spacer />
                        <v-btn @click="$router.push('/yours')">Отмена</v-btn>
                        <v-btn @click="setQuest()" color="primary">Сохранить</v-btn>
                    </v-card-actions>
                </v-col>
                <v-col class="col col-2" />
            </v-row>
        </v-main>
    </div>
</template>

<script>
import Header from '~/components/UI/Header.vue'
import toBase64 from '~/helpers/toBase64'
import Search from '~/components/UI/Search.vue'
import Quest from '~/components/Quest.vue'
import Add from '~/components/UI/Add'
import Stage from '~/components/create/Stage'
import ImageInput from '~/components/UI/ImageInput'
import { mapState } from 'vuex'
import { validationMixin } from 'vuelidate'
import { required, maxLength, minLength } from 'vuelidate/lib/validators'
import Shuffling from '~/components/create/Shuffling.vue'
import wrapper from '~/components/UI/wrapper'
export default {
    mixins: [validationMixin],
    validations: {
        quest: {
            title: { required, maxLength: maxLength(20) },
            description: { required, minLength: minLength(10) },
            img: { required },
            stages: { required }
        }
    },
    async created() {
        if (!this.isLoggedIn) this.$router.push('/login')
        this.currentId = this.$route.params.id
        console.log(this.currentQuest)
        if (this.currentQuest) {
            this.quest = JSON.parse(JSON.stringify(this.currentQuest))
            this.background = this.quest.img
        }
        if (this.currentId != 'new' && this.currentId != this.currentQuest.id) {
            if (this.quest.id && this.quest.id != 'new') return
            this.$request({
                url: `/GenerateQuest/GetQuest/${this.currentId}`,
                methods: "GET",
            }).then((res) => {
                this.quest = res
                console.log(this.quest)
                this.quest.stages = this.quest.stages.sort((a, b) => a.number - b.number)
                this.$store.commit('create/setCurrentQuest', this.quest)
                this.background = this.quest.img
            })
        }
        if (this.currentId != this.quest.id) {
            // this.$router.push(`/create/${this.quest.id}`)
        }
    },
    data() {
        return {
            key: 0,
            currentId: '',
            background: '',
            types: [
                'text',
                'video',
                'qrCode',
                'map',
                'test'
            ],
            quest: {},
        }
    },
    // layout: 'creating',
    components: {
        Header,
        Search,
        Quest,
        Add,
        ImageInput,
        Stage,
        Shuffling,
        wrapper
    },
    methods: {
        stageChange(e, idx) {
            this.myStagesErrors
            this.quest.stages[idx] = e
            this.$store.commit('create/setCurrentQuest', this.quest)
        },
        async setImage(e) {
            toBase64(e).then((res) => {
                this.quest = {
                    ...this.quest,
                    img: res
                }
                this.background = res
                this.$store.commit('create/setCurrentQuest', this.quest)
            })
        },
        createTest(id) {
            this.$store.commit('create/setCurrentStage', JSON.parse(JSON.stringify(this.quest.stages[id])))
            this.$router.push(`/create/test/${id}`)
        },
        createText(id) {
            this.$store.commit('create/setCurrentStage', JSON.parse(JSON.stringify(this.quest.stages[id])))
            this.$router.push(`/create/text/${id}`)
        },
        createMap(id) {
            this.$store.commit('create/setCurrentStage', JSON.parse(JSON.stringify(this.quest.stages[id])))
            this.$router.push(`/create/map/${id}`)
        },
        createQR(id) {
            this.$store.commit('create/setCurrentStage', JSON.parse(JSON.stringify(this.quest.stages[id])))
            this.$router.push(`/create/QR/${id}`)
        },
        addStage(type) {
            this.$v.quest.stages.$touch()
            let toPush = {
                number: this.quest.stages.length + 1,
                name: `Этап ${this.quest.stages.length + 1}`,
                type,
            }
            if (type == 'Видео') toPush.url = 'https://www.youtube.com/watch?v=dQw4w9WgXcQ'
            this.quest.stages.push(toPush)
            this.$store.commit('create/setCurrentQuest', { ...this.quest })
            this.$store.commit('create/setCurrentStage', toPush)
        },
        removeStage(idx) {
            this.quest.stages.splice(idx, 1)
            this.$store.commit('create/setCurrentQuest', this.quest)
        },
        shuffleComplete(stages) {
            this.key++
            this.quest.stages = JSON.parse(JSON.stringify(stages))
        },
        setQuest() {
            this.$v.quest.$touch()
            if (this.currentId == 'new') {
                this.$request({
                    url: '/GenerateQuest/CreateQuest',
                    method: "POST",
                    data: {
                        ...this.quest,
                    },
                })

            } else {
                this.$request({
                    url: '/GenerateQuest/UpdateQuest',
                    method: "PUT",
                    data: {
                        ...this.quest,
                    },
                }).then(res => {
                    this.currentId = res.id
                    this.$router.push(`/create/${this.currentId}`)
                })
            }
        }
    },
    computed: {
        ...mapState('auth', ['user', 'isLoggedIn']),
        ...mapState('create', ['createdQuests', 'currentQuest']),
        titleErrors() {
            const errors = []
            if (!this.$v.quest.title.$dirty) return errors
            !this.$v.quest.title.maxLength && errors.push('Слишком короткое название')
            !this.$v.quest.title.required && errors.push('Название обязательно')
            return errors
        },
        descriptionErrors() {
            const errors = []
            if (!this.$v.quest.description.$dirty) return errors
            !this.$v.quest.description.minLength && errors.push('Слишком короткое описание')
            !this.$v.quest.description.required && errors.push('Описание обязательно')
            return errors
        },
        imageErrors() {
            const errors = []
            if (!this.$v.quest.img.$dirty) return errors
            !this.$v.quest.img.required && errors.push('Необходимо выбрать изображение')
            return errors
        },
        stagesErrors() {
            const errors = []
            if (!this.$v.quest.stages.$dirty) return errors
            !this.$v.quest.stages.required && errors.push('Создайте этапы')
            return errors
        },
        summaryStagesErrors() {
            for (let i of this.myStagesErrors) {
                if (i) return false
            }
            return true
        },
        myStagesErrors() {
            const errors = []
            if (!this.$v.quest.stages.$dirty) return errors
            for (let i in this.quest.stages) {
                if (!this.quest.stages[i].title) errors[i] = true
                if (this.quest.stages[i].type == 'text') {
                    if (!this.quest.stages[i].text) errors[i] = true
                }
                if (this.quest.stages[i].type == 'video') {
                    const reg = new RegExp(/https:\/\/(www.|.{0,})youtube\.com\/watch\?v=.{3,}/)
                    if (!(this.quest.stages[i].url
                        && reg.test(this.quest.stages[i].url?.toLowerCase())
                        || this.quest.stages[i].url?.includes('https://youtu.be/'))) errors[i] = true
                }
                if (this.quest.stages[i].type == 'qrCode') {
                    if (!this.quest.stages[i].code) errors[i] = true
                }
                if (this.quest.stages[i].type == 'map') {
                    if (!this.quest.stages[i]?.coords?.latitude || !this.quest.stages[i]?.coords?.longitude) errors[i] = true
                }
            }
            return errors
        },
    },
    watch: {
        'quest.image': {
            handler: async function (val) {
                if (val && val.type && val.type.includes('image')) {
                    this.background = await toBase64(val)
                }
            },
            deep: true
        }
    },
}
</script>

<style>
.redBorder {
    border: 2px solid red !important
}
</style>


