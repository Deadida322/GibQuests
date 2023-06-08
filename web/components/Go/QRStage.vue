<template>
    <div>
        <v-card :loading="loading">
            <template slot="progress">
                <v-progress-linear color="deep-purple" height="10" indeterminate></v-progress-linear>
            </template>
            <ImageInput camera @change="setImage" :background="background" />
        </v-card>
        <v-snackbar v-model="error" timeout="3000">
            <span class="red--text">{{ errorText }}</span>
            <template v-slot:action="{ attrs }">
                <v-btn color="indigo" text v-bind="attrs" @click="error = false">
                    Окей
                </v-btn>
            </template>
        </v-snackbar>
    </div>
</template>

<script>
import ImageInput from '~/components/UI/ImageInput'
import QrScanner from 'qr-scanner';
import toBase64 from '~/helpers/toBase64';
export default {
    components: {
        ImageInput
    },
    data() {
        return {
            background: null,
            qr: '',
            error: false,
            errorText: '',
            loading: false
        }
    },
    props: {
        stage: {
            required: true,
            type: Object
        }
    },
    methods: {
        async setImage(e) {
            const ctx = this
            this.loading = true
            this.qr = e
            this.background = await toBase64(e)
            ctx.$emit('stageComplete')
            QrScanner.scanImage(e)
                .then(result => {
                    this.$router.push(`?q=${result}`)
                    this.loading = false
                    this.error = false
                    console.log(error)
                    if (result.toLowerCase() == this.stage.code.toLowerCase()) {
                        ctx.$emit('stageComplete')
                    }
                    else {
                        ctx.$emit('stageComplete')
                        this.loading = false
                        this.error = true
                        console.log()
                        this.errorText = 'Неверный код'
                    }
                })
                .catch(error => {
                    ctx.$emit('stageComplete')
                    this.error = true
                    this.errorText = 'QR-код не найден'
                    this.loading = false
                });
        },
    }
}
</script>
