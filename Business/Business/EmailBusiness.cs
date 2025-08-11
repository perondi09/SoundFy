using System.Net.Mail;

namespace Business
{
    public class EmailBusiness
    {     
        private SmtpClient StartarServerEmail()
        {
            return new SmtpClient("localhost")
            {
                Port = 25,
                DeliveryMethod = SmtpDeliveryMethod.Network
            };
        }
   
        public bool EnviarEmailGenerico(string destinatario, string assunto, string corpo)
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

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ocorreu um erro ao enviar email. {ex.Message}");
                return false;
            }
        }
    }
}