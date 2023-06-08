<template>
    <div v-if="user.userName">
        <Header title='QuestGib' />
        <div @click="$router.go(-1)" class="my-3">
            <v-icon large>
                mdi-arrow-left
            </v-icon>
        </div>
        <v-main class="pa-2 mt-4">
            <v-row>
                <v-col cols="0" md="2"></v-col>
                <v-col cols="12" md="8" class="pa-4 pa-md-0">
                    <ImageInput @change="setImage" :background="background" />
                    <v-card>
                        <v-card-title>
                            Общая информация
                        </v-card-title>
                        <v-form ref="personForm" v-model="personalValid">
                            <v-card-text>
                                <v-text-field :rules="[v => !!v || 'Укажите фамилию']" v-model="personalData.surname"
                                    outlined label="Фамилия" />
                                <v-text-field :rules="[v => !!v || 'Укажите имя']" v-model="personalData.name" outlined
                                    label="Имя" />
                                <div class="d-flex">
                                    <v-spacer />
                                    <v-btn @click="submitPersonal">Сохранить</v-btn>
                                </div>
                            </v-card-text>
                        </v-form>

                    </v-card>
                    <v-card class="mt-4">
                        <v-card-title>
                            Авторизационные данные
                        </v-card-title>
                        <v-card-text>
                            <v-form ref="authForm" v-model="authValid">
                                <v-text-field :rules="[v => !!v || 'Логин обязателен']" v-model="authData.userName" outlined
                                    label="Логин" />
                                <v-text-field :rules="[v => !!v || 'Пароль обязателен']" v-model="authData.password"
                                    outlined label="Пароль" />
                                <v-text-field :rules="[v => !!v || 'Повторите пароль']" v-model="authData.passwordRepeat"
                                    outlined label="Повторить пароль" />
                                <div class="d-flex">
                                    <v-spacer />
                                    <v-btn @click="submitAuth">Сохранить</v-btn>
                                </div>
                            </v-form>

                        </v-card-text>
                    </v-card>
                    <v-card class="mt-2">
                        <v-card-title>
                            Пройденные/текущие квесты
                        </v-card-title>
                        <v-card-text v-if="quests.length">
                            <Quest class="mt-2" v-for="item in quests" :item="item" />
                        </v-card-text>
                    </v-card>
                </v-col>
                <v-col cols="0" md="2"></v-col>
            </v-row>
        </v-main>
    </div>
</template>


<script>
import Header from '~/components/UI/Header'
import toBase64 from '~/helpers/toBase64'
import Stage from '~/components/Stage.vue'
import ImageInput from "~/components/UI/ImageInput.vue"
import { mapState } from "vuex"
import Quest from '~/components/Quest.vue'
export default {
    data() {
        return {
            id: 0,
            quests: [],
            background: "",
            personalValid: false,
            authValid: false,
            personalData: {},
            authData: {}
        }
    },
    created() {
        this.personalData = {
            img: this.user.img,
            name: this.user.name,
            surname: this.user.surname
        }
        this.background = this.personalData.img
        this.$request({
            url: "/GenerateQuest/GetFilteredQuests",
            method: "POST",
            headers: { 'Authorization': `Bearer ${this.accessToken}` },
            data: { id: this.user.id }
        }).then(res => {
            this.quests = res
        }).catch(err => {
            console.log(err, "err")
        })
    },
    components: {
        Header,
        Stage,
        ImageInput,
        Quest
    },
    methods: {
        async setImage(e) {
            toBase64(e).then((res) => {
                this.background = res
                this.personalData.img = res
            })
        },
        submitPersonal() {
            this.$refs.personForm.validate()
            if (!this.personalValid)
                return
            if (this.background) {
                this.$request({
                    url: "/Auth/UpdateUser",
                    method: "PATCH",
                    data: this.personalData
                }).then((res) => {
                    this.$showSuccess("Личные данные успешно изменены")
                }).catch((err) => {
                    this.$showError(err.join(', '))
                })
            } else {
                this.$showError("Укажите фото профиля")
            }
        },
        submitAuth() {
            this.$refs.authForm.validate()
            if (!this.authValid)
                return
            if (this.authForm.password == this.authForm.password) {
                this.$request({
                    url: "/Auth/UpdateUser",
                    method: "PATCH",
                    data: this.authData
                }).then((res) => {
                    this.$showSuccess("Авторизационные данные успешно изменены")
                }).catch((err) => {
                    this.$showError(err.join(', '))
                })
            } else {
                this.$showError("Пароли должны совпадать")
            }
        }
    },
    computed: {
        ...mapState("auth", ["user", "accessToken"])
    }
}
</script>
