<template>
    <div>
        <Header :fixed="true" title='QuestGib' />
        <v-main class="mt-12">
            <v-img class="rb" height="500" height-md="600" gradient="to bottom,rgba(33,29,82,0.7), rgba(73,79,113,0.8)"
                :src="require(`~/assets/landing.jpg`)">
                <div class="image_container pt-12 justify-center d-flex align-center flex-column">
                    <img width="250px" :src="require('~/assets/logo.png')" alt="">
                    <h1 class="mt-6 mb-6 text-body-1 white--text">Приложение к вашему празднику</h1>
                    <div class="flex mt-4">
                        <v-btn color="white" disabled class="mr-4">
                            <v-icon>
                                mdi-download-circle</v-icon>
                            Скачать
                        </v-btn>
                        <v-btn class="white--text" to="/login" color="deep-purple">
                            Попробовать
                        </v-btn>
                    </div>
                </div>
            </v-img>
            <v-row>
                <v-col cols="0" md="2"></v-col>
                <v-col cols="12" md="8" class="pa-6 pa-md-0">
                    <div class="text-h5 mt-10 pt-4 pb-4 mb-10 text-center">Сервис <span
                            class="deep-purple--text">QuestsGib</span>
                        создан для вашего настроения и настроения
                        ваших
                        близких.
                        Приложение предоставляет доступ к созданию и проведению квестов и викторин совершенно разной
                        направленности: от подарка до образовательной программы</div>
                </v-col>
                <v-col cols="0" md="2"></v-col>
            </v-row>
            <v-row class="deep-purple lighten-5">
                <v-col cols="0" md="2"></v-col>
                <v-col cols="12" md="8" class="pa-6 pa-md-0">
                    <h2 class="text-h3 mt-8 mb-4 text-center">Широкий список этапов</h2>
                    <div class="d-flex justify-space-between flex-wrap">
                        <Stage class="mt-5 w-50-100" v-for="stage in stages" :stage="stage" />
                    </div>

                    <div class="d-flex justify-center mt-4 mb-4">
                        <v-btn href="https://vk.com" color="primary">
                            <v-icon class="mr-2" right dark>
                                mdi-playlist-plus
                            </v-icon>
                            Предложить идею
                        </v-btn>
                    </div>
                </v-col>
                <v-col cols="0" md="2"></v-col>
            </v-row>
            <v-row>
                <v-col cols="0" md="2"></v-col>
                <v-col cols="12" md="8" class="pa-6 pa-md-0">
                    <h2 class="text-h3 mt-8 mb-4 text-center">Возможность отследить квест</h2>
                    <div class="d-flex justify-space-between flex-wrap">
                        <Watcher class="mt-5 w-50-100" v-for="user in watch" :user="user" />
                    </div>

                    <div class="text-h5 mt-10 pt-4 pb-4 mb-10 text-center text--secondary">
                        А также удобный личный кабинет для просмотра своих созданных и пройденных квестов</div>
                    <div class="d-flex justify-center mt-4 mb-4">
                        <v-btn to="/register" color="primary">
                            <v-icon class="mr-2" right dark>
                                mdi-account-multiple-plus
                            </v-icon>
                            Попробовать
                        </v-btn>
                    </div>
                </v-col>
                <v-col cols="0" md="2"></v-col>
            </v-row>
            <v-row class="deep-purple lighten-5">
                <v-col cols="0" md="2"></v-col>
                <v-col cols="12" md="8" class="pa-6 pa-md-0">
                    <h2 class="text-h3 mt-8 mb-4 text-center">Следующие обновления</h2>

                    <div class="d-flex justify-space-between flex-wrap">
                        <Update class="mt-5 w-50-100" v-for="update in updates" :update="update" />
                    </div>
                    <div class="d-flex justify-center mt-4 mb-4">
                        <v-btn href="https://vk.com" color="primary">
                            <v-icon class="mr-2" right dark>
                                mdi-playlist-plus
                            </v-icon>
                            Предложить идею
                        </v-btn>
                    </div>
                </v-col>
                <v-col cols="0" md="2"></v-col>
            </v-row>
        </v-main>
    </div>
</template>

