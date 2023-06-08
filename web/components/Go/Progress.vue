<template>
    <v-card class="progress_container">
        <div class="line_container" :style="{'width': ((current+1)/goal)*100+'%'}">
            <div class="text-center">
                {{current+1}}/{{goal}}
            </div>
        </div>
    </v-card>
</template>


<script>
export default {
    props:{
        goal: 0,
        current: 0
    }
}
</script>

<style lang="scss" scoped>
    .progress_line{
        position: absolute
    }
    .line_container{
        width: 50%;
        height: 22px;
        background: rgb(253,246,200);
        background: linear-gradient(90deg, rgba(253,246,200,1) 0%, rgba(252,126,87,1) 13%, rgba(157,90,161,1) 28%, rgba(111,89,227,1) 44%, rgba(182,168,254,1) 67%, rgba(81,44,212,1) 88%, rgba(14,41,91,1) 100%);
    }
    .progress_container{
        color: white;
        left: 0.5%;
        position: absolute;
        bottom: 10px;
        width: 99%;
        height: 22px;
    }
</style>

