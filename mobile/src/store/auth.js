import { makeAutoObservable, action, observable, runInAction } from "mobx";
import AsyncStorage from "@react-native-async-storage/async-storage";
import request from "../request";

export function Auth() {
    return makeAutoObservable(
        {
            loading: false,
            user: {},
            accessToken: "",
            tryToPair() {
                return new Promise(async (resolve, reject) => {
                    user = JSON.parse(await AsyncStorage.getItem("user"));
                    this.user = user;   
                    const info = {
                        token: user.token,
                        refreshToken: user.refreshToken,
                    };
                    request({
                        url: "/Auth/LoginByRefresh",
                        method: "POST",
                        data: info,
                       
                    })
                        .then((res) => {
                            this.user = res
                            this.accessToken = res.token
                            resolve(res)
                        })
                        .catch((err) => {
                            reject(err);
                        })
                });
            },
            login(data) {
                runInAction(() => {
                    this.loading = true;
                });
                const info = {
                    ...data,
                    userName: data.login,
                };
                return new Promise((resolve, reject) => {
                    request({
                        url: "/Auth/Login",
                        method: "POST",
                        data: info,
                    })
                        .then((res) => {
                            runInAction(() => {
                                this.loading = false;
                                this.user = res;
                                this.accessToken = res.token;
                                AsyncStorage.setItem(
                                    "user",
                                    JSON.stringify(this.user)
                                );
                                AsyncStorage.setItem(
                                    "refreshToken",
                                    JSON.stringify(this.user.refreshToken)
                                );
                            });
                            return resolve(res);
                        })
                        .catch((err) => {
                            runInAction(() => {
                                this.loading = false;
                            });
                            return reject(err);
                        });
                });
            },
        },
        {
            loading: observable,
            user: observable,
            login: action.bound,
        }
    );
}
export default Auth();
