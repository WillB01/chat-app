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



/////////////////////////////////////////////////////////////////////////////////////////////////
//////GROUP MESSAGE//////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////////////////////////////

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
        printNewGroup(false, groupName);
        connection.invoke('AddMemberToGroupDb', groupName);
        groupName = '';
    });
}

const inputNewGroup = document.querySelector('#new-group');
const newGroupBtn = document.querySelector('#new-group-btn');
const grouptContainer = document.querySelector('#group-container');


inputNewGroup.addEventListener('keydown', e => {
    const groupName = inputNewGroup.value.trim();
    if (e.keyCode === 13) {
        e.preventDefault();
        if (!isEmptyOrSpaces(groupName)) {
            printNewGroup(true, null);
        }
    }
});

newGroupBtn.addEventListener('click', (e) => {
    const groupName = inputNewGroup.value.trim();
    e.preventDefault();
    if (!isEmptyOrSpaces(groupName)) {
        printNewGroup(true, null);
    }
});

function printNewGroup(willCreateNewGroup, groupName) {
    const specificGroup = document.createElement('div');
    willCreateNewGroup
        ? createNewGroup(specificGroup)
        : addGroupFromInvite(groupName, specificGroup);
    getElement(specificGroup);
    grouptContainer.appendChild(specificGroup);
    groupName = '';
}

function createGroupIdAttr(originalGroupName) {
    const onlyInput = removeGuid(originalGroupName);
    const querySelectorFormat = classRegex(onlyInput);
    const guid = getGuid(originalGroupName);
    return `${querySelectorFormat}${guid}`;

}

function createNewGroup(element) {
    let groupName = inputNewGroup.value.trim();
    inputNewGroup.value = '';   
    connection.invoke('CreateGroupName', groupName)
        .then(newName => {
            groupName = newName;
            connection.invoke('SaveGroupDb', groupName);
            element.classList.add('group-name');
            if (groupName) {
                console.log(groupName);
                console.log(createGroupIdAttr(groupName));
                element.setAttribute('groupId', createGroupIdAttr(groupName));
                element.setAttribute('groupIdToSend', groupName);
                element.setAttribute('sendGroupInvite', true);
                //let groupNameClassName = classRegex(groupName);
                //element.classList.add(`${groupNameClassName}`);
            }

            element.innerHTML = removeGuid(groupName);

        });
}

function addGroupFromInvite(groupName, element) {
    if (groupName) {
        element.setAttribute('sendGroupInvite', false);
        element.setAttribute('groupId', createGroupIdAttr(groupName));
        element.setAttribute('groupIdToSend', groupName);
        element.classList.add('group-name');
        element.innerHTML = removeGuid(groupName);
    }
}

function printHistoryGroups(groups) {
    groups.map(item => {
        let specificGroup = document.createElement('div');
        specificGroup.classList.add('group-name');
        specificGroup.setAttribute('groupId', createGroupIdAttr(item.groupName));
        specificGroup.setAttribute('groupIdToSend', item.groupName);
        specificGroup.setAttribute('sendGroupInvite', false);

        //let groupNameClassName = classRegex(item.groupName);
        //specificGroup.classList.add(`${groupNameClassName}`);
        specificGroup.innerHTML = removeGuid(item.groupName);
        grouptContainer.appendChild(specificGroup);
        getElement(specificGroup);
    });

   
}

const newDiv = document.createElement('div');


function getElement(el) {
    el.addEventListener('click', (e) => {
        if (e.target.attributes.sendgroupinvite.value === "true") {
        createGroupInviteToFriends(el);
    }
    if (e.target.attributes.sendGroupInvite.value === "false") {
        const groupName = e.target.attributes.groupIdToSend.value;
        groupToChatWith = groupName;
        connection.invoke('AddToGroup', groupName);
        openGroupChat(groupName);
    }
})

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
    

    getFriendsToStartGroup(el.attributes.groupIdToSend.value);

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
        console.log(groupName);
        console.log(createGroupIdAttr(groupName));

        const group = document.querySelectorAll(`[groupId=${createGroupIdAttr(groupName)}]`)[0];
        const groupChatInviteChecks = document.querySelector('.group-chat-invite');
        group.setAttribute('data', 'sent');
        group.setAttribute('sendGroupInvite', false);
        group.setAttribute('groupIdToSend', groupName);
        connection.invoke('SendInviteToJoinGroup', groupName, friendsToSend);
        setTimeout(() => { groupChatInviteChecks.remove(); }, 1000);         
        
    });
       
}


function flattenGroupMsgHistory(history) {
    const stageOne = [];
    for (var i = 0; i < history.length; i++) {
        let timeRemake = timeChanger(history[i].time);
        stageOne.push({ [timeRemake]: [history[i].message, history[i].isLoggedIn, history[i].name]});
    }
    return stageOne;
};


function openGroupChat(group) {
    textToPrintDiv.innerHTML = '';
    button.append(buttonText);
    button.id = 'send-msg';
    button.setAttribute('data', 'group');
    input.id = 'chat-text';
    input.setAttribute('data', 'group');
    chatBox.append(input);
    chatBox.append(button);

    textToPrintDiv.setAttribute('chat-to-print', createGroupIdAttr(group));


    let history = connection.invoke('GetGroupChatHistoryAsync', group);
    history.then(result => {
        console.log(result);
        flattenGroupMsgHistory(result).map((item, index) => {
            const p = document.createElement("p");
            const text = document.createTextNode(`${item[Object.keys(item)][2]} - ${item[Object.keys(item)][0]} - ${Object.keys(item)} `); //the message!
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
    groupToChatWith = group;

};

function submitGroupMessege() {
    let text = input.value;
    input.value = '';
    connection.invoke('SendGroupMessage', groupToChatWith, text);
}
        

function renderGroupMessage(message, fromUser, time, group) {
    

    const userName = document.querySelector('.centered-text').innerHTML;
    let isLoggedin = fromUser === userName;

    divToPrint = document.querySelectorAll(`[chat-to-print=${createGroupIdAttr(group)}]`)[0];
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

function isEmptyOrSpaces(str) {
    return str === null || str.match(/^ *$/) !== null;
}

function classRegex(myStr) {
    return myStr.replace(/\s/g, '');
}


function getGroups() {
    connection.invoke('GetUsersGroupsAsync')
        .then(res => printHistoryGroups(res));
}

String.prototype.removeWord = function (searchWord) {
    var str = this;
    console.log(str);
    var n = str.search(searchWord);
    while (str.search(searchWord) > -1) {
        n = str.search(searchWord);
        str = str.substring(0, n) + str.substring(n + searchWord.length, str.length);
    }
    return str;
}

function removeGuid(groupName) {
    const regex = "(\{){0,1}[0-9a-fA-F]{8}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{12}(\}){0,1}";
    const gp = groupName;
    const guidMatch = groupName.match(regex);
    const guid = guidMatch[0].toString();
    const groupNameWithoutGuid = gp.replace(guid, '');
    return groupNameWithoutGuid.trim();
};

function getGuid(groupName) {
    const regex = "(\{){0,1}[0-9a-fA-F]{8}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{12}(\}){0,1}";
    const guidMatch = groupName.match(regex);
    return guidMatch[0].toString();

}

