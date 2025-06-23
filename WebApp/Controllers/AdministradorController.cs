using Data.Repository;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controllers
{
    public class AdministradorController : Controller
    {
        AdministradorRepository administradorRepository = new AdministradorRepository();

        //Pagina inicial do administrador
        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("logado") != "true")
                return RedirectToAction("Index", "Login");

            var usuarios = administradorRepository.ListarUsuarios();
            return View(usuarios);
        }

        //Metodo para excluir um usuario
        public IActionResult Excluir(int id)
        {
            if (HttpContext.Session.GetString("logado") != "true")
                return RedirectToAction("Index", "Login");

            administradorRepository.ExcluirUsuario(id);
            return RedirectToAction("Index");
        }
    }
}