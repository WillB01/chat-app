const friendDiv = document.querySelector('#friend');
const friendRequestResultDiv = document.querySelector('#friend-request-result');
const friendsContainer = document.querySelector('#friends-container');
const friendListContainer = document.querySelector('#friend-item');
const friendRequestBadge = document.querySelector('#friend-request-badge');
friendRequestBadge.setAttribute('style', "display: none");
let hasRequests = false;
let acceptBtn = '';
let declineBtn = '';
let friendsArr = [];

let sentFrom = '';
let toggleBtn = false;

let profileImage = [];

const connectionFriend = new signalR.HubConnectionBuilder().withUrl("/friendRequestHub").build();

const printFriends = (friend, id, dataItem) => {
    const profileImg = document.createElement('img');
    const div = document.createElement('div');

    if (dataItem !== null) {
        profileImg.setAttribute('src', `data:image/png;base64,${dataItem.profileImage1}`);
        profileImg.classList.add('friend-profile-image');
        div.appendChild(profileImg);
    } else {
        const i = document.createElement('i');
        profileImg.setAttribute('src', `../img/test3.png`);
        profileImg.classList.add('friend-profile-image');
        //i.classList.add('fas', 'fa-user-circle', 'friend-no-profile-image');
        div.appendChild(profileImg);
    }

    const pf = document.createElement('p');
    div.classList.add('friend-div');
    pf.appendChild(document.createTextNode(`${friend}`));
    pf.className = 'friends';
    pf.setAttribute('data-friend', friend);
    div.appendChild(pf);
    pf.setAttribute('draggable', true);

    friendListContainer.appendChild(div);
}; // creates elements in the friend list

const mapFriends = (friends) => {
    fetchFriendsProfileImage().then(data => {
        friends ? friends.map((item, index) => printFriends(item.name, item.identityId, data[index])) : '';
        clickHandlerFriendItem(); // find method in chathub.js
    });
};

const requestResult = (hasRequest, friendRequestsArray, friends, hasAccepted) => {
    hasRequests = hasRequest;

    if (hasAccepted || hasAccepted === false) {
        resetFriendContainer();
    }
    mapFriends(friends);
    friendsArr = friends;
    sentFrom = friendRequestsArray;
    checkIfFriendDivHasCorrectClass();
};

const start = () => {
    connectionFriend.start()
        .catch((err) => console.error(err.toString())
        );
};

const checkIfFriendDivHasCorrectClass = () => {
    if (sentFrom.length === 0) {
        friendDiv.classList.remove('has-friend-request');
        friendRequestBadge.setAttribute('style', "display: none");
    } else {
        friendDiv.classList.add('has-friend-request');
        addFriendRequestBadge();
    }
};

const friendRequestItems = (item) => {
    const newDiv = document.createElement('div');
    acceptBtn = document.createElement('button');
    declineBtn = document.createElement('button');
    newDiv.className = "friend-request-item";
    acceptBtn.className = 'accept';
    declineBtn.className = 'ignore';
    acceptBtn.innerHTML = 'Accept';
    declineBtn.innerHTML = 'Ignore';
    const text = document.createTextNode(`Accept friend request from  ${item.fromUserName}`);
    newDiv.appendChild(text);
    newDiv.appendChild(acceptBtn);
    newDiv.appendChild(declineBtn);

    return newDiv;
};

const getUserResponse = (response) => {
    acceptBtn.addEventListener('click', (e) => {
        response.hasAccepted = true;
        connectionFriend.invoke('SendUserResponse', response);
        e.target.parentNode.innerHTML = `Your are now friends with ${response.fromUserName}`;
    });

    declineBtn.addEventListener('click', (e) => {
        response.hasAccepted = false;
        connectionFriend.invoke('SendUserResponse', response);
        e.target.parentNode.innerHTML = `You declined ${response.fromUserName}`;
    });
};

const userClickOnRequest = () => {
    toggleBtn = !toggleBtn ? toggleBtn = true : toggleBtn = false;

    if (toggleBtn) {
        if (sentFrom.length === 0) {
            friendRequestResultDiv.setAttribute('style', "display: block; z-index: 20;");
            friendRequestResultDiv.innerHTML = 'no friend requests';
        } else {
            console.log(sentFrom);

            sentFrom.map(item => {
                friendRequestResultDiv.appendChild(friendRequestItems(item));
                getUserResponse(item);
            });
            friendRequestResultDiv.setAttribute('style', "width: 80%; color: #fff; justify-self: center; z-index: 10;");
        }
    }
    else {
        friendRequestResultDiv.innerHTML = '';
        friendRequestResultDiv.removeAttribute('style');
        friendRequestResultDiv.setAttribute('style', "display: none");
    }
};

friendDiv.addEventListener('click', userClickOnRequest);

connectionFriend.on('ReceiveFriendRequest', requestResult);

start();

function resetFriendContainer() {
    friendListContainer.innerHTML = '';
};

function addFriendRequestBadge() {
    friendRequestBadge.setAttribute('style', "display: block");
    friendRequestBadge.innerHTML = sentFrom.length;
};

function fetchFriendsProfileImage() {
    return fetch("/profile/GetFreindsProfileImages")
        .then(res => res.json())
        .then(data => data.value);
}