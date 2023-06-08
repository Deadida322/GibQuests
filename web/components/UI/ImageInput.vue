<template>
    <div @click="$emit('click')" class="IIContainer br-4" @dragover="drag = true" @mouseleave="drag = false"
        @dragleave="drag = false" :class="{ dragover: drag }">
        <v-img rounded max-height="250px" class='img' ref="dd" :src="backgroundComputed">
            <v-container class="image_input__container pa-4">
                <label for="file_input" class="file_input__label">
                    <v-btn icon color="primary" x-large class="d-flex align-center flex-column justify-center">
                        <v-icon class="icon" large>
                            mdi-upload
                        </v-icon>
                    </v-btn>
                    <div v-if="error" class="red--text caption">{{ error }}</div>
                    <div v-if="fileName" class="caption">{{ fileName }}</div>
                </label>
                <input :capture="camera ? 'capture' : 'none'" @change="filePicker" type="file" class="file_input"
                    id="file_input" accept="image/jpeg,image/png,image/jpg">
            </v-container>
        </v-img>
    </div>
</template>

<script>
import { mapState } from "vuex"
export default {
    data() {
        return {
            image: '',
            fileName: null,
            error: null,
            drag: false,
        }
    },
    props: {
        rules: [Array],
        background: null,
        camera: false
    },
    methods: {
        filePicker(e) {
            try {
                let file = e.target.files[0]
                if (file.type != 'image/png' && file.type != 'image/jpeg' && file.type != 'image/jpg') {
                    this.error = 'Неверный формат'
                    this.fileName = ''
                    return
                }
                if (file.size >= 10000000) {
                    this.error = 'Слишком большой размер'
                    this.fileName = ''
                    return
                }
                this.fileName = file.name
                this.error = ''
                this.$emit('change', file)
            }
            catch {
                this.error = 'Выберите файл'
                this.fileName = ''
            }
        }
    },
    computed: {
        ...mapState('auth', ['base']),
        backgroundComputed() {
            if (!this.background) return 'https://paltodaytv.com/img/default.jpg'
            if (this.background.length > 100) return this.background
            return this.background
        }
    }
}
</script>


<style lang="scss">
.image_input {
    min-height: auto !important;
}

.image_input__container {
    transition: all .3s ease-in-out;
    transform: translateY(-10px);
    opacity: 0;
    width: 100% !important;
    height: 100%;
    display: flex;
    flex-direction: column;
    justify-content: center;
    align-items: center;

    .file_input {
        width: 100% !important;
    }
}

.dragover {
    .img {
        transform: scale(1.1);
    }

    .image_input__container {
        transform: translateY(0px);
        opacity: 1;
    }
}

.IIContainer {
    overflow: hidden;

    * {
        transition: all .4s ease-in-out;
    }

    .img {
        position: relative;
    }

    &:hover {
        .img {
            transform: scale(1.1);
        }

        .image_input__container {
            transform: translateY(0px);
            opacity: 1;
        }
    }

    .file_input {
        opacity: 0;
        width: 100%;
        height: 100%;
        cursor: pointer;
        position: absolute;
        top: 0;
        left: 0
    }

    .file_input__label {
        display: flex;
        justify-content: center;
        align-items: center;
        width: 100%;
        height: 100%;
        color: white;
        flex-direction: column;
        transform: scale(1.1);
        background-color: rgba(0, 0, 0, 0.4);

        .icon {
            background-image: url("data:image/svg+xml,%3csvg width='100%25' height='100%25' xmlns='http://www.w3.org/2000/svg'%3e%3crect width='100%25' height='100%25' fill='none' stroke='%23E6E6E685' stroke-width='4' stroke-dasharray='6%2c 14' stroke-dashoffset='4' stroke-linecap='square'/%3e%3c/svg%3e");
            padding: 1em 2em;
            transform: scale(1.5)
        }
    }
}

.br-4 {
    border-radius: 4px;
}
</style>


