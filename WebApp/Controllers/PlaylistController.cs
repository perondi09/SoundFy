using Business;
using Business.Properties;
using Data.Models;
using Data.Repository;
using Microsoft.AspNetCore.Mvc;
using WebApp.ViewModel;

namespace WebApp.Controllers
{
    public class PlaylistController : Controller
    {
        private readonly PlaylistBusiness playlistBusiness = new PlaylistBusiness();
        private readonly OuvinteBusiness ouvinteBusiness = new OuvinteBusiness();

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
                return RedirectToAction("Index", "Login");
            }

            bool sucesso = playlistBusiness.AdicionarPlaylist(nome, usuarioId.Value);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult EditarPlaylist(int id)
        {
            if (HttpContext.Session.GetString("logado") != "true")
                return RedirectToAction("Index", "Login");

            var playlist = playlistBusiness.ObterPlaylistPorId(id);
            if (playlist == null)
                return NotFound();

            var playlistVm = new PlaylistViewModel
            {
                Id = playlist.Id,
                Nome_Playlist = playlist.Nome_Playlist,
                Usuario_Id = playlist.Usuario_Id
            };

            return View(playlistVm);
        }

        public IActionResult Excluir(int Id)
        {
            if (HttpContext.Session.GetString("logado") != "true")
                return RedirectToAction("Index", "Login");

            playlistBusiness.ExcluirPlaylistPorId(Id);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public JsonResult AdicionarMusica(int idMusica, int idPlaylist)
        {
            if (HttpContext.Session.GetString("logado") != "true")
                return Json(new { sucesso = false });

            playlistBusiness.AdicionarMusicaNaPlaylist(idMusica, idPlaylist);
            return Json(new { sucesso = true });
        }

        [HttpGet]
        public JsonResult BuscarMusicas(string termo)
        {
            var musicas = ouvinteBusiness.ListarMusicas()
                .Where(m => string.IsNullOrEmpty(termo) || m.Titulo.Contains(termo, StringComparison.OrdinalIgnoreCase))
                .Select(m => new { label = m.Titulo, value = m.Id })
                .ToList();

            return Json(musicas);
        }

        [HttpGet]
        public JsonResult MusicasDaPlaylist(int idPlaylist)
        {
            var playlist = playlistBusiness.ObterPlaylistPorId(idPlaylist);
            var musicas = new List<object>();

            if (playlist != null && playlist.Musicas != null)
            {
                musicas = playlist.Musicas
                    .Select(m => new { Titulo = m.Titulo, NomeArtista = m.NomeArtista })
                    .ToList<object>();
            }

            return Json(musicas);
        }
        
    }
}