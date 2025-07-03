using Business;
using Microsoft.AspNetCore.Mvc;
using WebApp.ViewModel;

namespace WebApp.Controllers
{
    public class AdministradorController : Controller
    {
        AdiministradorBusiness AdmBusiness = new AdiministradorBusiness();

        //Pagina inicial do administrador
        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("logado") != "true")
                return RedirectToAction("Index", "Login");

            var usuariosVm = new List<UsuarioViewModel>();
            var usuariosModel = AdmBusiness.ListarUsuariosPeloId();

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
        public IActionResult Excluir(int Id)
        {
            if (HttpContext.Session.GetString("logado") != "true")
                return RedirectToAction("Index", "Login");

            AdmBusiness.ExcluirUsuarioPeloId(Id);
            return RedirectToAction("Index");
        }
    }
}