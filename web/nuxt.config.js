import colors from 'vuetify/es5/util/colors'
export default {
  basePath: ' ./',
  ssr: false,
  target: 'server',
  head: {
    title: 'GibQuests',
    htmlAttrs: {
      lang: 'en'
    },
    meta: [
      { charset: 'utf-8' },
      { name: 'viewport', content: 'width=device-width, initial-scale=1' },
      { hid: 'description', name: 'description', content: '' },
      { name: 'format-detection', content: 'telephone=no' },
      { "http-equiv": 'Content-Security-Policy', content: "upgrade-insecure-requests"}
    ],
    link: [
      { rel: 'icon', type: 'image/x-icon', href: '/favicon.ico' }
    ]
  },

  css: [
    '~/node_modules/leaflet/dist/leaflet.css',
    "~/static/style/main.css",
    "~/node_modules/vue-wysiwyg/dist/vueWysiwyg.css",
  ],

  plugins: [
    '~/plugins/wysiwyg.js',
    '~/plugins/request.js',
    '~/plugins/messages.js',
    { src: '~plugins/leaflet.js', ssr: false }
  ],

  components: false,

  buildModules: [
    '@nuxtjs/vuetify',
  ],

  modules: [
    '@nuxtjs/axios',
  ],

  axios: {
    baseURL: 'http://127.0.0.1:9003/',
  },

  vuetify: {
    customVariables: ['~/assets/variables.scss'],
    theme: {
      primary: colors.deepPurple.accent4,
      themes: {
        light: {
          primary: colors.deepPurple.accent4
        }
      }
    }
  },

  build: {
    publicPath: '',
    basePath: './',
  },
}
