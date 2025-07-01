using System.Net.Mail;

namespace Business.Utilities
{
    public class EmailBusiness
    {
        //Criar SMTP Client
        private SmtpClient StartarServerEmail()
        {
            return new SmtpClient("localhost")
            {
                Port = 25,
                DeliveryMethod = SmtpDeliveryMethod.Network
            };
        }

        //Metodo para enviar email       
        public void EnviarEmailGenerico(string destinatario, string assunto, string corpo)
        {
            try
            {
                var smtpClient = StartarServerEmail();
                var mensagem = new MailMessage("nao-responda@soundfy.com", destinatario);
                {
                    mensagem.Subject = assunto;
                    mensagem.Body = corpo;
                    smtpClient.Send(mensagem);
                }
            }
            catch (SmtpException ex)
            {
                throw new Exception("Erro ao enviar e-mail: " + ex.Message);
            }
        }

    }
}