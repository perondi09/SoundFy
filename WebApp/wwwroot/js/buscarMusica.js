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

    $("#btn-adicionar-musica").on("click", function () {
        var idMusica = $("#musica-id").val();
        var idPlaylist = $("#playlist-id").val();
        if (idMusica && idPlaylist) {
            $.post('/Playlist/AdicionarMusica', { idMusica: idMusica, idPlaylist: idPlaylist }, function (res) {
                if (res.sucesso) {
                    carregarMusicasDaPlaylist();
                    $("#search-musica").val('');
                    $("#musica-id").val('');
                }
            });
        }
    });

    $("#nome_musica").on("focus", function () {
        var currentValue = $(this).val();
        $(this).autocomplete("search", currentValue);
    });

    $("#nome_musica").on("blur", function () {
        var currentValue = $(this).val();
        $(this).autocomplete("option", "source", function (request, response) {
            GetSugestaoMusica("", currentValue, (lista) => {
                if (!Array.isArray(lista)) {
                    response([]);
                    return;
                }
                response(lista.map(item => ({
                    label: item.Text,
                    value: item.Value,
                    id: item.Value
                })));
            });
        });
    });

    function GetSugestaoMusica(term, id, callback) {
        let url = `${urlConsultaMusica}/ConsultaMusicasAutoComplete`;
        var param = { term: term, id: id };
        PostAjax(url, param, function (data) {
            if (data.ok) {
                if (data.mensagemErro && data.mensagemErro !== "") {
                    callback([]);
                } else {
                    callback(data.lista);
                }
            }
        });
    }

    // Carrega as músicas ao abrir a tela
    carregarMusicasDaPlaylist();
});