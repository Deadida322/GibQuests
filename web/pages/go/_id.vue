<template>
    <div v-if="quest && quest.stages">
        <Header title='QuestGib' />
        <v-row class="my-3 d-flex justify-space-between text-center align-center">
            <v-col no-gutters class="col col-2">
                <v-icon @click="$router.go(-1)" large>
                    mdi-arrow-left
                </v-icon>
            </v-col>
            <v-col class="col text-h5 col-4">
                <v-card v-if="currentStage" class="rounded-pill grey lighten-4">
                    Этап <span class="primary--text">{{ currentStageNumber + 1 }}</span>
                </v-card>
                <v-card v-else class="rounded-pill grey lighten-4">
                    <span class="primary--text">Всё!</span>
                </v-card>
            </v-col>
            <v-col class="col col-2"></v-col>
        </v-row>
        <v-main v-if="currentStage" class="pa-2 mb-10">
            <v-row>
                <v-col cols="0" md="2"></v-col>
                <v-col cols="12" md="8" class="pa-4 pa-md-0">
                    <v-card class="relative mb-4">
                        <v-chip small class="stage-chip">{{ currentStage.type }}</v-chip>
                        <div class="primary--text pa-2 text-h6 text-center">
                            {{ currentStage.title }}
                        </div>
                        <VideoStage @stageComplete="stageComplete" v-if="currentStage.type == 'video'"
                            :stage="currentStage" />
                        <TextStage v-if="currentStage.type == 'text'" :stage="currentStage" />
                        <MapStage v-if="currentStage.type == 'map'" :key="currentStageNumber + 1"
                            @stageComplete="stageComplete" :stage="currentStage" />
                        <QRStage v-if="currentStage.type == 'qrCode'" @stageComplete="stageComplete"
                            :stage="currentStage" />
                        <TestStage v-if="currentStage.type == 'test'" @stageComplete="stageComplete"
                            :stage="currentStage" />
                        <v-card-actions>
                            <v-btn :disabled="!currentStageNumber" @click="previousStage" dark color="blue">Назад</v-btn>
                            <v-spacer></v-spacer>
                            <v-btn :disabled="!showBtn" @click="nextStage" color="primary">Далее</v-btn>
                        </v-card-actions>
                    </v-card>
                </v-col>
                <v-col cols="0" md="2"></v-col>
            </v-row>

        </v-main>
        <v-container v-else class="d-flex align-center flex-column justify-center w-100">
            <v-btn large to="/" fab color="success">
                <v-icon>
                    mdi-check
                </v-icon>
            </v-btn>
            <div class="text-h3">
                Ты умница!
            </div>
        </v-container>
        <Progress :goal="quest.stages && quest.stages.length"
            :current="currentStageNumber >= quest.stages.length ? quest.stages.length - 1 : currentStageNumber" />
    </div>
    <div class="w-100 mt-5 d-flex justify-center" v-else>
        <v-progress-circular :size="70" :width="4" color="deep-purple" indeterminate></v-progress-circular>
    </div>
</template>


<script>
import Header from '~/components/UI/Header'
import VideoStage from '~/components/Go/VideoStage'
import TextStage from '~/components/Go/TextStage'
import MapStage from '~/components/Go/MapStage'
import QRStage from '~/components/Go/QRStage'
import TestStage from '~/components/Go/TestStage'
import Progress from '~/components/Go/Progress'
import { mapGetters, mapState } from 'vuex'

export default {
    components: {
        Header,
        VideoStage,
        TextStage,
        MapStage,
        QRStage,
        TestStage,
        Progress
    },
    created() {
        if (!this.user.id) this.$router.push('/login')
    },
    mounted() {
        this.id = this.$route.params.id
        const baseUrl = "ws://127.0.0.1:9007/room"
        this.$request({
            url: `/ProcessQuest/ConnectToQuest`,
            method: "POST",
            headers: { 'Authorization': `Bearer ${this.accessToken}` },
            data: {
                id: this.id
            }
        }).then((res) => {
            const url = `${baseUrl}/${res.room}`
            this.ws = new WebSocket(url)
            this.quest = res.quest

            this.ws.onerror = (err) => console.log(err, 'err')
            this.ws.onopen = (e) => console.log(e, "open")
            this.ws.onmessage = e => {
                const res = JSON.parse(e.data)
                if (res.success) {
                    this.currentStageNumber = res.stage
                    this.showBtn = false
                } else {
                    this.$showError(res.error)
                }
            }
        })

    },
    data() {
        return {
            quest: {},
            id: 0,
            currentStageNumber: 0,
            showBtn: false,
            ws: false
        }
    },
    methods: {
        stageComplete() {
            this.showBtn = true
        },
        nextStage() {
            // this.showBtn = false
            const data = {
                userId: this.user.id,
                stage: this.currentStage
            }
            this.ws.send(JSON.stringify(data))
            // this.currentStageNumber++
        },
        previousStage() {
            this.showBtn = true
            this.currentStageNumber--
        }
    },
    computed: {
        ...mapState('auth', ['isLoggedIn', 'user', 'accessToken']),
        currentStage() {
            let stage = {}
            if (this.quest && this.quest.stages) {
                stage = this.quest.stages[this.currentStageNumber]
            }
            if (stage && stage.type === 'text') {
                this.showBtn = true
            }
            return stage || false
        }
    }
}
</script>
