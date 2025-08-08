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
            try
            {
                throw new Exception("Erro de teste"); // Simulação de erro para teste

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

        // Método para deslogar o usuário
        public IActionResult Deslogar()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Login");
        }
    }
}