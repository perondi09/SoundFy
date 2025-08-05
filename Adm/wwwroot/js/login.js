function login() {
    var email = document.getElementById("email").value;
    var senha = document.getElementById("senha").value;

    if (email === "" || senha === "") {
        alert("Preencha todos os campos.");
        return false;
    }
}

$("#formLogin").submit(function (e) {
    e.preventDefault();

    $.ajax({
        type: "POST",
        url: '@Url.Action("Autenticar", "LoginController")',
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
        }
    });
});