<template>
    <div v-on="$listeners" class="plus d-flex justify-center align-center">
        <v-menu
            bottom
            offset-x
            transition="slide-y-transition"
        >
            <template v-slot:activator="{ on, attrs }">
                <v-btn
                    color="success lighten-2"
                    small
                    nuxt
                    v-on="on"
                    v-bind="$attrs"
                    class="plus__btn"
                    fab
                >
                    <v-icon>
                        mdi-plus
                    </v-icon>
                </v-btn>
            </template>
            <slot></slot>
        </v-menu>
    </div>
</template>

<style lang="scss" scoped>
    .plus{
        width: 100%;
        min-height: 200px;
        margin-bottom: 2%;
        background-color: rgba(97, 97, 97, 0.049);
        background-image: url("data:image/svg+xml,%3csvg width='100%25' height='100%25' xmlns='http://www.w3.org/2000/svg'%3e%3crect width='100%25' height='100%25' fill='none' stroke='%23A5A5A5FF' stroke-width='2' stroke-dasharray='6%2c 14' stroke-dashoffset='3' stroke-linecap='square'/%3e%3c/svg%3e");
        &:hover {
            background-color: rgba(97, 97, 97, 0.069);

            cursor: pointer;
            .plus__btn{
                transform: scale(1.2)
            }
        }
    }
</style>

