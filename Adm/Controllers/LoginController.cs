using Business;
using Microsoft.AspNetCore.Mvc;

namespace SoundFy.Controllers
{
    public class LoginController : Controller
    {        
        AdiministradorBusiness adiministradorBusiness = new AdiministradorBusiness();

        public IActionResult Index()
        {
            return View();
        }
  
        [HttpPost]
        public IActionResult Autenticar(string email, string senha)
        {
            try
            {               
                if (adiministradorBusiness.ValidarSeUsuarioExiste(email, senha))
                {
                    HttpContext.Session.SetString("logado", "true");

                    return Json(new
                    {
                        sucesso = true,
                        redirecionar = Url.Action("Index", "Administrador")
                    });
                }
                return Json(new
                {
                    sucesso = false,
                    mensagem = "Email ou senha inválidos."
                });

            }

            catch (BusinessException ex)
            {
                Console.WriteLine($"Erro de negócio: {ex.Message}, Código de erro: {ex.ErrorCode}");
                return Json(new
                {
                    sucesso = false,
                    mensagem = "Erro de negócio: " + ex.Message
                });
            }

            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return Json(new
                {
                    sucesso = false,
                    mensagem = "Ocorreu um erro ao autenticar o usuário. Por favor, tente novamente."
                });
            }            
        }
    
        public IActionResult Deslogar()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Login");
        }
    }
}