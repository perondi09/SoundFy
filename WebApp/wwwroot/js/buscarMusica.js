$(function () {
    function carregarMusicasDaPlaylist() {
        var idPlaylist = $("#playlist-id").val();
        $.get('/Playlist/MusicasDaPlaylist', { idPlaylist: idPlaylist }, function (musicas) {
            var lista = $("#lista-musicas");
            lista.empty();
            musicas.forEach(function (musica) {
                lista.append("<li>" + musica.Titulo + " - " + musica.NomeArtista + "</li>");
            });
        });
    }

    $("#search-musica").autocomplete({
        source: function (request, response) {
            $.ajax({
                url: '/Playlist/BuscarMusicas',
                dataType: 'json',
                data: { termo: request.term },
                success: function (data) {
                    response(data);
                }
            });
        },
        select: function (event, ui) {
            $("#search-musica").val(ui.item.label);
            $("#musica-id").val(ui.item.value);
            return false;
        },
        minLength: 2
    });
});