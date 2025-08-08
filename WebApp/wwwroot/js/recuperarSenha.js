$(document).ready(function () {
    $("#btnEnviarEmail").on("click", function (e) {
        e.preventDefault();
        
        var email = $("#email").val();
        
        if (!email) {
            $("#mensagemEmail").text("Por favor, digite seu email.").show();
            return;
        }

        // Mostrar loading
        $("#mensagemEmail").hide();
        $("#loadingEmail").show();
        $("#btnEnviarEmail").prop('disabled', true).text('Enviando...');

        $.ajax({
            type: "POST",
            url: "/Login/RecuperarConta",
            data: {
                email: email
            },
            success: function (resposta) {
                $("#loadingEmail").hide();
                
                if (resposta.ok) {
                    $("#mensagemEmail")
                        .removeClass("alert-danger")
                        .addClass("alert-success")
                        .text("Código enviado para seu email! Redirecionando...")
                        .show();
                    
                    setTimeout(function() {
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

    $("#btnenviacodigo").on("click", function (e) {
        e.preventDefault();
        
        var codigo = $("#codigo").val();
        
        if (!codigo) {
            $("#mensagemCodigo").text("Por favor, digite o código.").show();
            return;
        }

        // Mostrar loading
        $("#mensagemCodigo").hide();
        $("#loadingCodigo").show();
        $("#btnenviacodigo").prop('disabled', true).text('Validando...');

        $.ajax({
            type: "POST",
            url: "/Login/ValidarCodigo",
            data: {
                codigo: codigo
            },
            success: function (resposta) {
                $("#loadingCodigo").hide();
                
                if (resposta.ok) {
                    $("#mensagemCodigo")
                        .removeClass("alert-danger")
                        .addClass("alert-success")
                        .text("Código validado! Redirecionando...")
                        .show();
                    
                    setTimeout(function() {
                        switch (resposta.tipo.toLowerCase()) {
                            case "ouvinte":
                                window.location.href = "/Ouvinte/Index";
                                break;
                            case "artista":
                                window.location.href = "/Artista/Index";
                                break;
                            case "administrador":
                                window.location.href = "/Dashboard/Index";
                                break;
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
});