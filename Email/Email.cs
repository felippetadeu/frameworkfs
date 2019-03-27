using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Email
{
    class Email
    {
        /// <summary>
        /// Classe de configuração de envio de email
        /// </summary>
        public class ConfiguracoesEmail
        {
            /// <summary>
            /// Email que será responsável por enviar
            /// </summary>
            public string De { get; set; } = "rsantosdesenv@gmail.com";

            /// <summary>
            /// Email ou emails (separados por ';') que receberão o e-mail
            /// </summary>
            public string Para { get; set; }

            /// <summary>
            /// Porta para envio do e-mail
            /// </summary>
            public int Porta { get; set; } = 587;

            /// <summary>
            /// Método de entrega do e-mail
            /// </summary>
            public SmtpDeliveryMethod MetodoEntrega { get; set; } = SmtpDeliveryMethod.Network;

            /// <summary>
            /// Host que será enviado o e-mail
            /// </summary>
            public string Host { get; set; } = "mx1537.wpservers.com.br";

            /// <summary>
            /// Assunto do e-mail
            /// </summary>
            public string Assunto { get; set; }

            /// <summary>
            /// Corpo do e-mail
            /// </summary>
            public string Corpo { get; set; }

            /// <summary>
            /// Credenciais para o envio do e-mail
            /// </summary>
            public NetworkCredential CredenciaisEnvio { get; set; }

            /// <summary>
            /// Corpo da mensagem é Html
            /// </summary>
            public bool CorpoHtml { get; set; } = false;

            /// <summary>
            /// Construtor
            /// </summary>
            /// <param name="de">Email que será responsável por enviar, caso não seja preenchido será associado o email: rsantosdesenv@gmail.com</param>
            /// <param name="para">Email ou emails (separados por ';') que receberão o e-mail</param>
            /// <param name="porta">Porta para envio do e-mail, caso não seja preenchida será utilizada a 587</param>
            /// <param name="metodoEntrega">Método de entrega do e-mail, caso não seja preenchido será utilizado o: SmtpDeliveryMethod.Network</param>
            /// <param name="host">Host que será enviado o e-mail, caso não seja preenchido será utilizado o: mx1537.wpservers.com.br</param>
            /// <param name="assunto">Assunto do e-mail</param>
            /// <param name="corpo">Corpo do e-mail</param>
            /// <param name="credenciaisEnvio">Credenciais para o envio do e-mail, caso não seja passada será utilizada as credenciais do rsantosdesenv@gmail.com</param>
            /// <param name="corpoHTML">Corpo do e-mail é HTML</param>
            public ConfiguracoesEmail(string para, string assunto, string corpo, string de = "rsantosdesenv@gmail.com", NetworkCredential credenciaisEnvio = null, int porta = 587, SmtpDeliveryMethod metodoEntrega = SmtpDeliveryMethod.Network, string host = "mx1537.wpservers.com.br", bool corpoHtml = false)
            {
                De = de;
                Para = para;
                Porta = porta;
                MetodoEntrega = metodoEntrega;
                Host = host;
                Assunto = assunto;
                CorpoHtml = corpoHtml;
                Corpo = corpo;
                CredenciaisEnvio = credenciaisEnvio;
                if (CredenciaisEnvio == null)
                {
                    CredenciaisEnvio = new NetworkCredential("rsantosdesenv@gmail.com", "M4952859");
                }
            }
        }

        public static class Mail
        {
            public static void EnviarEmail(ConfiguracoesEmail configuracao)
            {
                MailMessage mail = new MailMessage();
                mail.From = new MailAddress(configuracao.De, "RsWebDev");
                foreach (var item in configuracao.Para.Split(';'))
                {
                    mail.To.Add(new MailAddress(item));
                }
                SmtpClient client = new SmtpClient();
                client.Port = configuracao.Porta;
                client.DeliveryMethod = configuracao.MetodoEntrega;
                client.Host = configuracao.Host;
                client.Credentials = configuracao.CredenciaisEnvio;
                client.EnableSsl = true;
                mail.Subject = configuracao.Assunto;
                mail.IsBodyHtml = configuracao.CorpoHtml;
                mail.Body = configuracao.Corpo;
                client.Send(mail);
            }
        }
    }
}
