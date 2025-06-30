using Data.Repository;
using Microsoft.AspNetCore.Mvc;
using WebApp.ViewModel;

namespace WebApp.Controllers
{
    public class OuvinteController : Controller
    {
         //Pagina de erro    
        public IActionResult Erro()
        {
            return View("Erro");
        }
        
       // Criação de objetos
        ArtistaRepository artistaRepository = new ArtistaRepository();
        OuvinteRepository ouvinteRepository = new OuvinteRepository();
        MusicaRepository musicaRepository = new MusicaRepository();

        // Retorno de view da pagina de ouvintes
        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("logado") != "true")
                return RedirectToAction("Index", "Login");

            var musicas = musicaRepository.ListarMusicas();
            var musicasVm = new List<MusicaViewModel>();

            foreach (var musica in musicas)
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

            return View(musicasVm);
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
