let loginBrowser;

let currentOnline;
let maxOnline;

mp.events.add("authBrowser", (currentOnl, maxOnl) => {
    mp.gui.cursor.show(true, true);
    mp.gui.chat.activate(false);
    loginBrowser = mp.browsers.new("package://renade/web/auth.html");

    currentOnline = currentOnl;
    maxOnline = maxOnl;

    setSavedCredentials();
    setOnline(currentOnline, maxOnline);
});

mp.events.add("kickBrowser", () => {
    mp.gui.cursor.show(true, true);
    mp.gui.chat.activate(false);
    mp.browsers.new("package://renade/web/kick.html");
});

mp.events.add("loginPlayer", (loginOrEmailInput, passwordInput, saveCredentials) => {
    updateSavedCredentials(loginOrEmailInput, passwordInput, saveCredentials);
    mp.events.callRemote("LoginPlayer", loginOrEmailInput, passwordInput);
    setOnline(currentOnline, maxOnline);
	mp.game.invoke("0xAAB3200ED59016BC", mp.players.local.handle, 1, 1);
});

mp.events.add("logoutPlayer", () => {
    setSavedCredentials();
    setOnline(currentOnline, maxOnline);
});

mp.events.add("registerPlayer", (player, login, mail, password1, password2) => {
    mp.events.callRemote("RegisterPlayer", player, login, mail, password1, password2);
});

mp.events.add("spawnPlayer", () => {
    mp.events.callRemote("SpawnPlayer");
    mp.game.invoke("0xD8295AF639FD9CB8", mp.players.local.handle, 1, 1);
});

mp.events.add("spawnPlayerSuccess", () => {
    mp.gui.cursor.show(false, false);
    mp.gui.chat.activate(true);

    loginBrowser.destroy();
});

// TODO - accept an array of characters here 
mp.events.add("loginOrRegisterPlayerSuccess", (playerLogin, charFirstName, charFamilyName, charLvl) => {
    loginBrowser.execute(`loginOrRegisterSuccess("${playerLogin}", "${charFirstName}", "${charFamilyName}", "${charLvl}")`);
    setOnline(currentOnline, maxOnline);
});

mp.events.add("loginPlayerFailure", () => {
    // TODO - handle login failure
});

mp.events.add("registerPlayerFailure", () => {
    // TODO - handle registration failure
});

let setOnline = (currentOnline, maxOnline) => {
    loginBrowser.execute(`setOnline("${currentOnline}", "${maxOnline}")`);
}

let updateSavedCredentials = (loginOrEmailInput, passwordInput, save) => {
    if (save)
        mp.storage.data.auth = { login: loginOrEmailInput, password: passwordInput };
    else {
        if (mp.storage) {
            if (mp.storage.data) {
                if (mp.storage.data.auth)
                    delete mp.storage.data.auth;
            }
        }
    }
    mp.storage.flush();
}

let setSavedCredentials = () => {
    // Fix recursive storage error
    let storage = mp.storage.data;
    if (storage) {
        while (storage.hasOwnProperty('data'))
            storage = storage.data;
        delete mp.storage.data.data;
        Object.getOwnPropertyNames(storage).forEach(key => { mp.storage.data[key] = storage[key]; });
        mp.storage.flush();
    }

    // Set saved credentials
    let savedAuth = mp.storage.data.auth;
    if (savedAuth) {
        let savedLogin = savedAuth.login;
        let savedPassword = savedAuth.password;
        if (savedLogin && savedPassword)
            loginBrowser.execute(`setSavedCredentials("${savedLogin}", "${savedPassword}")`);
    }
}

let debug = (message) => {
    if (message instanceof Object) {
        let output = '';
        for (let property in message)
            output += property + ': ' + message[property] + '; ';
        message = output;
    }
    loginBrowser.execute(`debug("${message}")`);
}

// TODO - refactor or remove 
mp.events.add("aTaser", (targetName, type) => {
    mp.game.graphics.notify(`~g~[DEBUG] <C>${targetName.name}</C> started up aTaser event!`);
    targetName.setToRagdoll(5000, 5000, type, false, false, false);
});