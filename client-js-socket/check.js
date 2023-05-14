document.addEventListener("DOMContentLoaded", () => {
    let buttonCheck = document.querySelector("#check-quest");

    let questId = 1;
    buttonCheck.addEventListener("click", async () => {
        webSocket = new WebSocket(`ws://localhost:9007/check/${questId}`);
        webSocket.onopen = function (e) {
            webSocket.send(JSON.stringify("start"));
            console.log('start');
          };
        webSocket.onmessage = function (event) {
            console.log('событиые',event.data);
            //alert(`[message] Данные получены с сервера: ${event.data}`);
            };
      });
})