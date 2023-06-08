<template>
    <v-text-field
        solo
        :label="label"
        :placeholder="placeholder"
        append-icon="mdi-magnify"
        @input="onSearch($event)"
    ></v-text-field>
</template>

<script>
export default {
    data(){
        return{
            
        }
    },
    props:{
        label: {
            required: false,
            type: String
        },
        placeholder: {
            required: false,
            type: String
        }
    },
    methods:{
        onSearch(e){
            this.$emit('search', e)
        }
    }
}
</script>

