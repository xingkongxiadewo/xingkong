import axios from "axios"
import { ElMessage } from "element-plus"
import router from "../router/index"

axios.defaults.baseURL = "http://localhost:5291";

axios.defaults.headers.post["Content-Type"] = "application/json;charset=UTF-8"

axios.defaults.timeout = 500000

axios.interceptors.request.use(
    config => {
        const token = window.sessionStorage.getItem('auth_access_token')

        if (token)
            config.headers["authorization"] = token

        return config
    },
    error => {
        return Promise.reject(error)
    }
)

axios.interceptors.response.use(
    response => {
        if (response.data.code == 3 || response.data.code == 4) {
            ElMessage.error(response.data.message)
            return router.push('/login')
        }
        else if (response.data.code != 1){
            return ElMessage.error({ message: response.data.message, type: "error" });
        }    
        else if (response.status == 200 && response.data.code == 1) {
            return Promise.resolve(response);
        } else {
            return Promise.reject(response);
        }
    },
    error => {
        ElMessage.error(`异常请求:${JSON.stringify(error.message)}`)
    }
);
export default {
    post(url, data) {
        return new Promise((resolve, reject) => {

            axios({
                method: 'post',
                url,
                data: data,
            })
                .then(res => {
                    resolve(res.data)
                })
                .catch(err => {
                    reject(err)
                });
        })
    },

    get(url, data) {
        return new Promise((resolve, reject) => {
            axios({
                method: 'get',
                url,
                params: data,
            })
                .then(res => {
                    resolve(res.data)
                })
                .catch(err => {
                    reject(err)
                })
        })
    }
};
