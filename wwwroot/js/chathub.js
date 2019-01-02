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
                    const text = document.createTextNode(`${Object.keys(item)} - ${item[Object.keys(item)][0]}`);
                    if (item[Object.keys(item)][1]) {
                        p.classList.add('user-message-container');
                        p.setAttribute('data-user', friendDataValue);
                    }
                    p.appendChild(text);
                   
                   
                    textToPrintDiv.appendChild(p);
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
    //const css = 'div p:hover{ background-color: blue }';
    //const style = document.createElement('style');

    //style.styleSheet
    //    ? style.styleSheet.cssText = css
    //    : style.appendChild(document.createTextNode(css));
    
    el.forEach(item => {
        item.setAttribute('style', 'cursor: pointer');
        //item.appendChild(style);
    });
}