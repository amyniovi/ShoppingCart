//Ugly code , should be encapsulated in an IIFE which is hard to do when dealing with eventhandlers registered on global scope.
//no time to fix the issue
//Angular JS would have been the technology of choice but I was not allowed to use it
//const cachedCartItems = JSON.parse(localStorage.getItem('items')) || [];

//vanilla JS Ajax request
//Partial page update
var makeRequest = function (url, method, callback) {
    method = method || 'GET';

    var oReq = new XMLHttpRequest();
    oReq.onload = function (arg) {
        callback(arg.target.response);
    };
    oReq.open(method, url, true);
    oReq.responseType = 'json';
    oReq.send();
};


//checks cookies for a username
var getUserName = function () {
    var username = document.cookie.match(new RegExp(name + "=([^;]+)"));
    if (username) return username[1];
    return undefined;
};

//set the username
var username = getUserName();

//gets latest cart for the user
var updateCartInformation = function () {
    makeRequest(`/api/cart/${username}`,
        'GET',
        (data) => {
            console.log(data);
            var cart$ = document.getElementById('cart');
            if (!data) {
                cart$.innerHTML = '<div>There are no items in your basket</div>';
            } else {
                updateCartInformation.cache = data.Items;
                cart$.innerHTML = '<table class="table table-striped">' +
                    data.Items.map(x => (`<div><tr><td >${x.Name}</td>` +
                        `<td>${x.Qty}</td>` +
                        `<td>£${(x.PricePerUnit * x.Qty).toFixed(2)}</td></tr></div>`))
                    .toString() +
                    `</table><div><button class="btn btn-primary" onclick="checkoutBasket()">Checkout</button></div>`;
                updateQuantities(data.Items.map(x => { return { Id: x.Id, Qty: x.Qty }; }));
            }
        });
};


// `<div><button  onclick="checkoutBasket(${data.Items })">Checkout</button></div>`;
//This function returns the invoice for the cart
var checkoutBasket = function () {
    window.location = "invoice.html";
   
};

 



//this redirects to a login page if there is no username, 
//If the user is logged in it gets all the items 
if (!username) {
    setTimeout(() => { window.location = "login.html"; }, 1000);
} else {

    makeRequest('/api/item', 'GET', (data) => {
        var item$ = document.getElementById('items');

        var itemsHtmlArray = data.map(item => {

            return '<div class="panel panel-default"><div class="panel-body panel-resizable"  >' +
                `<div class=" text-center"><h4>${item.Name}</h4 ><a href="${item.Uri}">Item Details</a></div>` +
                `<div><h4>£${(item.Price).toFixed(2)}</h4></div>` +
                 `<span class="text-danger"> ${item.Info || ''}</span>` +
                 '<div ><table><tr><td><table style="height=20px"   ><tr><td>' +
                ` <button onclick="AddQty(${item.Id})" class="input-group-addon" id="basic-addon1">+</button></tr></td  > ` +
                `<tr><td><button onclick="SubtractQty(${item.Id})" class="input-group-addon" id="basic-addon1">-</button></td></tr></table></td>` +
                `<td><input id="qty_${item.Id}" type="text" disabled="true" class="form-control" placeholder="1" value="1" aria-describedby="basic-addon1"></td>` +
                '</tr></table></div>' +
                `<span><button onclick="addToBasket(${item.Id})">Add to Basket</button></span>` +
                '</div></div>';
        });

        item$.innerHTML = itemsHtmlArray.join('');
        updateQuantities(data.map(x => { return { Id: x.Id, Qty: x.Qty }; }));
    });


    updateCartInformation();
   
}


var updateQuantities = function (idQtyKeyValuePairs) {
    makeRequest(`/api/cart/${username}`,
        'GET',
        (cartItems) => {
            idQtyKeyValuePairs.forEach(x => {
                if (cartItems)
                    var cartItem = cartItems.Items.find(cartItem => cartItem.Id == x.Id);
                if (cartItem)
                    var qty = cartItem.Qty;
                var element = getQtyElementForItem(x.Id);
                element.value = qty || 1;
            });

        });
};

var AddQty = function (id) {
    var qty = getQtyForItem(id);
    getQtyElementForItem(id).value = qty + 1;
};

var SubtractQty = function (id) {
    var qty = getQtyForItem(id);
    if (qty >= 2)
        getQtyElementForItem(id).value = qty - 1;


};

var getQtyElementForItem = function (id) {
    return document.getElementById(`qty_${id}`);
};

var getQtyForItem = function (id) {
    var qtyElement = getQtyElementForItem(id);
    return parseInt(qtyElement.value);
};

//Adds single item to basket and updates cart 
var addToBasket = function (id) {
    var qty = getQtyForItem(id);
    makeRequest(`/api/cart/${username}/item/${id}/${qty}`, 'POST',
        (data) => {
            console.log(data);
            updateCartInformation();
        });
};




