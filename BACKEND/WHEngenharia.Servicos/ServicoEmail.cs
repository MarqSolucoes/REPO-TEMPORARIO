using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace WHEngenharia.Servicos
{
    public class ServicoEmail
    {
        public void EnviaEmail(List<MailAddress> emailsDestino, string titulo, string mensagem)
        {
            try
            {
                MailMessage message = new MailMessage();
                SmtpClient smtp = new SmtpClient();

                message.From = new MailAddress("osvaldo.torezan@solinski.com.br", "Osvaldo");

                emailsDestino.ForEach(x =>
                {
                    message.To.Add(x);
                });

                message.Subject = titulo;

                message.IsBodyHtml = true;
                message.Body = mensagem;
                smtp.Port = 587;
                smtp.Host = "mail.solinski.com.br";
                smtp.EnableSsl = true;
                smtp.UseDefaultCredentials = true;
                smtp.Credentials = new NetworkCredential("osvaldo.torezan@solinski.com.br", "#jA8AcZE8EWV%UhG");
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.Send(message);
            }
            catch(Exception ex)
            {

            }
        }
    }
}
