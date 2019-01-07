﻿const chatBox = document.querySelector('#chat-container');
const textToPrintDiv = document.querySelector('#text-print');
const sendMsgBtn = document.querySelector('#send-msg');
const friendsAndChatDiv = document.querySelector('#friends-and-chat');

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
                    scrollToBottom();
                });
            });
            userToChatWith = e.target.innerHTML;
          
            //const isScrolledToBottom = friendsAndChatDiv.scrollHeight - friendsAndChatDiv.clientHeight <= friendsAndChatDiv.scrollTop + 1;
            //console.log(isScrolledToBottom);
          

        });
    };
}; // starts when user clicks on a friend

connection.on('ReceiveMessage', renderMessage);
startChat();
waitForElement(".friends").then((element) => {});


input.addEventListener('keydown', e => {
    if (e.keyCode === 13) { submitMessage();}
});

button.addEventListener('click', () => {
    submitMessage();
});

function submitMessage() {
    let text = input.value;
    input.value = '';
    connection.invoke('SendPrivateMessage', userToChatWith, text);
};

function scrollToBottom() {
    const isScrolledToBottom = friendsAndChatDiv.scrollHeight - friendsAndChatDiv.clientHeight <= friendsAndChatDiv.scrollTop + 1;
    friendsAndChatDiv.scrollTop = friendsAndChatDiv.scrollHeight - friendsAndChatDiv.clientHeight;

};

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
    scrollToBottom();

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
    const dateTime = new Date(time);
    return `${dateTime.getHours()}:${dateTime.getMinutes()}`;
}



////////////////////////////////////////////////////
//////GROUP MESSAGE/////////////////////////////////
////////////////////////////////////////////////////

const inputNewGroup = document.querySelector('#new-group');
const newGroupBtn = document.querySelector('#new-group-btn');

const grouptContainer = document.querySelector('#group-container');



inputNewGroup.addEventListener('keydown', e => {

    if (e.keyCode === 13) {
        e.preventDefault();
        createNewGroup();
    }
});

newGroupBtn.addEventListener('click', (e) => {
    e.preventDefault();
    createNewGroup();
});

function createNewGroup() {
    const input = inputNewGroup.value;
    const specificGroup = document.createElement('div');
    specificGroup.classList.add('group-name');
    specificGroup.innerHTML = input;
    grouptContainer.appendChild(specificGroup);
    getElement(specificGroup);
  
   
}
const newDiv = document.createElement('div');


function getElement(el) {
    el.addEventListener('click', (ev) => {
        createGroupInviteToFriends(el);

    });
}

function createGroupInviteToFriends(el) {
    newDiv.innerHTML = '';
    const friendItem = document.querySelectorAll('.friends');
    for (var i = 0; i < friendItem.length; i++) {
        let checkbox = document.createElement("INPUT");
        let label = document.createElement("label");
        checkbox.setAttribute("type", "checkbox");
        checkbox.value = friendItem[i].innerHTML;
        label.appendChild(document.createTextNode(friendItem[i].innerHTML));
        newDiv.appendChild(label);
        newDiv.appendChild(checkbox);
    }

   
    const h1 = document.createElement('h3');
    h1.appendChild(document.createTextNode(el.innerHTML));
    newDiv.appendChild(h1);
    document.body.appendChild(newDiv);
}



function allowDrop(ev) {
    ev.preventDefault();
}

function drag(ev) {
    ev.dataTransfer.setData("Text", ev.target.id);
}

//function drop(ev) {
//    ev.preventDefault();
//    var data = ev.dataTransfer.getData("Text");
//    ev.target.appendChild(document.getElementById(data));

//}


