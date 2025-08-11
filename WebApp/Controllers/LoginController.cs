using Business;
using Data.Repository;
using Microsoft.AspNetCore.Mvc;

namespace SoundFy.Controllers
{
    public class LoginController : Controller
    {      
        UsuarioBusiness usuarioBusiness = new UsuarioBusiness();
        EmailBusiness emails = new EmailBusiness();
        AdiministradorBusiness adiministradorBusiness = new AdiministradorBusiness();

        private void GerarCaptcha()
        {
            var random = new Random();
            string captcha = random.Next(100000, 999999).ToString();
            HttpContext.Session.SetString("CaptchaLogin", captcha);
            ViewBag.Captcha = captcha;
        }

        public IActionResult Index()
        {
            GerarCaptcha();
            return View();
        }

        public IActionResult AtualizarCaptcha()
        {
            GerarCaptcha();
            return RedirectToAction("Index");
        }
  
        [HttpPost]
        public IActionResult Autenticar(string email, string senha, string captcha)
        {
            // Validação do captcha
            string? captchaCorreto = HttpContext.Session.GetString("CaptchaLogin");
            if (captcha != captchaCorreto)
            {
                GerarCaptcha();
                return Json(new { sucesso = false, mensagem = "Captcha incorreto." });
            }

            // Validação das credenciais
            if (!usuarioBusiness.ValidarUsuario(email, senha))
            {
                GerarCaptcha();
                return Json(new { sucesso = false, mensagem = "Email ou senha inválidos." });
            }

            // Obter tipo de usuário
            var tipoUsuario = usuarioBusiness.ObtemTipoUsuario(email);
            
            if (string.IsNullOrEmpty(tipoUsuario))
            {
                GerarCaptcha();
                return Json(new { sucesso = false, mensagem = "Tipo de usuário não identificado." });
            }

            try
            {
                // Enviar email de confirmação
                var ip = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "IP desconhecido";
                var navegador = Request.Headers["User-Agent"].ToString();
                var corpo = UsuarioBusiness.CriarCorpoLogin(ip, navegador, DateTime.Now);
                emails.EnviarEmailGenerico(email, "Confirmação de Login - SoundFy", corpo);
            }
            catch
            {
                // Log do erro seria ideal aqui
            }

            // Configurar sessão
            HttpContext.Session.SetString("logado", "true");
            HttpContext.Session.SetString("tipoUsuario", tipoUsuario);
            HttpContext.Session.SetString("emailUsuario", email);

            // Redirecionar baseado no tipo de usuário
            switch (tipoUsuario.ToLower())
            {
                case "ouvinte":
                    return Json(new { sucesso = true, redirecionar = Url.Action("Index", "Ouvinte") });
                    
                case "artista":
                    var usuario = usuarioBusiness.ObterUsuarioPorEmail(email);
                    if (usuario != null)
                        HttpContext.Session.SetInt32("IdArtista", usuario.Id);
                    return Json(new { sucesso = true, redirecionar = Url.Action("Index", "Artista") });
                    
                case "administrador":
                    return Json(new { sucesso = true, redirecionar = Url.Action("Index", "Dashboard") });
                    
                default:
                    GerarCaptcha();
                    return Json(new { sucesso = false, mensagem = "Tipo de usuário inválido." });
            }
        }

        public IActionResult RecuperarConta()
        {
            return View("RecuperarConta");
        }

        [HttpPost]
        public IActionResult RecuperarConta(string email)
        {
            try
            {
                if (usuarioBusiness.ValidarSeUsuarioExiste(email))
                {
                    string codigoVerificacao = new Random().Next(100000, 999999).ToString();

                    HttpContext.Session.SetString("CodigoVerificacao", codigoVerificacao);
                    HttpContext.Session.SetString("EmailRecuperacao", email);

                    string corpo = UsuarioBusiness.CriarCorpoRecuperacao(codigoVerificacao);

                    bool emailEnviado = emails.EnviarEmailGenerico(email, "Confirmação de Login - SoundFy", corpo);

                    if (emailEnviado)
                    {
                        return Json(new { ok = true });
                    }
                    else
                    {
                        return Json(new { ok = false, mensagem = "Erro ao enviar e-mail." });
                    }
                }
                else
                {
                    return Json(new { ok = false, mensagem = "E-mail não encontrado." });
                }
            }
            catch
            {
                return Json(new { ok = false, mensagem = "Erro no servidor." });
            }
        }

        public IActionResult ValidarCodigo()
        {
            return View("ValidarCodigo");
        }

        [HttpPost]
        public JsonResult ValidarCodigo(string codigo)
        {
            string codigoSessao = HttpContext.Session.GetString("CodigoVerificacao");
            string email = HttpContext.Session.GetString("EmailRecuperacao");

            if (codigo != codigoSessao)
            {
                return Json(new { ok = false, mensagem = "Código inválido." });
            }

            var tipoUsuario = usuarioBusiness.ObtemTipoUsuario(email);

            if (tipoUsuario == null)
                return Json(new { ok = false, mensagem = "Tipo de usuário não identificado." });

            HttpContext.Session.SetString("logado", "true");
            HttpContext.Session.SetString("tipoUsuario", tipoUsuario);

            if (tipoUsuario == "Artista")
            {
                var usuario = usuarioBusiness.ObterUsuarioPorEmail(email);
                if (usuario != null)
                    HttpContext.Session.SetInt32("IdArtista", usuario.Id);
            }

            return Json(new { ok = true, tipo = tipoUsuario });
        }

        public IActionResult Deslogar()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Login");
        }
    }
}