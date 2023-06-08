<template>
    <iframe width="100%" height="300" :src="computedSrc" title="YouTube video player" frameborder="0"
        allow="accelerometer; autoplay; clipboard-write; encrypted-media; gyroscope; picture-in-picture"
        allowfullscreen></iframe>
</template>

<script>
export default {
    mounted() {
        window.focus()
        let ctx = this
        window.onblur = function () {
            var monitor = setInterval(function () {
                var elem = document.activeElement;
                if (elem && elem.tagName == 'IFRAME') {
                    setTimeout(() => { ctx.$emit('stageComplete') }, 7000)
                    clearInterval(monitor)
                }
            }, 100)
        }
    },
    props: {
        stage: {
            type: Object,
            required: true
        }
    },
    destroyed() {
        window.onblur = function () { }
    },
    computed: {
        computedSrc() {
            let viewCode = ''
            if (this.url.includes('youtube.com')) {
                viewCode = this.stage.url.split('=')[1]
            }
            else {
                let result = this.stage.url.split('/')

                viewCode = result[result.length - 1]
                console.log(viewCode)

            }
            return `https://www.youtube.com/embed/${viewCode}`
        }
    }
}
</script>
