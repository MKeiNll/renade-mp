// https://keycode.info/
// Сделать Esc = кнопка назад
// keypress - https://stackoverflow.com/questions/3369593/how-to-detect-escape-key-press-with-pure-js-or-jquery
// Сделать Enter для перехода на некст поле
// input - https://stackoverflow.com/questions/40304963/how-to-focus-next-input-field-on-keypress/40305042
// input - http://jsfiddle.net/HF432/2/

let jsLogin = null;
let jsLogOut = null;
let jsReg = null;
let jsBackReg = null;
let jsEndReg = null;
let jsBackSlots = null;
let jsSlots = null;
let jsBackSpawn = null;
let jsSpawn = null;
let jsBackOffline = null;
let jsStore = null;

let loginPage = $(".login-page");
let regPage = $(".reg-page");
let slotsPage = $(".slots-page");
let spawnPage = $(".spawn-page");
let offlinePage = $(".offline-page");
let mainPages = $(
  ".login-page, .reg-page, .slots-page, .spawn-page, .offline-page"
);

// TODO - remove offline page & everything related to it
// TODO - refactor repeating code

let handleRegistration = () => {
  mp.trigger(
    "registerPlayer",
    $("#loginRegInput").val(),
    $("#emailRegInput").val(),
    $("#password1RegInput").val(),
    $("#password2RegInput").val()
  );
};

let handleSpawn = () => {
  mp.trigger("spawnPlayer");
};

let loginOrRegisterSuccess = (
  playerLogin,
  charFirstName,
  charFamilyName,
  charLvl
) => {
  $(".playerLogin").html(playerLogin);
  // TODO - this should happen after character selection
  $("#selectedCharacter").html(charFirstName + " " + charFamilyName);
  $("#firstCharacter").html(
    charFirstName + " " + charFamilyName + ", " + charLvl + " lvl"
  );
  mainPages.hide();
  slotsPage.show();
};

let setOnline = (currentOnline, maxOnline) => {
  $(".players").each(function() {
    $(this).text("(" + currentOnline + "/" + maxOnline + ")");
  });
};

let setSavedCredentials = (savedLogin, savedPassword) => {
  $("#loginLoginInput").val(savedLogin);
  $("#passwordLoginInput").val(savedPassword);
};

let debug = message => {
  let debugDiv = $("#debug");
  debugDiv.html(debugDiv.html() + message + "<br/>");
};

$(document).ready(() => {
  loginPage = $(".login-page");
  regPage = $(".reg-page");
  slotsPage = $(".slots-page");
  spawnPage = $(".spawn-page");
  offlinePage = $(".offline-page");

  mainPages = $(
    ".login-page, .reg-page, .slots-page, .spawn-page, .offline-page"
  );

  jsLogin = $(".js-login");
  jsLogOut = $(".js-logout");
  jsReg = $(".js-reg");
  jsBackReg = $(".js-back-reg");
  jsEndReg = $(".js-end-reg");
  jsBackSlots = $(".js-back-slots");
  jsSlots = $(".js-slots");
  jsBackSpawn = $(".js-back-spawn");
  jsSpawn = $(".js-spawn");
  jsBackOffline = $(".js-back-offline");
  jsStore = $(".js-store");

  loginPage.show();
  regPage.hide();
  slotsPage.hide();
  spawnPage.hide();
  offlinePage.hide();

  jsLogin.on("click", e => {
    e.preventDefault();
    mp.trigger(
      "loginPlayer",
      $("#loginLoginInput").val(),
      $("#passwordLoginInput").val(),
      $("#remember").is(":checked")
    );
  });

  jsEndReg.on("click", e => {
    e.preventDefault();
    mp.trigger(
      "registerPlayer",
      $("#loginRegInput").val(),
      $("#emailRegInput").val(),
      $("#password1RegInput").val(),
      $("#password2RegInput").val()
    );
  });

  // Регистрация. Перейти на страницу Регистрации
  jsReg.on("click", e => {
    e.preventDefault();
    mainPages.hide();
    regPage.show();
  });

  // Кнопка Назад. Вернуться на страницу Авторизации
  jsBackReg.on("click", e => {
    e.preventDefault();
    mainPages.hide();
    loginPage.show();
    mp.trigger("logoutPlayer");
  });

  // Выйти из аккаунта. Возвращает на странизцу Авторизации.
  jsLogOut.on("click", e => {
    e.preventDefault();
    mainPages.hide();
    loginPage.show();
    mp.trigger("logoutPlayer");
  });

  // Кнопка Назад. Вернуться на страницу Авторизации
  jsBackSlots.on("click", e => {
    e.preventDefault();
    mainPages.hide();
    loginPage.show();
    mp.trigger("logoutPlayer");
  });

  // Выбрал персонажа. Перейти на стринцу выбора места Спавна
  jsSlots.on("click", e => {
    e.preventDefault();
    mainPages.hide();
    spawnPage.show();
  });

  // Кнопка Назад. Вернуться на страницу Выбора Персонажа
  jsBackSpawn.on("click", e => {
    e.preventDefault();
    mainPages.hide();
    slotsPage.show();
  });

  // ЗАКОМЕНТИТЬ! Вторая яйчека неактивна. Третья ячека заблокирована. (Для приера)
  $(".slots.slot_2 ").addClass("disable");
  $(".slots.slot_3").addClass("lock");

  // Отключает переход на спаун для закрытых ячеек персонажа
  $(".slots.disable .js-slots").off("click");
  $(".slots.lock .js-slots").off("click");

  // ЗАКОМЕНТИТЬ! Аналогично для спауна
  $(".spawn.spawn_3 ").addClass("lock");
  $(".spawn.lock .js-spawn").off("click");

  // Цвет кнопки слотов персонажей. Красный (удалить), Зеленый (купить), Синий (создать).
  $(".slots .button").addClass("button-red js-char-delete");

  if ($(".slots").is(".disable")) {
    $(".disable .button").removeClass("button-red js-char-delete");
    $(".disable .button").addClass("button-blue js-create");
  }

  if ($(".slots").is(".lock")) {
    $(".lock .button").removeClass("button-red js-char-delete");
    $(".lock .button").addClass("button-green js-char-store");
  }

  // Последовательное переключение по окнам ввода (input) при нажатии "Enter"
  $(".inputs").keydown(function(e) {
    if (e.which === 13) {
      var index = $(".inputs").index(this) + 1;
      $(".inputs")
        .eq(index)
        .focus();
    }
  });

  // Клавиша "ESC" дублирует действие кнопки "Назад"
  $(this).on("keyup", function(evt) {
    if (evt.keyCode == 27) {
      if (regPage.is(":visible")) {
        mainPages.hide();
        loginPage.show();
        mp.trigger("logoutPlayer");
      }
      if (slotsPage.is(":visible")) {
        mainPages.hide();
        loginPage.show();
        mp.trigger("logoutPlayer");
      }
      if (spawnPage.is(":visible")) {
        mainPages.hide();
        slotsPage.show();
        mp.trigger("logoutPlayer");
      }
      if (offlinePage.is(":visible")) {
        mainPages.hide();
        loginPage.show();
        mp.trigger("logoutPlayer");
      }
    }
  });

  // Показывате статус сервера (используются классы 'on' и off')
  $(".server").addClass("on");
});

// Удалить 'js-char-delete' и созадить 'js-char-create', нужно использовать в свяке с id слота
