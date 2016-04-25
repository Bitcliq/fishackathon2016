using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Mail;
using System.Threading;

namespace Bitcliq.BIR.Utils
{
    public class SendMail
    {
        private string From;
        private string To;
        private string Subject;
        private string Body;
        private string Attachment;
        public SendMail(string from, string to, string subject, string body, string attachment)
        {
            this.From = from;
            this.To = to;
            this.Subject = subject;
            this.Body = body;
            this.Attachment = attachment;
            //Start();
            //Send();

            Thread newThread = new Thread(new ThreadStart(Send));
            newThread.Start();

        }

        


        public void Send()
        {
         
                MailMessage mail = new MailMessage(this.From, this.To, this.Subject, this.Body);

                mail.IsBodyHtml = true;

                SmtpClient smpt = new SmtpClient(StaticKeys.MailServer);

                string password = StaticKeys.MailServerPassword;
                if (password != "")
                    smpt.Credentials = new System.Net.NetworkCredential(this.From, password);

                if (Attachment + "" != "")
                    mail.Attachments.Add(new Attachment(Attachment));

                smpt.Send(mail);
           
        }
    }
}
