
if (!username)
    window.location = "login.html";
else {
    makeRequest(`api/order/cart/${username}`, 'GET', (data) => {
        var invoiceElement = document.querySelector('.invoice');
        var orderedItemsArrHtml = data.OrderedItems.map(item => {
            return `<tr><td>${item.Name}</td><td>${item.Qty}</td><td>£${(item.PricePerUnit * item.Qty).toFixed(2)}</td><tr>`;
            }
        );

        invoiceElement.innerHTML = orderedItemsArrHtml.join('') +
            `<tr><td><h3>Total Price : </h3></td><td><h3>${data.TotalPrice}</h3></td></tr>`;

    });
}
