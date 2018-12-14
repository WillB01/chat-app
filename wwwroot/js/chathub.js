const chatBox = document.querySelector('#chat-container');
const sendMsgBtn = document.querySelector('#send-msg');
const friendItem = document.querySelectorAll('.friends');
const chatText = document.querySelector('#chat-text');

const connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();
let userToChatWith = '';

const clickHandlerFriendItem = () => {
    for (var i = 0; i < friendItem.length; i++) {
        friendItem[i].addEventListener('click', (e) => {
            userToChatWith = e.target.innerHTML;
           
            console.log(e.target.className);
        });
    };
};


clickHandlerFriendItem();


connection.on('ReceiveMessage', renderMessage);

//connection.on("ReceiveMessage", function (user, message) {
//    console.log(message);
//    console.log(sendMsgBtn);
//    console.log(user);
    

//    let li = document.createElement("li");

//    li.textContent = user;
//    chatBox.appendChild(li);
//    renderMessage(user);
   
//});

connection.start()
    .catch((err) => console.error(err.toString())
    )
    .then(() => connection.invoke('getConnectionId'))
    .then((connectionId) => {
        console.log('conectionId' + connectionId);
    });

sendMsgBtn.addEventListener('click', () => {
    let text = chatText.value;
    connection.invoke('SendPrivateMessage', userToChatWith, text);
   
});

function renderMessage(message, time) {
    console.log(`${message}`);
    const chatContainer = document.querySelector('#chat-box');
    const p = document.createElement("p");
    const text = document.createTextNode(`${name} - ${message}`);
    p.appendChild(text);
    chatBox.appendChild(p);


};

//function send() {
//    connection.invoke('SendPrivateMessage', userToChatWith, 'text');
//}


//document.getElementById("sendButton").addEventListener("click", function (event) {
//    const user = document.getElementById("userInput").value;
//    const message = document.getElementById("messageInput").value;
//    connection.invoke("SendMessage", user, message).catch(function (err) {
//        return console.error(err.toString());
//    });
//    event.preventDefault();
//});