<script>
import Stage from '~/components/landing/Stage.vue';
import Watcher from '~/components/Watch/User.vue';
import Header from '~/components/UI/Header.vue';
import Update from '~/components/landing/Update.vue';
export default {
    components: {
        Header,
        Stage,
        Watcher,
        Update
    },
    data() {
        return {
            stages: [
                {
                    title: "Текстовый этап",
                    icon: 'mdi-text-box-edit-outline',
                    description: "Позволяет доносить до пользователя информацию, сообщать о том, что необходимо делать в слудеющем этапе в более развёрнутой форме, а также плести интриги и ломать третью стену",
                    status: "Доступно"
                },
                {
                    title: "QR-code этап",
                    icon: 'mdi-qrcode',
                    description: "Задайте кодовое слово, на основе которого будет построен QR-код. Пользователь должен отсканировать код, чтобы двинуться дальше. Отлично комбинируется с гео-этапом",
                    status: "Доступно"
                },
                {
                    title: "Видео этап",
                    icon: 'mdi-youtube',
                    description: "Наденьте маску Гая Фокса, снимите видео, выложите на ютуб и укажите ссылку на него. Ну или другие варианты использования видео. Позволяет казать видео с youtube в качестве этапа",
                    status: "Доступно"
                },
                {
                    title: "Тестовый этап",
                    icon: 'mdi-school-outline',
                    description: "Создайте свой собственный тест с разными типами вопросов: выбор ответа, выбор нескольких ответов, впишите слово, укажите порядок. Тестовые этапы помогут закрепить знания, полученные в ходе квеста",
                    status: "Доступно"
                },
                {
                    title: "Гео-этап",
                    icon: 'mdi-map-outline',
                    description: "Отметьте точку на карте, при достижении которой пользователь сможет перейти на следующий уровень. Укажите свою локацию, либо выберите её на карте тапом. Этап добавляет интерактивности в ваш квест",
                    status: "Доступно"
                },
                {
                    title: "Горячо-холодно",
                    icon: 'mdi-fire-circle',
                    description: "Отметьте точку на карте, указанная вами точка не будет отображена на карте, вместо этого пользователь будет получать указания, в верном ли направлении он движется",
                    status: "В разработке"
                },
                {
                    title: "Зона поиска",
                    icon: 'mdi-map-legend',
                    description: "Отметьте на карте зону поиска в которой пользователь вашего квеста должен что-то сделать. Отмеченная вами зона будет отображена у игрока на устройстве",
                    status: "В разработке"
                },
                {
                    title: "Sharing",
                    icon: 'mdi-share-all-outline',
                    description: "Пользователь должен распространить ссылку на максимальное количество других пользователей. Подходит для акционных и коммерчиских квестов, чтобы повысить спрос на ваш продукт",
                    status: "Скоро"
                }
            ],
            watch: [
                {
                    now: 2,
                    all: 20,
                    username: "davidal",
                    name: "Давид",
                    surname: "Алиев",
                },
                {
                    now: 5,
                    all: 15,
                    username: "valdes32",
                    name: "Владислав",
                    surname: "Араев",
                },

            ],
            updates: [
                {
                    title: "Ветвления",
                    icon: "mdi-file-tree",
                    description: "Возможность задать разные исходы событий в зависимости от выбора в этапах. Добавят вариативности для каждого квеста"
                },
                {
                    title: "Модерация этапов",
                    icon: "mdi-shield-crown",
                    description: "Не все этапы можно проверить в автоматическом режиме, некоторые этапы могут требовать дополнительной проверки организатором, например, проверка отправленного фото"
                },
                {
                    title: "Подсказки",
                    icon: "mdi-tooltip-edit",
                    description: "Возможность добавить подсказку к этапу и воспользоваться ей при прохождении"
                },
                {
                    title: "Мини-игры",
                    icon: "mdi-gamepad-circle-down",
                    description: "Конструктор аркадных мини-игр, прохождение которых станет плюсом в вашем квесте"
                }
            ]

        }
    },
    layout: 'nolog',
}

</script>

<style lang="scss">
.w-50-100 {
    width: 49%;

    @media screen and (max-width: 800px) {
        width: 100%;
    }
}

body {
    overflow-x: clip;
}

.image_container {
    height: 100%;
}

.rb {
    clip-path: ellipse(100% 52% at 49% 45%);
    background-color: #f3e5f5;

    @media screen and (max-width: 600px) {
        clip-path: none;
    }
}
</style>