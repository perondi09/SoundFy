function login() {
    var email = document.getElementById("email").value;
    var senha = document.getElementById("senha").value;

    if (email === "" || senha === "") {
        alert("Preencha todos os campos.");
        return false;
    }
}