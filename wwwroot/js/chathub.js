
const chatBox = document.querySelector('#chat-container');
const connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();


connection.on("ReceiveMessage", function (user, time, message) {
    console.log(message);
    const encodedMsg = message;
    let li = document.createElement("li");
   
    li.textContent = encodedMsg;
    chatBox.appendChild(li);
});

connection.start().catch(function (err) {
    return console.error(err.toString());
});

//document.getElementById("sendButton").addEventListener("click", function (event) {
//    const user = document.getElementById("userInput").value;
//    const message = document.getElementById("messageInput").value;
//    connection.invoke("SendMessage", user, message).catch(function (err) {
//        return console.error(err.toString());
//    });
//    event.preventDefault();
//});