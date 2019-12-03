$(document).ready(() => {
  mp.trigger("toggleCreator");

  $("input[type=range]").on("input", function() {
    mp.trigger("changeFaceFeature", $(this).attr("name"), $(this).val());
  });

  $("#gendermale").click(function() {
    mp.trigger("setMale", true);
  });

  $("#genderfemale").click(function() {
    mp.trigger("setMale", false);
  });
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
