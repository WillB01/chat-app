const chatBox = document.querySelector('#chat-container');
const textToPrintDiv = document.querySelector('#text-print');
const sendMsgBtn = document.querySelector('#send-msg');


const chatText = document.querySelector('#chat-text');

let userConnectionId = '';
let userToChatWith = '';
let userName = '';
let friendDataValue = '';
let divToPrint = '';

//Signalr stuff ////////////
const connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();

const startChat = () => {
    connection.start()
        .catch((err) => console.error(err.toString())
        )
        .then(() =>
            connection.invoke('GetUserName')
        )
        .then((name) => {
            userName = name;
        });
};

const button = document.createElement("button");
var buttonText = document.createTextNode("Send");
const input = document.createElement("input");
////////////////////////

const flattenMsgHistory = (history) => {
    const stageOne = [];
    for (var i = 0; i < history.length; i++) {
        stageOne.push({ [history[i].time]: [history[i].message, history[i].isLoggedin] });
    }
    return stageOne;
};

const clickHandlerFriendItem = (friendItem) => {
    for (var i = 0; i < friendItem.length; i++) {
        friendItem[i].addEventListener('click', (e) => {
            friendDataValue = e.target.dataset.friend;
            textToPrintDiv.innerHTML = '';

            const value = e.target.innerHTML;

            button.append(buttonText);
            button.id = 'send-msg';
            input.id = 'chat-text';
            chatBox.append(input);
            chatBox.append(button);

            textToPrintDiv.setAttribute('friend-chat', friendDataValue);

            let history = connection.invoke('GetHistory', value);
            history.then(result => {
                flattenMsgHistory(result).map((item, index) => {
                    const p = document.createElement("p");
                    const text = document.createTextNode(`${Object.keys(item)} - ${item[Object.keys(item)][0]}`);
                    if (item[Object.keys(item)][1]) {
                        p.classList.add('user-message-container');
                        p.setAttribute('data-user', friendDataValue);
                    }
                    p.appendChild(text);
                    console.log(divToPrint);
                    textToPrintDiv.appendChild(p);
                });
            });
            userToChatWith = e.target.innerHTML;
        });
    };
}; // starts when user clicks on a friend

connection.on('ReceiveMessage', renderMessage);
startChat();
window.addEventListener('load', () => {
    alert('');
    const friendItem = document.querySelectorAll('.friends');

    clickHandlerFriendItem(friendItem);
});


button.addEventListener('click', () => {
    let text = input.value;
    input.value = '';
    connection.invoke('SendPrivateMessage', userToChatWith, text);
});

function renderMessage(message, time, isLoggedin, fromFriend) {
    divToPrint = document.querySelectorAll(`[friend-chat=${fromFriend}]`)[0];
    var testP = document.createElement("p");

    testP.appendChild(document.createTextNode(`${time} - ${message}`));
    if (isLoggedin) {
        testP.classList.add('user-message-container');
    }

    if (divToPrint !== undefined) {
        divToPrint.appendChild(testP);
    }
};

async function connect(conn) {
    conn.start().catch(e => {
        sleep(5000);
        console.log("Reconnecting Socket");
        connect(conn);
    }
    )
}

connection.onclose(function (e) {
    connect(connection);
});

async function sleep(msec) {
    return new Promise(resolve => setTimeout(resolve, msec));
}