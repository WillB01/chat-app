const searchInput = document.querySelector('#search-input');

let searchFriend = [];
var viewModel = new Bloodhound({
    datumTokenizer: Bloodhound.tokenizers.obj.whitespace('value'),
    queryTokenizer: Bloodhound.tokenizers.whitespace,

    remote: {
        url: '../search/friends/?q=%QUERY',
        wildcard: '%QUERY',
        filter: (resut) => {
            const friends = [...resut];
            searchFriend = friends;

            return $.map(friends, (res) => {
                return { userName: res.userName, id: res.id };
            })
        },
        rateLimitby: 3,
    }
});

$('#remote .typeahead').typeahead(null, {
    name: 'viewModel',
    displayKey: 'userName',
    source: viewModel,
});

const fetchPost = (itemToSend) => {
    fetch("/search/postsearchresult",
        {
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json',
            },
            method: "POST",

            body: JSON.stringify(itemToSend)
        })
        .then(function (res) { console.log(res); })
        .catch(function (res) { console.log(res); });
};

$('input.typeahead').on('typeahead:selected', function (event, selection) {
    fetchPost(selection);
});