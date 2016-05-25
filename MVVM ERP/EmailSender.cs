using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace MVVM_ERP
{
    public class EmailSender
    {
        MailMessage mail;
        public EmailSender()
        {

        }
        public bool SendMessage(string receiverAddress, string body, int rptId)
        {
            try
            {
                mail = new MailMessage();
                mail.From = new MailAddress("sipcaller@redapptests.com");
                mail.To.Add(receiverAddress);
                mail.Subject = "Cambio en estado de reporte " + rptId.ToString();
                mail.Body = body;
                mail.IsBodyHtml = true;
                using (SmtpClient client = new SmtpClient())
                {
                    client.UseDefaultCredentials = false;
                    client.Credentials = new NetworkCredential("sipcaller@redapptests.com", "z%gfPKBv1sQU");
                    client.Host = "mail.redapptests.com";
                    client.Port = 25;
                    client.Send(mail);
                }
                return true;
            }
            catch
            {
                return false;
            }

        }
    }
}
