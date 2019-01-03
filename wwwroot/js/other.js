const friendMenu = document.querySelector('#friend-list-menu');
const friendsAndChatContainer = document.querySelector('#friends-and-chat');
friendsAndChatContainer.classList.add('change-size-half');
let showFriendContainer = false;

friendMenu.addEventListener('click', () => {
    showFriendContainer = !showFriendContainer ? showFriendContainer = true : showFriendContainer = false;
    const friendListContainer = document.querySelector('#friends-container');

    if (showFriendContainer) {
        friendListContainer.classList.add('display-none');
        friendsAndChatContainer.classList.remove('change-size-half');


    } else {
        friendListContainer.classList.remove('display-none');
        friendsAndChatContainer.classList.remove('change-size-chat');
        friendsAndChatContainer.classList.add('change-size-half');



    }

  
});