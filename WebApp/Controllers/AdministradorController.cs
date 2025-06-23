using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data.Repository;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controllers
{
    public class AdministradorController : Controller
    {
        AdministradorRepository administradorRepository = new AdministradorRepository();

        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("logado") != "true")
                return RedirectToAction("Index", "Login");

            var usuarios = administradorRepository.ListarUsuarios();
            return View(usuarios);
        }

        public IActionResult Excluir(int id)
        {
            if (HttpContext.Session.GetString("logado") != "true")
                return RedirectToAction("Index", "Login");

            administradorRepository.ExcluirUsuario(id);
            return RedirectToAction("Index");
        }
    }
}