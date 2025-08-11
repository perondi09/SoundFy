using Business;
using Microsoft.AspNetCore.Mvc;
using WebApp.ViewModel;

namespace WebApp.Controllers
{
    public class ArtistaController : Controller
    {       
        ArtistaBusiness artistaBusiness = new ArtistaBusiness();
        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("logado") != "true")
                return RedirectToAction("Index", "Login");

            int? usuarioId = HttpContext.Session.GetInt32("IdArtista");
            if (usuarioId == null)
            {
                TempData["Mensagem"] = "Sessão expirada. Faça login novamente.";
                return RedirectToAction("Index", "Login");
            }

            var musicas = artistaBusiness.ListarMusicasPorArtista(usuarioId.Value);
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

        [HttpGet]
        public IActionResult AdicionarMusica()
        {
            if (HttpContext.Session.GetString("logado") != "true")
                return RedirectToAction("Index", "Login");

            return View();
        }

        [HttpPost]
        public IActionResult AdicionarMusica(string titulo, string nomeArtista, string genero, int ano, IFormFile arquivo)
        {
            if (HttpContext.Session.GetString("logado") != "true")
                return RedirectToAction("Index", "Login");
            int? Usuario_Id = HttpContext.Session.GetInt32("IdArtista");
            if (Usuario_Id == null)
            {
                TempData["Mensagem"] = "Sessão expirada. Faça login novamente.";
                return RedirectToAction("Index", "Login");
            }

            if (arquivo == null || arquivo.Length == 0)
            {
                ModelState.AddModelError("", "Arquivo inválido.");
                return View();
            }

            using var memoryStream = new MemoryStream();
            arquivo.CopyTo(memoryStream);
            byte[] arquivoBytes = memoryStream.ToArray();


            bool sucesso = artistaBusiness.AdicionarMusica(
                titulo, nomeArtista, genero, ano, arquivo.FileName, arquivoBytes, Usuario_Id.Value);

            if (sucesso)
            {
                TempData["Mensagem"] = "Música adicionada com sucesso!";
                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError("", "Erro ao adicionar música.");
                return View();
            }
        }

        [HttpGet]
        public IActionResult ExcluirMusica(int id)
        {
            if (HttpContext.Session.GetString("logado") != "true")
                return RedirectToAction("Index", "Login");

            var sucesso = artistaBusiness.ExcluirMusicaPorId(id);

            if (sucesso)
                TempData["Mensagem"] = "Música excluída com sucesso!";
            else
                TempData["MensagemErro"] = "Erro ao excluir música.";

            return RedirectToAction("Index");
        }

    }
}