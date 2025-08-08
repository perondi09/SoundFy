$(document).ready(function () {
    $("#formLogin").on("submit", function (e) {
        e.preventDefault();
        $.ajax({
            type: "POST",
            url: "/Login/Autenticar",
            data: {
                email: $("#email").val(),
                senha: $("#senha").val()
            },
            success: function (resposta) {
                if (resposta.sucesso) {
                    window.location.href = resposta.redirecionar;
                } else {
                    $("#mensagemErro").text(resposta.mensagem);
                }
            },
            error: function () {
                $("#mensagemErro").text("Erro ao tentar autenticar.");
                $("#mensagemErro").show();
            }
        });
    });
});
