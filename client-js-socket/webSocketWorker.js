let webSocket = {};

self.addEventListener(
  "message",
  function (e) {
    var data = e.data;
    console.log("data in worker", data);
    switch (data.cmd) {
      case "start":
        webSocket = new WebSocket(`wss://localhost:44325/room/${data.data}`);
        webSocket.onmessage = function (event) {
            console.log('событиые',event.data);
            //alert(`[message] Данные получены с сервера: ${event.data}`);
            postMessage(event.data);
          };
        break;
      default:
        webSocket.send(JSON.stringify(data.data));
    }
  },
  false
);

webSocket.onopen = function (e) {
  // alert("[open] Соединение установлено");
  // alert("Отправляем данные на сервер");
  //webSocket.send("Меня зовут Джон");
};



webSocket.onclose = function (event) {
  if (event.wasClean) {
    alert(
      `[close] Соединение закрыто чисто, код=${event.code} причина=${event.reason}`
    );
  } else {
    // например, сервер убил процесс или сеть недоступна
    // обычно в этом случае event.code 1006
    alert("[close] Соединение прервано");
  }
};

webSocket.onerror = function (error) {
  alert(`[error]`);
};
