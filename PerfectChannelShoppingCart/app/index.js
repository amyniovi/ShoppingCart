(function() {
    var cart$ = [
        {
            id: 1,
            name: 'Chocolate',
            description: 'this is my chocolate'
        }
    ];

    var getUserName = function() {
        var username = document.cookie.match(new RegExp(name + "=([^;]+)"));
        if (username) return username;
        return undefined;
    };

    if (!getUserName()) {
        setTimeout(() => { window.location = "login.html"; }, 1000);
    }

})();