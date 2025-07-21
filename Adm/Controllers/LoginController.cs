using Business;
using Microsoft.AspNetCore.Mvc;

namespace SoundFy.Controllers
{
    public class LoginController : Controller
    {
        //Criação de objetos      
        AdiministradorBusiness adiministradorBusiness = new AdiministradorBusiness();

        // Retorno de view da pagina de login
        public IActionResult Index()
        {
            return View();
        }

        // Autenticação do usuario     
        [HttpPost]
        public IActionResult Autenticar(string email, string senha)
        {
            if (adiministradorBusiness.ValidarSeUsuarioExiste(email, senha))
            {
                HttpContext.Session.SetString("logado", "true");
                HttpContext.Session.SetString("tipoUsuario", "Administrador");
                return RedirectToAction("Index", "Administrador");
            }
            else
            {
                ViewBag.Mensagem = "Email ou senha inválidos.";
                return View("Index");
            }
        }

        // Método para deslogar o usuário
        public IActionResult Deslogar()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Login");
        }
    }
}