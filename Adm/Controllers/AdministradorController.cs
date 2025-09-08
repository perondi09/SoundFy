using Adm.ViewModel;
using Business;
using Microsoft.AspNetCore.Mvc;

namespace Adm.Controllers
{
    public class AdministradorController : Controller
    {
        AdiministradorBusiness AdmBusiness = new AdiministradorBusiness();
        
        [Route("/Home")]
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
        
        public IActionResult Excluir(int Id)
        {
            if (HttpContext.Session.GetString("logado") != "true")
                return RedirectToAction("Index", "Login");

            AdmBusiness.ExcluirUsuarioPeloId(Id);
            return RedirectToAction("Index");
        }
    }
}