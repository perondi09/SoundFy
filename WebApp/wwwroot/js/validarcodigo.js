$(document).ready(function () {
    $('#btnenviacodigo').click(function () {
        const codigo = $('#codigo').val().trim();
        $('#mensagemCodigo').hide().text('');

        if (!codigo) {
            $('#mensagemCodigo').text("Digite o código.").show();
            return;
        }

        $.ajax({
            type: 'POST',
            url: '/Login/ValidarCodigoAjax',
            data: { codigo: codigo },
            success: function (res) {
                if (res.ok) {
                    if (res.tipo === "Ouvinte") {
                        window.location.href = "/Ouvinte/Index";
                    } else if (res.tipo === "Artista") {
                        window.location.href = "/Artista/Index";
                    } else {
                        $('#mensagemCodigo').text("Tipo de usuário inválido.").show();
                    }
                } else {
                    $('#mensagemCodigo').text(res.mensagem).show();
                }
            },
            error: function () {
                $('#mensagemCodigo').text("Erro ao validar o código.").show();
            }
        });
    });
});
