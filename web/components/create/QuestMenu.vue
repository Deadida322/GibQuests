<template>
    <v-menu
        top
        offset-y
        transition="slide-x-transition"
    >
        <template v-slot:activator="{ on, attrs }">
            <v-btn
                icon
                v-bind="attrs"
                v-on="on"
            >
                <v-icon>mdi-dots-vertical</v-icon>
            </v-btn>
        </template>
        <v-list>
            <v-list-item-group>
                <v-list-item :to="`/watch/${id}`" color="blue">
                    <v-list-item-icon>
                        <v-icon color="blue">mdi-eye</v-icon>
                    </v-list-item-icon>
                    <v-list-item-title>Отслеживать</v-list-item-title>
                </v-list-item>
                <v-list-item :to="`/create/${id}`" color="primary">
                    <v-list-item-icon>
                        <v-icon color="primary">mdi-pencil</v-icon>
                    </v-list-item-icon>
                    <v-list-item-title>Редактировать</v-list-item-title>
                </v-list-item>
                <v-list-item @click="copy" color="warning">
                    <v-list-item-icon>
                        <v-icon color="warning">mdi-clipboard</v-icon>
                    </v-list-item-icon>
                    <v-list-item-title>Копировать url</v-list-item-title>
                </v-list-item>
                <v-list-item @click="$emit('remove')" color="error">
                    <v-list-item-icon>
                        <v-icon color="error" class="mr-0">mdi-delete</v-icon>
                    </v-list-item-icon>
                    <v-list-item-title prepend-icon="mdi-delete">Удалить</v-list-item-title>
                </v-list-item>
            </v-list-item-group>
        </v-list>
    </v-menu>
</template>


<script>
    export default {
        props:{
            id: {
                type: Number | String,
                required: true
            }
        },
        methods:{
            copy(){
                const ctx = this
                navigator.clipboard.writeText(`${window.location.href.replace('yours','view')}/${this.id}`).then(function() {
                    console.log(window.location.href);
                }, function() {
                    console.log('Copy error')
                });
            }
        }
    }
</script>

