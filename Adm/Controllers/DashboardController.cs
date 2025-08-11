using Adm.ViewModel;
using Business.Properties;
using Microsoft.AspNetCore.Mvc;

namespace Adm.Controllers
{
    public class DashboardController : Controller
    {
        OuvinteBusiness ouvinteBusiness = new OuvinteBusiness();

        public IActionResult Dashboard()
        {
            if (HttpContext.Session.GetString("logado") != "true")
                return RedirectToAction("Dashboard", "Administrador");

            var reproducaoVm = new List<ReproducoesViewModel>();
            var musicaModel = ouvinteBusiness.ListarReproducoes();

            foreach (var musica in musicaModel)
            {
                reproducaoVm.Add(new ReproducoesViewModel
                {
                    Id = musica.Id,
                    Titulo = musica.Titulo,
                    NomeArtista = musica.NomeArtista,
                    Reproducao = musica.Reproducao,
                });
            }

            return View(reproducaoVm);
        }
    }
}