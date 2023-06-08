<template>
    <div>
        <div class="map-container">
            <l-map :zoom="zoom" :center="yourPosition" ref="map">
                <l-tile-layer :url="url" :attribution="attribution" />
                <l-marker :lat-lng="yourPosition">
                    <l-tooltip :options="{ permanent: true, interactive: true }">
                        <div>
                            Вы здесь
                        </div>
                    </l-tooltip>
                </l-marker>
                <l-marker :lat-lng="goal">
                    <l-tooltip :options="{ permanent: true, interactive: true }">
                        <div>
                            Цель
                        </div>
                    </l-tooltip>
                </l-marker>
            </l-map>
        </div>
        <v-card-text>
            <h2 class="text-body-1">
                Ваше местоположение: <br><span class="primary--text">{{ yourLat + ' ' + yourLong }}</span>
            </h2>
            <h2 class="text-body-1">
                Цель: <br><span class="primary--text">{{ stage.coords.latitude + ' ' + stage.coords.longitude }}</span>
            </h2>
        </v-card-text>

    </div>
</template>

<script>
import { latLng } from "leaflet";
import { Icon } from 'leaflet';

export default {
    data() {
        return {
            key: 0,
            geo: 0,
            latSuccess: false,
            longSuccess: false,
            yourLat: 0,
            yourLong: 0,
            url: 'https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png',
            zoom: 10,
            attribution:
                '&copy; <a href="http://osm.org/copyright">OpenStreetMap</a> contributors',
        }
    },
    mounted() {
        function success(e) {
            const { latitude, longitude } = e.coords
            const position = [latitude, longitude]
            this.yourLat = position[0]
            this.yourLong = position[1]
        }
        success = success.bind(this)
        function error({ message }) {
            console.log(message)
        }
        navigator.geolocation.getCurrentPosition(() => {
        }, error, {
            enableHighAccuracy: true,
            timeout: 10000
        })
        this.geo = navigator.geolocation.watchPosition(success, error, {
            enableHighAccuracy: true
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
    props: {
        stage: {
            type: Object,
            required: true
        }
    },
    computed: {
        yourPosition() {
            return latLng(this.yourLat, this.yourLong)
        },
        goal() {
            return latLng(this.stage.coords.latitude, this.stage.coords.longitude)
        }
    },
    watch: {
        yourLat() {
            console.log(Math.abs(this.yourLat - this.goalLat))
            if (Math.abs(this.yourLat - this.stage.coords.latitude) < 0.0001500) {
                this.latSuccess = true
            }
            if (this.latSuccess && this.longSuccess) this.$emit('stageComplete')

        },
        yourLong() {
            console.log(this.yourLong - this.goalLong)

            if (Math.abs(this.yourLong - this.stage.coords.longitude) < 0.0001500) {
                this.longSuccess = true
            }
            if (this.latSuccess && this.longSuccess) this.$emit('stageComplete')
            console.log(this.latSuccess, this.longSuccess);

        }
    }
}
</script>

<style lang="scss" scoped>
.map-container {
    height: 400px !important;
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

