using Data.Repository;
using Microsoft.AspNetCore.Mvc;
using WebApp.ViewModel;

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

            var usuariosVm = new List<UsuarioViewModel>();
            var usuariosModel = administradorRepository.ListarUsuarios();

            foreach (var usuario in usuariosModel)
            {
                usuariosVm.Add(new UsuarioViewModel
                {
                    Id = usuario.Id,
                    Email = usuario.Email,
                    Tipo = usuario.Tipo
                });
            }

            return View(usuariosVm);
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