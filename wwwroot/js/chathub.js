const chatBox = document.querySelector('#chat-container');
const textToPrintDiv = document.querySelector('#text-print');
const sendMsgBtn = document.querySelector('#send-msg');
const friendsAndChatDiv = document.querySelector('#friends-and-chat');

const chatText = document.querySelector('#chat-text');

let userConnectionId = '';
let userToChatWith = '';
let userName = '';
let friendDataValue = '';
let divToPrint = '';

let groupToChatWith = '';

//Signalr stuff ////////////
const connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();

const startChat = () => {
    connection.start()
        .catch((err) => console.error(err.toString())
        )
        .then(() => {
            getGroups();
            return connection.invoke('GetUserName');
        })
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
            button.setAttribute('data', 'friend');
            input.id = 'chat-text';
            input.setAttribute('data', 'friend');
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
        });
    };
}; // starts when user clicks on a friend

connection.on('ReceiveMessage', renderMessage);
startChat();
waitForElement(".friends").then((element) => {});


input.addEventListener('keydown', e => {
    if (e.keyCode === 13) {
        if (e.target.attributes.data.value === 'friend') {
            submitMessage();
        } else {
            submitGroupMessege();
        }
    }
});

button.addEventListener('click', (e) => {
    if (e.target.attributes.data.value === 'friend') {
        submitMessage();
    } else {
        submitGroupMessege();
    }
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

connection.on('ReceiveGroupInvite', renderGroupInvite);
connection.on('GroupReceiveMessage', renderGroupMessage);


const btnYes = document.createElement('button');
const btnNo = document.createElement('button');
const div = document.createElement('div');
function renderGroupInvite(groupName) {
    div.innerHTML = '';
    groupName = groupName;
    const h3 = document.createElement('h3');
    h3.innerHTML = groupName;
    btnYes.innerHTML = 'join';
    btnNo.innerHTML = 'decline';
    div.classList.add('modal-group-invite');
    div.appendChild(h3);
    div.appendChild(btnYes);
    div.appendChild(btnNo);
    document.body.appendChild(div);

    btnYes.addEventListener('click', () => {
        div.remove();
        createNewGroup(groupName);
        connection.invoke('AddMemberToGroupDb', groupName);
        groupName = '';
    });




}

const inputNewGroup = document.querySelector('#new-group');
const newGroupBtn = document.querySelector('#new-group-btn');

const grouptContainer = document.querySelector('#group-container');



inputNewGroup.addEventListener('keydown', e => {

    if (e.keyCode === 13) {
        e.preventDefault();
        connection.invoke('SaveGroupDb', inputNewGroup.value);
        createNewGroup();
    }
});

newGroupBtn.addEventListener('click', (e) => {
    e.preventDefault();
    connection.invoke('SaveGroupDb', inputNewGroup.value);
    createNewGroup();
});

function createNewGroup(groupName) {
    const input = inputNewGroup.value;
    inputNewGroup.value = '';
    const specificGroup = document.createElement('div');
    specificGroup.classList.add('group-name');
    if (!groupName) {
        if (input) {
            let groupNameClassName = input.replace(/\s/g, '');
            specificGroup.classList.add(`${groupNameClassName}`);
        }
            
        specificGroup.innerHTML = input;
    }
    if (groupName) { specificGroup.innerHTML = groupName; }
    getElement(specificGroup, groupName);
    grouptContainer.appendChild(specificGroup);
    groupName = '';
}

function printHistoryGroups(groups) {
    console.log(groups);
    groups.map(item => {
        let specificGroup = document.createElement('div');
        specificGroup.classList.add('group-name');
        let groupNameClassName = classRegex(item.groupName);
        specificGroup.classList.add(`${groupNameClassName}`);
        specificGroup.innerHTML = item.groupName;
        grouptContainer.appendChild(specificGroup);
        getElement(specificGroup, item.groupName);
    });

   
}

const newDiv = document.createElement('div');


function getElement(el, groupName) {
    el.addEventListener('click', (e) => {
        if (!e.target.attributes.data && !groupName) {
            createGroupInviteToFriends(el);
        }
        else {
            var groupN = e.target.innerHTML;
            groupToChatWith = groupN;
            connection.invoke('AddToGroup', groupN);
            openGroupChat(groupN);
        }

    });
}

function createGroupInviteToFriends(el) {
    newDiv.innerHTML = '';
    newDiv.classList.add('group-chat-invite');
    const h1 = document.createElement('h3');
    const myBtn = document.createElement('button');
    myBtn.appendChild(document.createTextNode('Add'));
    myBtn.classList.add('send-to-friends-group-chat');
    h1.appendChild(document.createTextNode(`Add friends to group - ${el.innerHTML}`));
    newDiv.appendChild(h1);
    newDiv.appendChild(myBtn);
    const friendItem = document.querySelectorAll('.friends');
    for (var i = 0; i < friendItem.length; i++) {
        let checkbox = document.createElement("INPUT");
        let label = document.createElement("label");
        checkbox.setAttribute("type", "checkbox");
        checkbox.classList.add('friend-add-group-chat');
        checkbox.value = friendItem[i].innerHTML;
        label.appendChild(document.createTextNode(friendItem[i].innerHTML));
        newDiv.appendChild(label);
        newDiv.appendChild(checkbox);
    }


    
    document.body.appendChild(newDiv);

    getFriendsToStartGroup(el.innerHTML);

}

function getFriendsToStartGroup(groupName) {
    let friendsToSend = [];
    const groupChatInvite = document.querySelectorAll('.friend-add-group-chat');

    for (var i = 0; i < groupChatInvite.length; i++) {
        
        groupChatInvite[i].addEventListener('change', (e) => {
            if (e.target.checked) {
                friendsToSend.push(e.target.value);
            } else {
                friendsToSend = friendsToSend.filter(item => item !== e.target.value);
            }

        });
    }

    const sendBtn = document.querySelector('.send-to-friends-group-chat');
    sendBtn.addEventListener('click', (e) => {
        const group = document.querySelector(`.${groupName.replace(/\s/g, '')}`);
        const groupChatInviteChecks = document.querySelector('.group-chat-invite');
        group.setAttribute('data', 'sent');
        connection.invoke('SendInviteToJoinGroup', groupName, friendsToSend);
        setTimeout(() => { groupChatInviteChecks.remove(); }, 1000);         
        
    });
       
}


function openGroupChat(group) {

    //const button = document.createElement("button");
    //const buttonText = document.createTextNode("Send");
    //const input = document.createElement("input");

    textToPrintDiv.innerHTML = '';
    button.append(buttonText);
    button.id = 'send-msg';
    button.setAttribute('data', 'group');
    input.id = 'chat-text';
    input.setAttribute('data', 'group');
    chatBox.append(input);
    chatBox.append(button);

    textToPrintDiv.setAttribute('group-chat', classRegex(group));

    let wrapperDiv = document.createElement('div');





    //let history = connection.invoke('GetGroupHistory', group);
    //history.then(result => {
    //    flattenMsgHistory(result).map((item, index) => {
    //const p = document.createElement("p");
    //const text = document.createTextNode(`${item[Object.keys(item)][0]}`);
    //const text1 = document.createTextNode(`${item[Object.keys(item)][0]} - ${Object.keys(item)} `); //the message!
    //let wrapperDiv = document.createElement('div');
    //if (item[Object.keys(item)][1]) {
    //    wrapperDiv.classList.add('user-message-wrapper');
    //    p.classList.add('user-message-container');
    //    p.setAttribute('data-user', friendDataValue);
    //}
    //if (!item[Object.keys(item)][1]) {
    wrapperDiv.classList.add('other-message-wrapper');
    //}
    p.appendChild(text1);
    wrapperDiv.appendChild(p);


    textToPrintDiv.appendChild(wrapperDiv);
    scrollToBottom();
    //});
    //});
    groupToChatWith = group;
    //    });

    //});
};

function submitGroupMessege() {
    let text = input.value;
    input.value = '';
    connection.invoke('SendGroupMessage', groupToChatWith, text);
}
        

function renderGroupMessage(message, fromUser, time, group) {
    const userName = document.querySelector('.centered-text').innerHTML;
    let isLoggedin = fromUser === userName;

    divToPrint = document.querySelectorAll(`[group-chat=${classRegex(group)}]`)[0];
    let wrapperDiv = document.createElement('div');
    let remakeTime = timeChanger(time);

    let testP = document.createElement("p");
    testP.appendChild(document.createTextNode(`${fromUser} - ${message} - ${remakeTime} `));
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

function classRegex(myStr) {
    return myStr.replace(/\s/g, '');
}


function getGroups() {
    connection.invoke('GetUsersGroupsAsync')
        .then(res => printHistoryGroups(res));
}