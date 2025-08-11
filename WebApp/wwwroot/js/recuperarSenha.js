$(document).ready(function () {
    $("#btnEnviarEmail").on("click", function (e) {
        e.preventDefault();

        $("#mensagemEmail").hide();
        $("#loadingEmail").show();
        $("#btnEnviarEmail").prop('disabled', true).text('Enviando...');

        $.ajax({
            type: "POST",
            url: "/Login/RecuperarConta",
            data: {
                email: $("#email").val(),
            },
            success: function (resposta) {
                $("#loadingEmail").hide();

                if (resposta.ok) {
                    $("#mensagemEmail")
                        .removeClass("alert-danger")
                        .addClass("alert-success")
                        .text("Código enviado para seu email! Redirecionando...")
                        .show();

                    setTimeout(function () {
                        window.location.href = "/Login/ValidarCodigo";
                    }, 2000);
                } else {
                    $("#mensagemEmail")
                        .removeClass("alert-success")
                        .addClass("alert-danger")
                        .text(resposta.mensagem)
                        .show();
                    $("#btnEnviarEmail").prop('disabled', false).text('Enviar código');
                }
            },
            error: function () {
                $("#loadingEmail").hide();
                $("#mensagemEmail")
                    .removeClass("alert-success")
                    .addClass("alert-danger")
                    .text("Erro ao tentar enviar email.")
                    .show();
                $("#btnEnviarEmail").prop('disabled', false).text('Enviar código');
            }
        });
    });
});