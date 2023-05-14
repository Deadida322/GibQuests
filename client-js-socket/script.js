document.addEventListener("DOMContentLoaded", () => {
  let buttonMove = document.querySelector("#move-quest");
  let buttonGet = document.querySelector("#get-quest");
  let buttonClose = document.querySelector("#close-quest");
  let myWorker = {};
  let startQuest;
  let stages = [];
  let stage = {};
  const userId = 2
  let reqQuest = {
    requestUserId: userId,
    id: 1,
  };
  buttonGet.addEventListener("click", async () => {
    let response = await fetch(
      "http://localhost:9007/ProcesssQuest/ConnectToQuest",
      {
        method: "POST",
        headers: {
          "Content-Type": "application/json;charset=utf-8",
        },
        body: JSON.stringify(reqQuest),
      }
    )
      .then((res) => res.json())
      .catch((err) => console.log(err));
    startQuest = response.data;
    myWorker = new Worker("webSocketWorker.js");
    myWorker.postMessage({ cmd: "start", data: startQuest.room });

    myWorker.onmessage = function (e) {
      let message = JSON.parse(e.data);
      console.log("Message received from worker", message);
      if(message.sucsess) {
        stage = message.stage
      }
      else {
        alert(message.error)
      }
    };

    console.log("startQuest ", startQuest);
    stages = startQuest.quest.stages;
    stages.sort(compareStages);
    stage = stages[0];
    // stage = {
    //   type: 5,
    //   order: 0,
    //   title: "Тести",
    //   questions: [
    //     {
    //       type: 1,
    //       title: "Сколько хош",
    //       order: 0,
    //       answers: ["адин"],
    //       rightAnswers: ["адин"]
    //     },
    //     {
    //       type: 2,
    //       title: "Сколько хош",
    //       order: 1,
    //       answers: ["адин", "два", "три"],
    //       rightAnswers: ["адин"]
    //     },
    //     {
    //       type: 3,
    //       title: "Сколько хош",
    //       order: 2,
    //       answers: ["адын", "Два", "Три"],
    //       rightAnswers: ["адин", "Два"]
    //     },
    //     {
    //       type: 4,
    //       title: "Сколько хош",
    //       order: 3,
    //       answers: ["Четыре", "Два", "Три"],
    //       rightAnswers: ["Четыре", "Два", "Три"]
    //     }
    //   ]
    // };
    // console.log(stages);
  });

  buttonMove.addEventListener("click", () => {
    myWorker.postMessage({
      cmd: "process",
      data: {
        stage: stage,
        userId: userId,
      },
    });
  });

  buttonClose.addEventListener("click", () => {
    myWorker.terminate();
  });
});

function compareStages(a, b) {
  if (a.order < b.order) return -1;
  else if (b.order < a.order) return 1;
  return 0;
}
