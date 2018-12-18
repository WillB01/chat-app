const connectionFriend = new signalR.HubConnectionBuilder().withUrl("/friendRequestHub").build();

const test = (test) => {
    console.log(test);
    if (test) {
        let t = document.querySelector('#friend');
        t.classList.add('friend-test');
    }
};

const start = () => {
    connectionFriend.start()
        .catch((err) => console.error(err.toString())
        );
        
};
connectionFriend.on('ReceiveFriendRequest', test);
start();





