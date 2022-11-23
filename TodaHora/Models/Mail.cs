using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Net.Mail;
using System.Web.UI.WebControls;
using System.Net;
using System.Text;
using ClassLibrary_Email;

namespace TodaHora.Models
{
    public class Mail
    {
        //Mail styles
        private string strEstilo;
        private string strEstiloNegrito;
        private string strEstiloTitulo;

        private string strFrom;
        private string strSMTP;
        private int intPorta;
        public string Assunto;
        public ClassLibrary_Email.Email newUsername { get; set; }
        public string username;
        public ClassLibrary_Email.Email NewUserMail { get; set; }
        public string email;

        public Mail()
        {
            strFrom = ConfigurationManager.AppSettings["MailSenderName"];
            strSMTP = ConfigurationManager.AppSettings["SMTP"];
            intPorta = Convert.ToInt32(ConfigurationManager.AppSettings["SMTP_Port"]);
            strEstilo = "style=font-family:Arial;font-size:16;font-weight:normal;";
            strEstiloNegrito = "style=font-family:Verdana;font-size:16;font-weight:bold;";
            strEstiloTitulo = "style=font-family:Verdana;font-size:19;font-weight:bold;";
        }

        public bool UserMailPasswordDefiner(Mail mail)
        {
            try
            {
                ClassLibrary_Email.CEmail objM = new ClassLibrary_Email.CEmail();
                List<ClassLibrary_Email.Email> mailTo = new List<ClassLibrary_Email.Email>();

                Email email = new Email();
                email.strEmail = mail.email;

                mailTo.Add(email);
                StringBuilder sb = new StringBuilder();

                #region Corpo do e-mail 

                sb.AppendFormat("<span {0}> Olá, {1}! Tudo bem? Esperamos que sim. </span>", strEstilo, mail.username);

                #endregion

                bool blnRes = objM.SendMail(mailTo, null, null, strFrom, mail.Assunto, sb.ToString(), strSMTP, intPorta);

                return blnRes;

            }
            catch (Exception ex)
            {
                //Trata-se de envio de e-mail (o que não é essencial), não faz nada.
                throw ex;
            }
        }

        
    }
}