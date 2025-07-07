$(document).ready(function () {
    // alert("Esta sendo referenciado")    
//Chamanda do botao clique
  $('#btnenviacodigo').click(function () {
    var codigo = $('#codigo').val()
 
    $.ajax({
      type: 'POST',
      url: '/Login/RecuperarConta',
      data: { codigo: codigo },
      success: function (response) {
        console.log('Sucesso:', response);
        alert('Email enviado com sucesso!');
      },
      error: function (xhr, status, error) {
        console.error('Erro:', error);
        alert('Erro ao enviar o email.');
      }
    });
  });
});
