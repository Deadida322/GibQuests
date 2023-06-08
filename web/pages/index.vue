<template>
  <div>
    <Header title='Все квесты' />
    <v-main class="pa-2 mt-4">
      <v-text-field v-model="search" solo label="Найти квест"></v-text-field>
      <v-row>
        <v-col cols="0" md="1"></v-col>
        <v-col cols="12" md="10" class="pa-4 pa-md-0">
          <div v-if="quests.length" class="d-flex justify-space-between align-center flex-wrap">
            <Quest class="mb-4 w-50-100" v-for="item in computedQuests" :item="item" :key="item.id" />
          </div>
        </v-col>
        <v-col cols="1" md="2"></v-col>
      </v-row>
    </v-main>
  </div>
</template>

<script>
import Header from '~/components/UI/Header'
import Quest from '~/components/Quest.vue'
export default {
  components: {
    Header,
    Quest,
  },
  data() {
    return {
      search: '',
      quests: []
    }
  },
  created() {
    this.$request({
      url: "/GenerateQuest/GetFilteredQuests",
      method: "POST",
      data: {}
    }).then((res) => {
      this.quests = res
    }).catch(err => {
      console.log(err, "err")
    })
  },
  computed: {
    computedQuests() {
      if (this.search) {
        return [...this.quests].filter((item) => item.title.toLowerCase().includes(this.search.toLowerCase()))
      } else {
        return this.quests
      }

    }
  }
}
</script>
