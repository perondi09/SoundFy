using Business;
using Data.Models;
using Microsoft.AspNetCore.Mvc;
using WebApp.ViewModel;

namespace WebApp.Controllers
{
    public class PlaylistController : Controller
    {
        private readonly PlaylistBusiness playlistBusiness = new PlaylistBusiness();

        public IActionResult Index()
        {
            if (HttpContext.Session == null || HttpContext.Session.GetString("logado") != "true")
                return RedirectToAction("Index", "Login");

            int? usuarioId = HttpContext.Session.GetInt32("UsuarioId");
            if (usuarioId == null)
                return RedirectToAction("Index", "Login");

            var playlistVm = new List<PlaylistViewModel>();
            var playlistModel = playlistBusiness.ListarPlaylist()
                .Where(p => p.Usuario_Id == usuarioId.Value)
                .ToList();

            MapPlaylistModelParaPlaylistViewModel(playlistVm, playlistModel);

            return View(playlistVm);
        }

        private void MapPlaylistModelParaPlaylistViewModel(List<PlaylistViewModel> playlistVm, List<PlaylistModel> playlistModel)
        {
            foreach (var playlist in playlistModel)
            {
                playlistVm.Add(new PlaylistViewModel
                {
                    Id = playlist.Id,
                    Nome_Playlist = playlist.Nome_Playlist,
                    Usuario_Id = playlist.Usuario_Id,
                    Musicas = playlist.Musicas.Select(m => new MusicaViewModel
                    {
                        Id = m.Id,
                        Titulo = m.Titulo,
                        NomeArtista = m.NomeArtista,
                        Genero = m.Genero,
                        Ano = m.Ano,
                        NomeArquivo = m.NomeArquivo
                    }).ToList()
                });
            }
        }

        [HttpGet]
        public IActionResult AdicionarPlaylist()
        {
            if (HttpContext.Session.GetString("logado") != "true")
                return RedirectToAction("Index", "Login");

            return View();
        }

        [HttpPost]
        public IActionResult AdicionarPlaylist(string nome)
        {
            if (HttpContext.Session.GetString("logado") != "true")
                return RedirectToAction("Index", "Login");

            int? usuarioId = HttpContext.Session.GetInt32("UsuarioId");
            if (usuarioId == null)
            {
                TempData["Mensagem"] = "Sessão expirada. Faça login novamente.";
                return RedirectToAction("Index", "Login");
            }

            bool sucesso = playlistBusiness.AdicionarPlaylist(nome, usuarioId.Value);

            if (sucesso)
                TempData["Mensagem"] = "Playlist criada com sucesso!";
            else
                TempData["Mensagem"] = "Erro ao criar playlist.";

            return RedirectToAction("Index");
        }
    }
}