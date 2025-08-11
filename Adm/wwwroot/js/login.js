$(document).ready(function () {
    $("#formLogin").on("submit", function (e) {
        e.preventDefault();        
  
        $("#mensagemErro").hide();
        $("#loading").show();
        $("#formLogin button[type='submit']").prop('disabled', true).text('Carregando...');
        
        $.ajax({
            type: "POST",
            url: "/Login/Autenticar",
            data: {
                email: $("#email").val(),
                senha: $("#senha").val(),
                captcha: $("#captcha").val()
            },
            success: function (resposta) {
                if (resposta.sucesso) {
                    $("#loading").hide();
                    $("#mensagemSucesso").text("Login realizado! Redirecionando...").show();
                    window.location.href = resposta.redirecionar;
                } else {
                    $("#loading").hide();
                    $("#mensagemErro").text(resposta.mensagem).show();
                    $("#formLogin button[type='submit']").prop('disabled', false).text('Login');
                    
                    if (resposta.mensagem.includes("Captcha") || resposta.mensagem.includes("inválido")) {
                        setTimeout(function() {
                            location.reload();
                        }, 2000);
                    }
                }
            },
            error: function () {
                $("#loading").hide();
                $("#mensagemErro").text("Erro ao tentar autenticar.").show();
                $("#formLogin button[type='submit']").prop('disabled', false).text('Login');
            }
        });
    });
});
