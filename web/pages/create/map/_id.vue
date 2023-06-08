<template>
    <div>
        <Header class="z-index-100" title='Отметить точку' />
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
            <div class="map-container">
                <l-map :zoom="zoom" :center="currentLoc" ref="map">
                    <l-tile-layer :url="url" :attribution="attribution" />
                    <l-marker :lat-lng="center">
                        <l-tooltip :options="{ permanent: true, interactive: true }">
                            <div>
                                Вы здесь
                            </div>
                        </l-tooltip>
                    </l-marker>
                    <l-marker :lat-lng="currentLoc">
                        <l-tooltip :options="{ permanent: true, interactive: true }">
                            <div>
                                Отмечено
                            </div>
                        </l-tooltip>
                    </l-marker>
                </l-map>
            </div>
            <div class="text-body-1 mt-3">
                Широта
            </div>
            <v-text-field readonly solo class="mt-2" v-model.number="lat" />
            <div class="text-body-1 mt-1">
                Долгота
            </div>
            <v-text-field readonly solo class="mt-2" v-model.number="long" />
            <v-btn @click="setStage" class="w-100" color="primary">Отметить здесь</v-btn>
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
import { latLng } from "leaflet";
import { Icon } from 'leaflet';
export default {
    components: {
        Header
    },
    data() {
        return {
            quest: {},
            snackbar: false,
            stage: {},
            currentLat: 0,
            currentLong: 0,
            url: 'https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png',
            attribution:
                '&copy; <a href="http://osm.org/copyright">OpenStreetMap</a> contributors',
            lat: 0,
            long: 0,
            zoom: 14,
            geo: null,
            id: 0
        }
    },
    created() {
        this.id = this.$route.params.id
        this.quest = { ...this.$store.getters['create/getCurrentQuest'] }
        this.stage = { ...this.$store.getters['create/getCurrentStage'] }
        // if(this.stage && this.stage.type !='Карта') this.$router.go(-1)

        if (this.stage && this.stage.coords?.lat) {
            this.currentLat = this.stage.coords.lat
        }
        if (this.stage && this.stage.coords?.long) {
            this.currentLong = this.stage.coords.long
        }

        function success({ coords }) {
            const { latitude, longitude } = coords
            const position = [latitude, longitude]
            this.lat = position[0]
            this.long = position[1]
        }
        navigator.geolocation.getCurrentPosition(() => { }, error, {
            enableHighAccuracy: true,
            timeout: 10000
        })
        success = success.bind(this)
        function error({ message }) {
            console.log(message)
        }
        navigator.geolocation.getCurrentPosition(success, error, {
            enableHighAccuracy: true
        })
        this.$nextTick(() => {
            this.geo = navigator.geolocation.watchPosition(success, error, {
                enableHighAccuracy: true
            })
        })
        delete Icon.Default.prototype._getIconUrl;
        Icon.Default.mergeOptions({
            iconRetinaUrl: require('leaflet/dist/images/marker-icon-2x.png'),
            iconUrl: require('leaflet/dist/images/marker-icon.png'),
            shadowUrl: require('leaflet/dist/images/marker-shadow.png'),
        });
    },
    destroyed() {
        navigator.geolocation.clearWatch(this.geo)
    },
    methods: {
        setStage() {
            this.currentLat = this.lat
            this.currentLong = this.long
            this.stage.coords = {
                latitude: this.lat,
                longitude: this.long
            }
            this.quest.stages[this.id] = this.stage
            this.$store.commit('create/setCurrentQuest', this.quest)
            this.snackbar = true
        },
    },
    computed: {
        center() {
            return latLng(this.lat, this.long)
        },
        currentLoc() {
            return latLng(this.currentLat, this.currentLong)
        }
    },

}
</script>

<style lang="scss" scoped>
.map-container {
    height: 300px !important;
    overflow: hidden;
    z-index: 0 !important;

    div {
        z-index: 0 !important;
    }
}

.w-100 {
    width: 100%
}

.z-index-100 {
    position: relative;
    z-index: 100 !important;
}
</style>


