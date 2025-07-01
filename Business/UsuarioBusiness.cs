namespace Business
{
    public class UsuarioBusiness
    {
        public static string CriarCorpoLogin(string ip, string navegador, DateTime dataHora)
        {
            return $@"
            Seu login foi realizado com sucesso em {dataHora:dd/MM/yyyy HH:mm:ss}.
            IP de acesso: {ip}
            Navegador: {navegador}";
        }

        public static string CriarCorpoRecuperacao(string codigo)
        {
            return $@"
            Você solicitou a recuperação de sua conta.
            Seu código de verificação é: {codigo}
            Se você não solicitou, ignore este e-mail.";
        }

        public static string CriarCorporegistro()
        {
            return $@"
            Seja bem vindo ao SoundFy.";            
        }
    }
}