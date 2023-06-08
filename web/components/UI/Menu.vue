<template>
  <v-list>
    <v-divider />
    <v-list-item active-class="activeLink" to="/">
      <v-list-item-icon>
        <v-icon>mdi-home</v-icon>
      </v-list-item-icon>
      <v-list-item-title>Главная</v-list-item-title>
    </v-list-item>
    <v-list-item active-class="activeLink" to="/landing">
      <v-list-item-icon>
        <v-icon>mdi-information-outline</v-icon>
      </v-list-item-icon>
      <v-list-item-title>О нас </v-list-item-title>
    </v-list-item>
    <v-list-group prepend-icon="mdi-pencil">
      <template v-slot:activator>
        <v-list-item-title>Создать квест</v-list-item-title>
      </template>
      <v-list-item active-class="activeLink" to="/yours" class="ml-6">
        <v-list-item-icon>
          <v-icon small>mdi-format-list-bulleted-square</v-icon>
        </v-list-item-icon>
        <v-list-item-title>Ваши квесты</v-list-item-title>
      </v-list-item>
      <v-list-item @click="$store.commit('create/removeCurrentQuest')" active-class="activeLink" to="/create/new"
        class="ml-6">
        <v-list-item-icon>
          <v-icon small>mdi-loupe</v-icon>
        </v-list-item-icon>
        <v-list-item-title>Создать новый</v-list-item-title>
      </v-list-item>
    </v-list-group>
    <v-list-group prepend-icon="mdi-play">
      <template v-slot:activator>
        <v-list-item-title>Пройти квест</v-list-item-title>
      </template>
      <v-list-item nuxt active-class="activeLink" to="/" class="ml-6">
        <v-list-item-icon>
          <v-icon small>mdi-magnify</v-icon>
        </v-list-item-icon>
        <v-list-item-title>Найти</v-list-item-title>
      </v-list-item>
      <v-list-item nuxt active-class="activeLink" to="/lk" class="ml-6">
        <v-list-item-icon>
          <v-icon small>mdi-progress-question</v-icon>
        </v-list-item-icon>
        <v-list-item-title>Прогресс</v-list-item-title>
      </v-list-item>
    </v-list-group>
  </v-list>
</template>

<script>
export default {
  data() {
    return {
      group: null
    }
  }
}
</script>

