
export default function ({ $axios, store }, inject) {
    const BASE_URL = "http://127.0.0.1:9003/api";

    const client = $axios.create({
        baseURL: BASE_URL,
    });
    client.interceptors.response.use(
        async (response) => {
            if(response.data.success){
                console.log(response, "Успешный ответ")
                return await Promise.resolve(response.data.data);
            } else{
                console.log(response, 'Не успешный ответ')
                return await Promise.reject(response.data.errors);
            }
        },
        async (error) => {
            const originalRequest = error.config;
            if (error.response?.status === 401 && !originalRequest._retry) {
                originalRequest._retry = true;
                return await store.dispatch("auth/tryToPair").then(async (res) => {
                    originalRequest.headers["Authorization"] =
                        "Bearer " + res.token;
                    return await client(originalRequest);
                })
              }
            
            return error;
        }
    );

    inject("request", client);
}
