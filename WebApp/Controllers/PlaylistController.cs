using Business;
using Data.Models;
using Microsoft.AspNetCore.Mvc;
using WebApp.ViewModel;
using System.Collections.Generic;

namespace WebApp.Controllers
{
    public class PlaylistController : Controller
    {
        private readonly PlaylistBusiness playlistBusiness = new PlaylistBusiness();

        public IActionResult Minhas()
        {
            int usuarioId = Convert.ToInt32(HttpContext.Session.GetString("usuarioId"));
            var playlists = playlistBusiness.ListarPlaylistsPorUsuario(usuarioId)
                .Select(p => new PlaylistViewModel
                {
                    Id = p.Id,
                    Nome = p.Nome,
                    UsuarioId = p.Usuario_Id
                }).ToList();

            return View(playlists);
        }

        public IActionResult Criar()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Criar(string nome)
        {
            int usuarioId = Convert.ToInt32(HttpContext.Session.GetString("usuarioId"));
            try
            {
                playlistBusiness.CriarPlaylist(nome, usuarioId);
                return RedirectToAction("Minhas");
            }
            catch (Exception ex)
            {
                ViewBag.Erro = ex.Message;
                return View();
            }
        }

        public IActionResult Detalhes(int id)
        {
            var musicas = playlistBusiness.ListarMusicasDaPlaylist(id)
                .Select(m => new MusicaViewModel
                {
                    Id = m.Id,
                    Titulo = m.Titulo,
                    NomeArtista = m.NomeArtista,
                    Genero = m.Genero,
                    Ano = m.Ano,
                    NomeArquivo = m.NomeArquivo
                }).ToList();

            var playlist = new PlaylistViewModel
            {
                Id = id,
                Musicas = musicas
            };

            return View(playlist);
        }

        [HttpPost]
        public IActionResult Excluir(int id)
        {
            playlistBusiness.ExcluirPlaylist(id);
            return RedirectToAction("Minhas");
        }

        [HttpPost]
        public IActionResult AdicionarMusica(int playlistId, int musicaId)
        {
            playlistBusiness.AdicionarMusicaNaPlaylist(playlistId, musicaId);
            return RedirectToAction("Detalhes", new { id = playlistId });
        }

        [HttpPost]
        public IActionResult RemoverMusica(int playlistId, int musicaId)
        {
            playlistBusiness.RemoverMusicaDaPlaylist(playlistId, musicaId);
            return RedirectToAction("Detalhes", new { id = playlistId });
        }
    }
}