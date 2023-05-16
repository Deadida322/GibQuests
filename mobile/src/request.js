import axios from "axios";
const BASE_URL = "http://192.168.43.173:9003/api";
import auth from "./store/auth";

const client = axios.create({
    baseURL: BASE_URL,
});

client.interceptors.response.use(
    (response) => {
        return response;
    },
    (error) => {
        const originalRequest = error.config;
        if (error.response?.status === 401 && !originalRequest._retry) {
            originalRequest._retry = true;
            return auth.tryToPair()
                .then((res)=>{
                    originalRequest.headers['Authorization'] = 'Bearer ' + res.token
                    return client(originalRequest)
                })
          }
        
        return error;
    }
);
const request = async function (options) {
    const onSuccess = function (response) {
        if (response?.success) {
            return Promise.resolve(response.data);
        } else {
            return Promise.reject(response?.errors);
        }
    };

    const onError = function (error) {
        return Promise.reject(error.message || error);
    };

    try {
        const response = await client(options);
        console.log(response, "response")
        return onSuccess(response.data || response);
    } catch (error) {
        console.log(error, "error")
        return onError(error);
    }
};

export default request;
export const axiosInstance = client;
