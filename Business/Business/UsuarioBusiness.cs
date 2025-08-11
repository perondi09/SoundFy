using Data.Models;
using Data.Repository;

namespace Business
{
    public class UsuarioBusiness
    {
        UsuarioRepository usuarioRepo = new UsuarioRepository();
               
        public static string CriarCorpoLogin(string ip, string navegador, DateTime dataHora)
        {
            return $@"
            Seu login foi realizado com sucesso em {dataHora:dd/MM/yyyy HH:mm:ss}.
            IP: {ip}
            Navegador: {navegador}";
        }

        public static string CriarCorporegistro()
        {
            return $@"
            Seja bem vindo ao SoundFy.";
        }

        public bool ValidarUsuario(string email, string senha)
        {
            return usuarioRepo.ValidarUsuario(email, senha);
        }

        public bool ValidarSeUsuarioExiste(string email)
        {
            return usuarioRepo.ValidaUsuarioExistente(email);
        }

        public bool RegistrarUsuario(string email, string senha, string tipo)
        {
            return usuarioRepo.RegistrarUsuario(email, senha, tipo);
        }

        public string ObtemTipoUsuario(string email)
        {
            return usuarioRepo.ObterTipoUsuario(email);
        }      

        public UsuarioModel ObterUsuarioPorEmail(string email)
        {
            return usuarioRepo.ObterUsuarioPorEmail(email);
        }       

        public static string CriarCorpoRecuperacao(string codigo)
        {
            return $@"
            Você solicitou a recuperação de sua conta.
            Seu código de verificação é: {codigo}";
        }       
    }
}