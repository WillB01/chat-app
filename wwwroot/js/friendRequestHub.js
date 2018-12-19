const friendDiv = document.querySelector('#friend');
const friendRequestResultDiv = document.querySelector('#friend-request-result');
let acceptBtn = '';
let declineBtn = '';


let sentFrom = '';
let toggleBtn = false;
const connectionFriend = new signalR.HubConnectionBuilder().withUrl("/friendRequestHub").build();


const requestResult = (hasRequest, sentfromName) => {
    sentFrom = sentfromName;
    
    console.log(sentfromName);
    if (hasRequest) {
       
        friendDiv.classList.add('has-friend-request');
    }
};

const start = () => {
    connectionFriend.start()
        .catch((err) => console.error(err.toString())
        );
        
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
    acceptBtn.addEventListener('click', () => {
        response.hasAccepted = true;
        connectionFriend.invoke('SendUserResponse', response);
        console.log(response);
        //postUserResponse(response);
});

declineBtn.addEventListener('click', () => {
    response.hasAccepted = false;
    //postUserResponse(response);
});

};


const userClickOnRequest = () => {
    toggleBtn = !toggleBtn ? toggleBtn = true : toggleBtn = false;

    if (toggleBtn) {

        if (sentFrom.length === 0) {
            friendRequestResultDiv.innerHTML = '';

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

const postUserResponse = (userResponse) => {
    fetch("/search/postsearchresult",
        {

            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json',
            },
            method: "POST",

            body: JSON.stringify(userResponse)
        })
        .then(function (res) { console.log(res); })
        //.then(function () {

        //    window.location.replace('/profile/');
        //})
        .catch(function (res) { console.log(res); });
};

friendDiv.addEventListener('click', userClickOnRequest);







connectionFriend.on('ReceiveFriendRequest', requestResult);
start();





