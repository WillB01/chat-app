const chatBox = document.querySelector('#chat-container');
const textToPrintDiv = document.querySelector('#text-print');


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

            userConnectionId = connectionId;

        });
};
////////////////////////

const clickHandlerFriendItem = () => {
    for (var i = 0; i < friendItem.length; i++) {
        friendItem[i].addEventListener('click', (e) => {
            textToPrintDiv.innerHTML = '';
            const value = e.target.innerHTML;
          
            let history = connection.invoke('GetHistory', value);
            history.then(h => {
                userHistory = h;
                console.log(h);
                const p = document.createElement("p");
                const text = document.createTextNode(`${userHistory}`);
                p.appendChild(text);
                textToPrintDiv.appendChild(p);
                userToChatWith = e.target.innerHTML;
              
                
            });
            
           
        });
    };
};


connection.on('ReceiveMessage', renderMessage);
startChat();
clickHandlerFriendItem();


sendMsgBtn.addEventListener('click', () => {
    let text = chatText.value;
    chatText.value = '';
    connection.invoke('SendPrivateMessage', userToChatWith, text);
   
});

function renderMessage(message, time) {
    console.log(`${message}`);

    const p = document.createElement("p");
    const text = document.createTextNode(`${time} - ${message}`);
    p.appendChild(text);
    textToPrintDiv.appendChild(p);


};
