
var viewModel = new Bloodhound({
    datumTokenizer: Bloodhound.tokenizers.obj.whitespace('value'),
    queryTokenizer: Bloodhound.tokenizers.whitespace,
   

    remote: {
        url: '../search/friends/?q=%QUERY',
        wildcard: '%QUERY',
        filter: (list) => {
           
            let test = [...list];
            console.log(test);
            return $.map(test, (name) => {
                return { user: name.userName };
            })
            
        },
        rateLimitby: 3
    }
});



// passing in `null` for the `options` arguments will result in the default
// options being used
$('#remote .typeahead').typeahead(null, {
    name: 'viewModel',
    displayKey: 'user',
    source: viewModel
});

