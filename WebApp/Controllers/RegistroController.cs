using Business;
using Microsoft.AspNetCore.Mvc;

namespace SoundFy.Controllers
{
    public class RegistroController : Controller
    {
        //Criação de objetos
        UsuarioBusiness usuarioBusiness = new UsuarioBusiness();
        EmailBusiness emails = new EmailBusiness();

        // Método para retornar a view de registro
        public IActionResult Index()
        {
            return View();
        }

        // Método para registrar um novo usuário
        [HttpPost]
        public IActionResult Registrar(string email, string senha, string tipo)
        {
            if (usuarioBusiness.ValidarSeUsuarioExiste(email))
            {
                ViewBag.Mensagem = "Usuário já cadastrado.";
                return View("Index");
            }

            if (usuarioBusiness.RegistrarUsuario(email, senha, tipo))
            {
                return RedirectToAction("Index", "Login");
            }

            ViewBag.Mensagem = "Erro ao registrar usuário.";
            return View("Index");
        }

        //Método para enviar o e-mail de confirmação
        [HttpGet]
        public IActionResult ConfirmarEmail(string email)
        {
            string corpo = UsuarioBusiness.CriarCorporegistro();

            emails.EnviarEmailGenerico(email, "Confirmação de Login - SoundFy", corpo);

            return RedirectToAction("Index", "Login");
        }
    }
}
