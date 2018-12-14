const chatBox = document.querySelector('#chat-container');
const sendMsgBtn = document.querySelector('#send-msg');
const friendItem = document.querySelectorAll('.friends');
const chatText = document.querySelector('#chat-text');
let userConnectionId = '';
let userHistory = '';
let userToChatWith = '';

//Signalr stuff ////////////
const connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();


const startChat = () => {
    connection.start()
        .catch((err) => console.error(err.toString())
        )
        .then(() =>
            connection.invoke('getConnectionId')
        )
        .then((connectionId) => {
            let history = connection.invoke('GetHistory', "will");
            history.then(h => {
                userHistory = h;
                userConnectionId = connectionId;
                console.log(userHistory);
                console.log(userConnectionId);
            });
        })
};
////////////////////////

const clickHandlerFriendItem = () => {
    for (var i = 0; i < friendItem.length; i++) {
        friendItem[i].addEventListener('click', (e) => {
            const p = document.createElement("p");
            const text = document.createTextNode(`${userHistory}`);
            p.appendChild(text);
            chatBox.appendChild(p);
            userToChatWith = e.target.innerHTML;
            console.log(e.target.innerHTML);
           
        });
    };
};







connection.on('ReceiveMessage', renderMessage);
startChat();
clickHandlerFriendItem();

//connection.on("ReceiveMessage", function (user, message) {
//    console.log(message);
//    console.log(sendMsgBtn);
//    console.log(user);
    

//    let li = document.createElement("li");

//    li.textContent = user;
//    chatBox.appendChild(li);
//    renderMessage(user);
   
//});




sendMsgBtn.addEventListener('click', () => {
    let text = chatText.value;

    connection.invoke('SendPrivateMessage', userToChatWith, text);
   
});

function renderMessage(message, time) {
    console.log(`${message}`);
    const p = document.createElement("p");
    const text = document.createTextNode(`${time} - ${message}`);
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