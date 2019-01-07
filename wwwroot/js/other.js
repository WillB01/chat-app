﻿const friendMenu = document.querySelector('#friend-list-menu');
const friendsAndChatContainer = document.querySelector('#friends-and-chat');
friendsAndChatContainer.classList.add('change-size-half');
//const backdrop = document.querySelector('.backdrop');
let showFriendContainer = false;

friendMenu.addEventListener('click', () => {
    showFriendContainer = !showFriendContainer ? showFriendContainer = true : showFriendContainer = false;
    const friendListContainer = document.querySelector('#friends-container');

    if (showFriendContainer) {
        friendListContainer.classList.add('display-none');
        friendsAndChatContainer.classList.remove('change-size-half');
        //backdrop.classList.remove('backdrop');


    } else {
        friendListContainer.classList.remove('display-none');
        friendsAndChatContainer.classList.remove('change-size-chat');
        friendsAndChatContainer.classList.add('change-size-half');
        //backdrop.classList.add('backdrop');



    }

  
});

fetchUserProfileImage().then(data => {
    const authNav = document.querySelector('#profile');

    console.log(data);
    const profileImg = document.createElement('img');
    const div = document.createElement('div');
    const p = document.createElement('p');
   
    p.appendChild(document.createTextNode(`${data.userName}`));
   
    if (data.img) {
        p.classList.add('centered-text');
        profileImg.setAttribute('src', `data:image/png;base64,${data.img}`);
        profileImg.classList.add('navbar-profile-image');
        authNav.appendChild(profileImg);
        authNav.appendChild(p);
    }
    if (!data.img) {
        p.classList.add('navbar-no-profile-image');
        div.appendChild(p);
        authNav.appendChild(div);
    }

   
   

        
});


function fetchUserProfileImage() {
    return fetch("/profile/GetImage")
        .then(res => res.json())
        .then(data => data.value);

}


