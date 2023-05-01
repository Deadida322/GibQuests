import request from "../../request";
import { LOGIN_USER } from "../types";

export const setUser = (data) => {
    const info = { ...data, userName: data.login };

    return async (dispatch) => {
        try {
            const data = await request({
                url: "/Auth/Login",
                method: "POST",
                data: info,
            });
            return (user) => {
                dispatch({
                    type: LOGIN_USER,
                    payload: data,
                });
            };
        } catch (err) {
            return Promise.reject(err);
        }
    };
};
