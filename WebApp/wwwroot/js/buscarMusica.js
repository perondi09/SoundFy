$('#btnBuscar').click(function () {
    var titulo = $('#buscaMusica').val();
    $.get('/Playlist/BuscarMusica', { titulo: titulo }, function (data) {
        var html = '';
        data.forEach(function (musica) {
            html += '<div>' + musica.Titulo +
                ' <button onclick="adicionarMusica(' + musica.Id + ')">Adicionar</button></div>';
        });
        $('#resultadosMusica').html(html);
    });
});

function adicionarMusica(idMusica) {
    var idPlaylist = $('#idPlaylist').val();
    $.post('/Playlist/AdicionarMusica', { idMusica: idMusica, idPlaylist: idPlaylist }, function (res) {
        if (res.sucesso) {
            alert('Música adicionada à playlist!');
        } else {
            alert('Erro ao adicionar música: ' + (res.erro || 'Desconhecido'));
        }
    });
}