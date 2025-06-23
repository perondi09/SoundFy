using Data.Repository;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controllers
{
    public class OuvinteController : Controller
    {
        ArtistaRepository artistaRepository = new ArtistaRepository();
        OuvinteRepository ouvinteRepository = new OuvinteRepository();
              
        public IActionResult Erro()
        {
            return View("Erro");
        }

        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("logado") != "true")
                return RedirectToAction("Index", "Login");

            var musicas = artistaRepository.ListarMusicas();
            return View(musicas);
        }

        public IActionResult Reproduzir(int id)
        {
            if (HttpContext.Session.GetString("logado") != "true")
                return RedirectToAction("Index", "Login");

            var musicas = artistaRepository.ListarMusicas();
            var musica = musicas.FirstOrDefault(m => m.Id == id);

            if (musica == null)
            {
                return NotFound();
            }

            return View(musica);
        }
        
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
