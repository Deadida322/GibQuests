import Swal from "sweetalert2"

export default function (ctx, inject) {
    const showError = (msg="Произошла ошибка при выполнении запроса")=>{
        Swal.fire({
            title: 'Ошибка!',
            text: msg,
            icon: 'error',
        })
    }
    const showSuccess = (msg="Запрос выполнен успешно")=>{
        Swal.fire({
            title: 'Отлично!',
            text: msg,
            icon: 'success',
        })
    }
    inject('showError', showError)
    inject('showSuccess', showSuccess)
}