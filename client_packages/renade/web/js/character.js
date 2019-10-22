$(document).ready(() => {
  mp.trigger("toggleCreator");
});

createCharacter = () => {
  // TODO - name & familyName input
  mp.trigger(
    "createCharacter",
    "Hollow",
    "C",
    $("#gendermale").hasClass("on") ? 0 : 1,
    $("#mother")
      .text()
      .trim(),
    $("#father")
      .text()
      .trim(),
    $("#similar").val(),
    $("#skin").val(),
    $("#noseHeight").val(),
    $("#noseWidth").val(),
    $("#noseTipLength").val(),
    $("#noseDepth").val(),
    $("#noseTipHeight").val(),
    $("#noseBroke").val(),
    $("#eyebrowDepth").val(),
    $("#eyebrowHeight").val(),
    $("#cheekboneWidth").val(),
    $("#cheekboneHeight").val(),
    $("#cheekDepth").val(),
    $("#eyeScale").val(),
    $("#lipThickness").val(),
    $("#jawWidth").val(),
    $("#jawShape").val(),
    $("#chinHeight").val(),
    $("#chinDepth").val(),
    $("#chinWidth").val(),
    $("#chinIndent").val(),
    $("#neck").val(),
    $("#hairM").val(),
    $("#eyebrowsM").val(),
    $("#beard").val(),
    $("#eyeColor").val(),
    $("#hairColor").val()
  );
};
