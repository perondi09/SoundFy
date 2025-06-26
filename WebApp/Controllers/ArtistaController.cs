using Data.Repository;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controllers
{
    public class ArtistaController : Controller
    {
        // Pagina de erro
        public IActionResult Erro()
        {
            return View("Erro");
        }

        // Criação da instancia do ArtistaRepository para manipular músicas
        ArtistaRepository artistaRepository = new ArtistaRepository();

        //Login com sessao
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

            var musicas = artistaRepository.ListarMusicasPorUsuario(usuarioId.Value);
            return View(musicas);
        }
        //Pagina para adicionar musicas
        [HttpGet]
        public IActionResult AdicionarMusica()
        {
            if (HttpContext.Session.GetString("logado") != "true")
                return RedirectToAction("Index", "Login");

            return View();
        }

        //Metodo para adicionar musicas
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


            bool sucesso = artistaRepository.AdicionarMusica(
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

        //Metodo para excluir musicas
        [HttpGet]
        public IActionResult ExcluirMusica(int id)
        {
            if (HttpContext.Session.GetString("logado") != "true")
                return RedirectToAction("Index", "Login");

            var sucesso = artistaRepository.ExcluirMusicaPorId(id);

            if (sucesso)
                TempData["Mensagem"] = "Música excluída com sucesso!";
            else
                TempData["MensagemErro"] = "Erro ao excluir música.";

            return RedirectToAction("Index");
        }

    }
}