import { getItem, setItem } from "~/helpers/persistanceStorage";

export const state = () => {
    return {
        loading: false,
        user: {},
        accessToken: "",
        isLoggedIn: false,
    };
};
export const actions = {
    login({ commit }, data) {
        commit("setLoading", true);
        return new Promise((resolve, reject) => {
            this.$request({
                url: "/Auth/Login",
                method: "POST",
                data: data,
            })
                .then((res) => {
                    commit("setLoading", false);
                    commit("setUser", res);
                    commit("setIsLoggedIn", true);
                    commit("setAccessToken", res.refreshToken);
                    return resolve(res);
                })
                .catch((err) => {
                    commit("setLoading", false);
                    return reject(err);
                });
        });
    },

    async tryToPair({ commit }) {
        const user = getItem("user");

        if(!user){
            this.$router.push("/login");
            return
        }
        commit("setLoading", true);
        console.log(user)
        const info = {
            token: user.token,
            refreshToken: user.refreshToken,
        };
        return await new Promise((resolve, reject) => {
            this.$request({
                url: "/Auth/LoginByRefresh",
                method: "POST",
                data: info,
            })
                .then(async (res) => {
                    if (res.length) {
                        return reject(res);
                    } else {
                        commit("setLoading", false);
                        commit("setUser", res);
                        commit("setAccessToken", res.token);
                        commit("setIsLoggedIn", true);
                        return resolve(res);
                    }
                })
                .catch(async (err) => {
                    this.$router.push("/login");
                    setItem("user", null)
                    setItem("token", null)
                    commit("setLoading", false);
                    return reject(err);
                });
        });
    },
};

export const mutations = {
    setUser(state, user) {
        setItem("user", user);
        state.user = user;
    },
    setLoading(state, loading) {
        state.loading = loading;
    },
    setAccessToken(state, token) {
        setItem("refreshToken", token);
        state.token = token;
    },
    setIsLoggedIn(state, val) {
        state.isLoggedIn = val;
    },
    logout(state) {
        state.isLoggedIn = false;
        state.user = {};
        state.accessToken = false;
        setItem('user', false);
        this.$router.push("/login")
    },
};
