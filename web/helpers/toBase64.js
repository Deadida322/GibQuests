export default function toBase64(item){
    return new Promise(resolve=>{
        let reader = new FileReader();
        reader.readAsDataURL(item);
        reader.onload = function () {
            return resolve(reader.result)
        }
    })   
}