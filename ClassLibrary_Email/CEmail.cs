using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace ClassLibrary_Email
{
    public class CEmail
    {
        /// <summary>
        /// Método para disparar e-mail
        /// </summary>
        /// <param name="MailTo">Destinatários do e-mail</param>
        /// <param name="MailCC">Copiados do e-mail</param>
        /// <param name="MailOCC">Cópias ocultas do e-mail</param>
        /// <param name="strFrom">Remetente</param>
        /// <param name="strSubject">Assunto</param>
        /// <param name="strMessage">Mensagem</param>
        /// <param name="strSMTP">Servidor de SMTP</param>
        /// <param name="intPort">Número da porta utilizada no servidor de SMTP</param>
        /// <returns>booleano (true ou false)</returns>
        public bool SendMail(List<Email> MailTo, List<Email> MailCC, List<Email> MailOCC, string strFrom, string strSubject, string strMessage, string strSMTP, int? intPort = 587)
        {
            try
            {
                MailMessage _objMailMessage = new MailMessage();
                for (int i = 0; i < MailTo.Count; i++)
                {
                    _objMailMessage.To.Add(MailTo[i].strEmail);
                }
                if (MailCC != null)
                {
                    for (int i = 0; i < MailCC.Count; i++)
                    {
                        _objMailMessage.CC.Add(MailCC[i].strEmail);
                    }
                }
                if (MailOCC != null)
                {
                    for (int i = 0; i < MailOCC.Count; i++)
                    {
                        _objMailMessage.Bcc.Add(MailOCC[i].strEmail);
                    }
                }
                _objMailMessage.From = new MailAddress(strFrom);
                _objMailMessage.Subject = strSubject;
                _objMailMessage.IsBodyHtml = true;
                _objMailMessage.Body = strMessage;
                _objMailMessage.Priority = MailPriority.Normal;


                SmtpClient _smtpClient = new SmtpClient();
                _smtpClient.Host = strSMTP;
                _smtpClient.Port = Convert.ToInt32(intPort);
                _smtpClient.UseDefaultCredentials = false;
                //_smtpClient.Credentials = new NetworkCredential(ConfigurationManager.AppSettings["MailSenderName"], ConfigurationManager.AppSettings["MailSenderPassword"]);
                _smtpClient.Credentials = new NetworkCredential("igorsalvador0621@outlook.com.br",""); // Create new e-mail for it
                _smtpClient.EnableSsl = true;
                _smtpClient.Send(_objMailMessage);

                return true;
            }
            catch
            {
                //throw ex;
                return false;
            }
        }
    }
}
