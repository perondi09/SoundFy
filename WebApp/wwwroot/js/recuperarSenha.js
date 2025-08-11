$(document).ready(function () {
    const $btnEmail = $("#btnEnviarEmail");
    const $mensagem = $("#mensagemEmail");
    const $loading = $("#loadingEmail");

    function alterarEstadoBotao(disabled, texto) {
        $btnEmail.prop('disabled', disabled).text(texto);
    }

    function exibirMensagem(tipo, texto) {
        $mensagem
            .removeClass("alert-success alert-danger")
            .addClass(`alert-${tipo}`)
            .text(texto)
            .show();
    }

    $btnEmail.on("click", function (e) {
        e.preventDefault();

        $mensagem.hide();
        $loading.show();
        alterarEstadoBotao(true, 'Enviando...');

        $.ajax({
            type: "POST",
            url: "/Login/RecuperarConta",
            data: { email: $("#email").val() },
            success: function (resposta) {
                $loading.hide();

                if (resposta.ok) {
                    exibirMensagem("success", "Código enviado para seu email! Redirecionando...");
                    window.location.href = "/Login/ValidarCodigo";
                } else {
                    exibirMensagem("danger", resposta.mensagem);
                    alterarEstadoBotao(false, 'Enviar código');
                }
            },
            error: function () {
                $loading.hide();
                exibirMensagem("danger", "Erro ao tentar enviar email.");
                alterarEstadoBotao(false, 'Enviar código');
            }
        });
    });
});