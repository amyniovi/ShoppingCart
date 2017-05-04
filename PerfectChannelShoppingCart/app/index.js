//Ugly code , should be encapsulated in an IIFE which is hard to do when dealing with eventhandlers registered on global scope.
//no time to fix the issue
//Angular JS would have been the technology of choice but I was not allowed to use it hence this is what i came up with.


//vanilla JS Ajax request
//Partial page update
var makeRequest = function (url, method, callback) {
    method = method || 'GET';
    
    var oReq = new XMLHttpRequest();
    oReq.onload = function(arg) {
        callback(arg.target.response);
    };
    oReq.open(method, url, true);
    oReq.responseType = 'json';
    oReq.send();
};


//checks cookies for a username
var getUserName = function() {
    var username = document.cookie.match(new RegExp(name+ "=([^;]+)"));
    if (username) return username[1];
    return undefined;
};

//set the username
var username = getUserName();

//gets latest cart for the user
var updateCartInformation = function() {
    makeRequest(`/api/cart/${username}`, 'GET', (data) => {
        var cart$ = document.getElementById('cart');
        if (!data) {
            cart$.innerHTML = '<div>There are no items in your basket</div>';
        } else {
            cart$.innerHTML = data.Items.map(x => (`<div>${x.Name}</div>`)).toString();
        }
    });
};

//this redirects to a login page if there is no username, else it gets all the items
if (!username) {
    setTimeout(() => { window.location = "login.html"; }, 1000);
} else {
    makeRequest('/api/item', 'GET', (data) => {
        var item$ = document.getElementById('items');
        var itemsHtmlArray = data.map(item => {
            return '<div class="panel panel-default"><div class="panel-body">' +
                `<div>${item.Name}</div>` +
                `<div>${item.Price}</div>` +
                `<button onclick="addToBasket(${item.Id})">Add to Basket</button>` +
                '</div></div>';
        });

        item$.innerHTML = itemsHtmlArray.join('');
    });

    updateCartInformation();
}


//Adds single item to basket and updates cart 
var addToBasket = function(id) {
//console.log(id);
makeRequest(`/api/cart/${username}/item/${id}`, 'POST',
    (data) => {
        console.log(data);
        updateCartInformation();
    });
};




