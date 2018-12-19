const friendDiv = document.querySelector('#friend');
const friendRequestResultDiv = document.querySelector('#friend-request-result');
let sentFrom = '';
let toggleBtn = false;
const connectionFriend = new signalR.HubConnectionBuilder().withUrl("/friendRequestHub").build();


const requestResult = (hasRequest, sentfromName) => {
    sentFrom = sentfromName;
    if (hasRequest) {
       
        friendDiv.classList.add('friend-test');
    }
};

const start = () => {
    connectionFriend.start()
        .catch((err) => console.error(err.toString())
        );
        
};

const toggle = (elem) => {

    // If the element is visible, hide it
    if (window.getComputedStyle(elem).display === 'block') {
        hide(elem);
        return;
    }

    // Otherwise, show it
    show(elem);

};

const show = function (elem) {
    elem.style.display = 'block';
};

// Hide an element
const hide = function (elem) {
    elem.style.display = 'none';
};


const userClickOnRequest = () => {
    toggleBtn = !toggleBtn ? toggleBtn = true : toggleBtn = false;
    
    console.log(toggleBtn);
    
    const newDiv = document.createElement('div');
    const p = document.createElement('p');


    if (toggleBtn) {
        !sentFrom
            ? friendRequestResultDiv.innerHTML = `<p> no new request </p>`
            : friendRequestResultDiv.innerHTML = `<p> new request from ${sentFrom}</p>`;

    } else {
        friendRequestResultDiv.innerHTML = '';
    }
   
    newDiv.appendChild(p);
    friendDiv.appendChild(p);
};


friendDiv.addEventListener('click', userClickOnRequest);
connectionFriend.on('ReceiveFriendRequest', requestResult);
start();





