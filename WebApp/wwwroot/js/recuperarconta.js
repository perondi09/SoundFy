$(document).ready(function () {
  $("#formRecuperar").submit(function (e) {
    e.preventDefault();

    const email = $("#email").val().trim();
    const regexEmail = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;

    $("#mensagemEmail").hide().text("");

    if (email === "") {
      $("#mensagemEmail").text("Preencha o e-mail.").show();
      return;
    }

    if (!regexEmail.test(email)) {
      $("#mensagemEmail").text("E-mail inválido.").show();
      return;
    }

    $.ajax({
      type: "POST",
      url: "/Login/RecuperarConta",
      data: { email: email },
      success: function (res) {
        if (res.ok) {
          window.location.href = "/Login/ValidarCodigo";
        } else {
          $("#mensagemEmail").text(res.mensagem).show();
        }
      },
      error: function () {
        $("#mensagemEmail").text("Erro ao processar a requisição.").show();
      },
    });
  });
});
