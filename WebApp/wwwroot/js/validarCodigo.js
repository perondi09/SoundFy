$("#btnenviacodigo").on("click", function (e) {
    e.preventDefault();

    $("#mensagemCodigo").hide();
    $("#loadingCodigo").show();
    $("#btnenviacodigo").prop('disabled', true).text('Validando...');

    $.ajax({
        type: "POST",
        url: "/Login/ValidarCodigo",
        data: {
            codigo: $("#codigo").val()
        },
        success: function (resposta) {
            $("#loadingCodigo").hide();

            if (resposta.ok) {
                $("#mensagemCodigo")
                    .removeClass("alert-danger")
                    .addClass("alert-success")
                    .text("Código validado! Redirecionando...")
                    .show();

                setTimeout(function () {
                    switch (resposta.tipo.toLowerCase()) {
                        case "ouvinte":
                            window.location.href = "/Ouvinte/Index";
                            break;
                        case "artista":
                            window.location.href = "/Artista/Index";                       
                        default:
                            window.location.href = "/Login/Index";
                    }
                }, 2000);
            } else {
                $("#mensagemCodigo")
                    .removeClass("alert-success")
                    .addClass("alert-danger")
                    .text(resposta.mensagem)
                    .show();
                $("#btnenviacodigo").prop('disabled', false).text('Validar Código');
            }
        },
        error: function () {
            $("#loadingCodigo").hide();
            $("#mensagemCodigo")
                .removeClass("alert-success")
                .addClass("alert-danger")
                .text("Erro ao validar código.")
                .show();
            $("#btnenviacodigo").prop('disabled', false).text('Validar Código');
        }
    });
});