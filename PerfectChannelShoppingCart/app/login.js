(function () {
    var btn$ = document.getElementById("loginBtn");
    btn$.onclick = function () {
        var usernameInput$ = document.getElementById("username");
        console.log(`the username is ${usernameInput$}`);
        document.cookie = "username = " + usernameInput$.value;
        window.location = "index.html";
    };
})();