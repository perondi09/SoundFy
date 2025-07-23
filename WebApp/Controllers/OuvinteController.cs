using Business;
using Business.Properties;
using Data.Models;
using Microsoft.AspNetCore.Mvc;
using WebApp.ViewModel;

namespace WebApp.Controllers
{
    public class OuvinteController : Controller
    {
        // Criação de objetos
        OuvinteBusiness ouvinteBusiness = new OuvinteBusiness();

        public IActionResult Index()
        {
            if (HttpContext.Session == null || HttpContext.Session.GetString("logado") != "true")
                return RedirectToAction("Index", "Login");

            var musicasVm = new List<MusicaViewModel>();
            var musicasModel = ouvinteBusiness.ListarMusicas();
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

            var musicas = ouvinteBusiness.ListarMusicas();
            var musica = musicas.FirstOrDefault(m => m.Id == id);

            if (musica == null)
            {
                return NotFound();
            }

            return View(musica);
        }

        // Método para reproduzir o áudio da música 
        public FileStreamResult StreamAudio(int id)
        {
            if (HttpContext.Session.GetString("logado") != "true")
                return null;

            ouvinteBusiness.IncrementarReproducao(id);

            var bytes = ouvinteBusiness.ObterBytesMusicaPorId(id);
            return new FileStreamResult(new MemoryStream(bytes), "audio/mpeg");
        }
    }
}
