<template>
    <div>
        <Header title='QuestGib' />
        <div @click="$router.go(-1)" class="my-3">
            <v-icon large>
                mdi-arrow-left
            </v-icon>
        </div>
        <v-main class="mt-4">
            <v-row>
                <v-col cols="0" md="2"></v-col>
                <v-col cols="12" md="8" class="pa-4 pa-md-0">
                    <template v-if="quest">
                        <v-img class="d-flex text-center align-center" max-height="400px"
                            gradient="to top right, rgba(0,0,0,.4), rgba(25,32,72,.6)" dark :src="quest.img">
                            <div class="text-h4">{{ quest.title }}</div>
                        </v-img>
                        <v-card class="text-body-1 pa-2 mt-2" v-html="quest.description" />
                        <div class="text-h5 mt-2">Этапы</div>
                        <Stage v-for="(item, idx) in quest.stages" class="mt-4" :key="idx" :idx="idx" :stage="item" />
                        <div class="d-flex mt-4">
                            <v-spacer /><v-btn :to="`/go/${quest.id}`" color="primary">Пройти</v-btn>
                        </div>
                    </template>
                    <div class="w-100 d-flex justify-center" v-else>
                        <v-progress-circular :size="70" :width="4" color="deep-purple" indeterminate></v-progress-circular>
                    </div>
                </v-col>
                <v-col cols="0" md="2"></v-col>
            </v-row>

        </v-main>

    </div>
</template>


<script>
import Header from '~/components/UI/Header'
import Stage from '~/components/Stage.vue'
export default {
    data() {
        return {
            id: 0,
            quest: false
        }
    },
    created() {
        this.id = this.$route.params.id
        this.$request({
            url: `/GenerateQuest/GetQuest/${this.id}`,
            method: "GET"
        }).then(res => {
            this.quest = res
        })
    },
    components: {
        Header,
        Stage
    }
}
</script>
