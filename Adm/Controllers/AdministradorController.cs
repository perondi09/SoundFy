using Adm.ViewModel;
using Business;
using Data.Models;
using Microsoft.AspNetCore.Mvc;
using Data.ViewModel;
{
    
}

namespace Adm.Controllers
{
    public class AdministradorController : Controller
    {
        private readonly AdiministradorBusiness _bus = new AdiministradorBusiness();

        //Pagina inicial do administrador
        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("logado") != "true")
                return RedirectToAction("Index", "Login");

            var usuarios = _bus.ListarUsuariosPeloId();
            var usuariosVm = usuarios.Select(u => new UsuarioViewModel
            {
                Id    = u.Id,
                Email = u.Email,
                Tipo  = u.Tipo
            }).ToList();

            // mapeia estatísticas de artistas
            var estArt = _bus.ObterEstatisticasArtistas();
            ViewBag.EstatisticasArtistas = estArt
                .Select(a => new EstatisticaViewArtistaModel
                {
                    NomeArtista      = a.NomeArtista,
                    TotalMusicas     = a.TotalMusicas,
                    TotalReproducoes = a.TotalReproducoes
                })
                .ToList();

            // mapeia estatísticas de músicas
            var estMus = _bus.ObterEstatisticasMusicas();
            ViewBag.EstatisticasMusicas = estMus
                .Select(m => new EstatisticaMusicaViewModel
                {
                    Id               = m.Id,
                    Titulo           = m.Titulo,
                    NomeArtista      = m.NomeArtista,
                    TotalReproducoes = m.TotalReproducoes
                })
                .ToList();

            return View(usuariosVm);
        }

        //Metodo para excluir um usuario
        public IActionResult Excluir(int id)
        {
            if (HttpContext.Session.GetString("logado") != "true")
                return RedirectToAction("Index", "Login");

            _bus.ExcluirUsuarioPeloId(id);
            return RedirectToAction("Index");
        }
    }
}