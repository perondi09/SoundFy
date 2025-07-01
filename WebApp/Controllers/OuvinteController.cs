using Data.Models;
using Data.Repository;
using Microsoft.AspNetCore.Mvc;
using WebApp.ViewModel;
using Microsoft.AspNetCore.Http;

namespace WebApp.Controllers
{
    public class OuvinteController : Controller
    {     
       // Criação de objetos
        ArtistaRepository artistaRepository = new ArtistaRepository();
        OuvinteRepository ouvinteRepository = new OuvinteRepository();
        MusicaRepository musicaRepository = new MusicaRepository();

        public IActionResult Index()
        {
            if (HttpContext.Session == null || HttpContext.Session.GetString("logado") != "true")
                return RedirectToAction("Index", "Login");

            var musicasVm = new List<MusicaViewModel>();
            var musicasModel = musicaRepository.ListarMusicas();
            MapMusicasModelParaMusicasViewModel(musicasVm, musicasModel);

            return View(musicasVm);
        }

        private static void MapMusicasModelParaMusicasViewModel(List<MusicaViewModel> musicasVm, List<MusicaModel> musicasModel)
        {
            foreach (var musica in musicasModel)
            {
                musicasVm.Add(new MusicaViewModel
                {
                    Id = musica.Id,
                    Titulo = musica.Titulo,
                    NomeArtista = musica.NomeArtista,
                    Genero = musica.Genero,
                    Ano = musica.Ano,
                    NomeArquivo = musica.NomeArquivo
                });
            }
        }

        // Retorno de view para reproduzir uma música
        public IActionResult Reproduzir(int id)
        {
            if (HttpContext.Session.GetString("logado") != "true")
                return RedirectToAction("Index", "Login");

            var musicas = musicaRepository.ListarMusicas();
            var musica = musicas.FirstOrDefault(m => m.Id == id);

            if (musica == null)
            {
                return NotFound();
            }

            return View(musica);
        }
       
        // Método para reproduzir o áudio da música 
        public IActionResult StreamAudio(int id)
        {
            byte[]? audioBytes = ouvinteRepository.ObterBytesMusicaPorId(id);
            if (audioBytes == null)
            {
                return NotFound();
            }

            return File(audioBytes, "audio/mpeg");
        }
    }
}
