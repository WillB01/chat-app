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
        let timeRemake = timeChanger(history[i].time);
        stageOne.push({ [timeRemake]: [history[i].message, history[i].isLoggedin] });
    }
    return stageOne;
};

const clickHandlerFriendItem = () => {
    const friendItem = document.querySelectorAll('.friends');
    friendListItemStyle(friendItem);
    
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
                    //const text = document.createTextNode(`${item[Object.keys(item)][0]}`);
                    const text = document.createTextNode(`${item[Object.keys(item)][0]} - ${Object.keys(item)} `); //the message!
                    let wrapperDiv = document.createElement('div');
                    if (item[Object.keys(item)][1]) {
                        wrapperDiv.classList.add('user-message-wrapper');
                        p.classList.add('user-message-container');
                        p.setAttribute('data-user', friendDataValue);
                    }
                    if (!item[Object.keys(item)][1]) {
                        wrapperDiv.classList.add('other-message-wrapper');
                    }
                    p.appendChild(text);
                    wrapperDiv.appendChild(p);
                   

                    textToPrintDiv.appendChild(wrapperDiv);
                });
            });
            userToChatWith = e.target.innerHTML;
        });
    };
}; // starts when user clicks on a friend

connection.on('ReceiveMessage', renderMessage);
startChat();
waitForElement(".friends").then((element) => {});


button.addEventListener('click', () => {
    let text = input.value;
    input.value = '';
    connection.invoke('SendPrivateMessage', userToChatWith, text);
});

function renderMessage(message, time, isLoggedin, fromFriend) {
    divToPrint = document.querySelectorAll(`[friend-chat=${fromFriend}]`)[0];
    let wrapperDiv = document.createElement('div');
    let remakeTime = timeChanger(time);

    let testP = document.createElement("p");
    testP.appendChild(document.createTextNode(`${message} - ${remakeTime} `));
    wrapperDiv.appendChild(testP);
    
    if (isLoggedin) {
        testP.classList.add('user-message-container');
        wrapperDiv.classList.add('user-message-wrapper');
    }

    if (!isLoggedin) {
        wrapperDiv.classList.add('other-message-wrapper');
    }

    if (divToPrint !== undefined) {
        divToPrint.appendChild(wrapperDiv);
    }
};

async function connect(conn) {
    conn.start().catch(e => {
        sleep(5000);
        console.log("Reconnecting Socket");
        connect(conn);
    }
    );
}

connection.onclose(function (e) {
    connect(connection);
});

async function sleep(msec) {
    return new Promise(resolve => setTimeout(resolve, msec));
}

function waitForElement(selector) {
    return new Promise(function (resolve, reject) {
        var element = document.querySelector(selector);

        if (element) {
            resolve(element);
            return;
        }

        var observer = new MutationObserver(function (mutations) {
            mutations.forEach(function (mutation) {
                var nodes = Array.from(mutation.addedNodes);
                for (var node of nodes) {
                    if (node.matches && node.matches(selector)) {
                        observer.disconnect();
                        resolve(node);
                        return;
                    }
                };
            });
        });

        observer.observe(document.documentElement, { childList: true, subtree: true });
    });
} // checks if element is created.

function friendListItemStyle(el) {
    el.forEach(item => {
        item.setAttribute('style', 'cursor: pointer');
    });
}

function timeChanger(time) {
    let timeRemake = time.substring(time.indexOf("T") + 1);
    return timeRemake = timeRemake.substring(0, timeRemake.indexOf(".") + 0);
}