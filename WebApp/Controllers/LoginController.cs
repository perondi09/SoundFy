using Business.Utilities;
using Data.Repository;
using Microsoft.AspNetCore.Mvc;

namespace SoundFy.Controllers
{
    public class LoginController : Controller
    {
        // Pagina de erro
        public IActionResult Erro()
        {
            return View("Erro");
        }

        //Criação de objetos
        UsuarioRepository usuarioRepository = new UsuarioRepository();
        EmailUltilities emails = new EmailUltilities();

        // Gera um captcha de 6 dígitos e salva na sessão
        private void GerarCaptcha()
        {
            var random = new Random();
            string captcha = random.Next(000000, 999999).ToString();
            HttpContext.Session.SetString("CaptchaLogin", captcha);
            ViewBag.Captcha = captcha;
        }

        // Retorno de view da pagina de login
        public IActionResult Index()
        {
            GerarCaptcha();
            return View();
        }

        // Botão para atualizar o captcha
        public IActionResult AtualizarCaptcha()
        {
            GerarCaptcha();
            return RedirectToAction("Index");
        }

        // Autenticação do usuario     
        [HttpPost]
        public IActionResult Autenticar(string email, string senha, string captcha)
        {
            string? captchaCorreto = HttpContext.Session.GetString("CaptchaLogin");
            if (captcha != captchaCorreto)
            {
                ViewBag.Mensagem = "Captcha incorreto.";
                GerarCaptcha();
                return View("Index");
            }

            var tipoUsuario = usuarioRepository.ObterTipoUsuario(email);
           
            if (tipoUsuario == null)
            {
                var adminRepo = new AdministradorRepository();
                if (adminRepo.ValidarAdministrador(email, senha))
                {
                    HttpContext.Session.SetString("logado", "true");
                    HttpContext.Session.SetString("tipoUsuario", "Administrador");
                    return RedirectToAction("Index", "Administrador");
                }
            }
            else if (usuarioRepository.ValidarUsuario(email, senha))
            {
                var ip = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "IP desconhecido";
                var navegador = Request.Headers["User-Agent"].ToString();

                emails.EnviarEmailLogin(email, ip, navegador);

                HttpContext.Session.SetString("logado", "true");
                HttpContext.Session.SetString("tipoUsuario", tipoUsuario ?? "");

                if (tipoUsuario == "Ouvinte")
                {
                    return RedirectToAction("Index", "Ouvinte");
                }
                else if (tipoUsuario == "Artista")
                {                   
                    var usuario = usuarioRepository.ObterUsuarioPorEmail(email);
                    if (usuario != null)
                        HttpContext.Session.SetInt32("IdArtista", usuario.Id);

                    return RedirectToAction("Index", "Artista");
                }
                else
                {
                    ViewBag.Mensagem = "Tipo de usuário não reconhecido.";
                    GerarCaptcha();
                    return View("Index", "Login");
                }
            }

            ViewBag.Mensagem = "E-mail ou senha inválidos.";
            GerarCaptcha();
            return View("Index", "Login");
        }

        //Retorno de view a pagina de recuperar senha
        public IActionResult RecuperarConta()
        {
            return View("RecuperarConta");
        }

        //Validação de email para recuperar conta
        [HttpPost]
        public IActionResult RecuperarConta(string email)
        {
            if (usuarioRepository.ValidaUsuarioExistente(email))
            {
                emails.EnviarCodigoRecuperacao(email);
                ViewBag.Mensagem = "Um e-mail de recuperação foi enviado para você.";
            }
            else
            {
                ViewBag.Mensagem = "E-mail não encontrado.";
            }
            return View("ValidarCodigo");
        }

        //Retorno de view a pagina validar codigo
        public IActionResult ValidarCodigo()
        {
            return View("ValidarCodigo");
        }

        //Validação do código de recuperação
        [HttpPost]
        public IActionResult ValidarCodigo(string codigo)
        {

            if (emails.ValidarCodigoRecuperacao(codigo))
            {
                return RedirectToAction("Index", "PaginaInicial");
            }
            else
            {
                ViewBag.Mensagem = "Código inválido.";
                return View("ValidarCodigo");
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