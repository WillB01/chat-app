function readURL(input) {
    console.log(input);
    if (input.files && input.files[0]) {
        var reader = new FileReader();

        reader.onload = function (e) {
            const img = document.querySelector('#blah')
                img.setAttribute('src', e.target.result);
                img.setAttribute('style', 'width: 240px; height: 240px;');
        };

        reader.readAsDataURL(input.files[0]);
    }
}

document.querySelector("#ProfileImage").addEventListener('change',function () {
    readURL(this);
});

