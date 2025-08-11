$(document).ready(function () {
    const $btnCodigo = $("#btnenviacodigo");
    const $mensagem = $("#mensagemCodigo");
    const $loading = $("#loadingCodigo");

    function alterarEstadoBotao(disabled, texto) {
        $btnCodigo.prop('disabled', disabled).text(texto);
    }

    function exibirMensagem(tipo, texto) {
        $mensagem
            .removeClass("alert-success alert-danger")
            .addClass(`alert-${tipo}`)
            .text(texto)
            .show();
    }

    function redirecionarPorTipo(tipo) {
        const rotas = {
            ouvinte: "/Ouvinte/Index",
            artista: "/Artista/Index"
        };
        window.location.href = rotas[tipo?.toLowerCase()] || "/Login/Index";
    }

    $btnCodigo.on("click", function (e) {
        e.preventDefault();

        $mensagem.hide();
        $loading.show();
        alterarEstadoBotao(true, 'Validando...');

        $.ajax({
            type: "POST",
            url: "/Login/ValidarCodigo",
            data: { codigo: $("#codigo").val() },
            success: function (resposta) {
                $loading.hide();

                if (resposta.ok) {
                    exibirMensagem("success", "Código validado! Redirecionando...");
                    redirecionarPorTipo(resposta.tipo);
                } else {
                    exibirMensagem("danger", resposta.mensagem);
                    alterarEstadoBotao(false, 'Validar Código');
                }
            },
            error: function () {
                $loading.hide();
                exibirMensagem("danger", "Erro ao validar código.");
                alterarEstadoBotao(false, 'Validar Código');
            }
        });
    });
});