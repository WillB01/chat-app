const friendDiv = document.querySelector('#friend');
const friendRequestResultDiv = document.querySelector('#friend-request-result');
const friendsContainer = document.querySelector('#friends-container');
let acceptBtn = '';
let declineBtn = '';

let sentFrom = '';
let toggleBtn = false;
const connectionFriend = new signalR.HubConnectionBuilder().withUrl("/friendRequestHub").build();

const printFriends = (friend) => {
    const pf = document.createElement('p');
    pf.appendChild(document.createTextNode(`${friend}`));
    pf.className = 'friends';
    pf.setAttribute('data-friend', friend);
    friendsContainer.appendChild(pf);
};

const mapFriends = (friends) => {
    friends ? friends.map(item => printFriends(item.name)) : '';
};

const requestResult = (hasRequest, friendRequestsArray, friends) => {
    sentFrom = friendRequestsArray;
    checkIfFriendDivHasCorrectClass();
    mapFriends(friends);
    if (hasRequest) {
        checkIfFriendDivHasCorrectClass();
    }
};

const start = () => {
    connectionFriend.start()
        .catch((err) => console.error(err.toString())
        );
};

const checkIfFriendDivHasCorrectClass = () => {

    sentFrom.length === 0
        ? friendDiv.classList.remove('has-friend-request')
        : friendDiv.classList.add('has-friend-request');
};

const friendRequestItems = (item) => {
    const newDiv = document.createElement('div');
    acceptBtn = document.createElement('button');
    declineBtn = document.createElement('button');
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
        checkIfFriendDivHasCorrectClass();
        resetFriendContainer();
      
       
    });

    declineBtn.addEventListener('click', (e) => {
        response.hasAccepted = false;
        connectionFriend.invoke('SendUserResponse', response);
        e.target.parentNode.innerHTML = `You declined ${response.fromUserName}`;
        checkIfFriendDivHasCorrectClass();
    });
};

const userClickOnRequest = () => {
    toggleBtn = !toggleBtn ? toggleBtn = true : toggleBtn = false;

    if (toggleBtn) {
        if (sentFrom.length === 0) {
            friendRequestResultDiv.setAttribute('style', "display: block");
            friendRequestResultDiv.innerHTML = 'no friend requests';
        } else {
            sentFrom.map(item => {
                friendRequestResultDiv.appendChild(friendRequestItems(item));
                getUserResponse(item);
            });
            friendRequestResultDiv.setAttribute('style', "background: gray; padding: 10px; margin-bottom: 10px");
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
    friendsContainer.innerHTML = '';
};