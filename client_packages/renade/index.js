let loginBrowser;

let currentOnline;
let maxOnline;

mp.events.add("authBrowser", (currentOnl, maxOnl) => {
  mp.gui.cursor.show(true, true);
  mp.gui.chat.activate(false);
  mp.game.ui.displayRadar(false);
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

mp.events.add(
  "loginPlayer",
  (loginOrEmailInput, passwordInput, saveCredentials) => {
    updateSavedCredentials(loginOrEmailInput, passwordInput, saveCredentials);
    mp.events.callRemote("LoginPlayer", loginOrEmailInput, passwordInput);
    setOnline(currentOnline, maxOnline);
  }
);

mp.events.add("logoutPlayer", () => {
  setSavedCredentials();
  setOnline(currentOnline, maxOnline);
});

mp.events.add("registerPlayer", (player, login, mail, password1, password2) => {
  mp.events.callRemote(
    "RegisterPlayer",
    player,
    login,
    mail,
    password1,
    password2
  );
});

mp.events.add("spawnPlayer", () => {
  mp.events.callRemote("SpawnPlayer");
});

mp.events.add("spawnPlayerSuccess", () => {
  mp.gui.cursor.show(false, false);
  mp.gui.chat.activate(true);

  loginBrowser.destroy();
});

mp.events.add(
  "createCharacter",
  (
    player,
    firstName,
    familyName,
    gender,
    mother,
    father,
    similarity,
    skinColor,
    noseHeight,
    noseWidth,
    noseLength,
    noseBridge,
    noseTip,
    noseBridgeTip,
    browWidth,
    browHeight,
    cheekboneWidth,
    cheekboneHeight,
    cheeksWidth,
    eyes,
    lips,
    jawWidth,
    jawHeight,
    chinLength,
    chinPosition,
    chinWidth,
    chinShape,
    neckWidth,
    hair,
    eyebrows,
    beard,
    eyeColor,
    hairColor
  ) => {
    mp.events.callRemote(
      "CreateCharacter",
      player,
      firstName,
      familyName,
      gender,
      mother,
      father,
      similarity,
      skinColor,
      noseHeight,
      noseWidth,
      noseLength,
      noseBridge,
      noseTip,
      noseBridgeTip,
      browWidth,
      browHeight,
      cheekboneWidth,
      cheekboneHeight,
      cheeksWidth,
      eyes,
      lips,
      jawWidth,
      jawHeight,
      chinLength,
      chinPosition,
      chinWidth,
      chinShape,
      neckWidth,
      hair,
      eyebrows,
      beard,
      eyeColor,
      hairColor
    );
    // TODO - do something UI-wise
  }
);

// TODO - accept an array of characters here
mp.events.add(
  "loginOrRegisterPlayerSuccess",
  (playerLogin, charFirstName, charFamilyName, charLvl) => {
    loginBrowser.execute(
      `loginOrRegisterSuccess("${playerLogin}", "${charFirstName}", "${charFamilyName}", "${charLvl}")`
    );
    setOnline(currentOnline, maxOnline);
  }
);

mp.events.add("loginPlayerFailure", () => {
  // TODO - handle login failure
});

mp.events.add("registerPlayerFailure", () => {
  // TODO - handle registration failure
});

let setOnline = (currentOnline, maxOnline) => {
  loginBrowser.execute(`setOnline("${currentOnline}", "${maxOnline}")`);
};

let updateSavedCredentials = (loginOrEmailInput, passwordInput, save) => {
  if (save)
    mp.storage.data.auth = {
      login: loginOrEmailInput,
      password: passwordInput
    };
  else {
    if (mp.storage) {
      if (mp.storage.data) {
        if (mp.storage.data.auth) delete mp.storage.data.auth;
      }
    }
  }
  mp.storage.flush();
};

let setSavedCredentials = () => {
  // Fix recursive storage error
  let storage = mp.storage.data;
  if (storage) {
    while (storage.hasOwnProperty("data")) storage = storage.data;
    delete mp.storage.data.data;
    Object.getOwnPropertyNames(storage).forEach(key => {
      mp.storage.data[key] = storage[key];
    });
    mp.storage.flush();
  }

  // Set saved credentials
  let savedAuth = mp.storage.data.auth;
  if (savedAuth) {
    let savedLogin = savedAuth.login;
    let savedPassword = savedAuth.password;
    if (savedLogin && savedPassword)
      loginBrowser.execute(
        `setSavedCredentials("${savedLogin}", "${savedPassword}")`
      );
  }
};

let debug = message => {
  if (message instanceof Object) {
    let output = "";
    for (let property in message)
      output += property + ": " + message[property] + "; ";
    message = output;
  }
  loginBrowser.execute(`debug("${message}")`);
};

mp.events.add("toggleCreator", () => {
  mp.players.local.freezePosition(true);

  let sceneryCamera = mp.cameras.new(
    "default",
    new mp.Vector3(
      mp.players.local.position.x + 0.25,
      mp.players.local.position.y + 1,
      mp.players.local.position.z + 0.5
    ),
    new mp.Vector3(0, 0, 165),
    40
  );

  sceneryCamera.pointAtCoord(
    new mp.Vector3(
      mp.players.local.position.x,
      mp.players.local.position.y,
      mp.players.local.position.z
    )
  );
  sceneryCamera.setActive(true);
  mp.game.cam.renderScriptCams(true, false, 0, true, false);
});

mp.events.add("changeAppearance", (index, value) => {
  mp.players.local.setFaceFeature(parseInt(index), parseInt(value));
});
