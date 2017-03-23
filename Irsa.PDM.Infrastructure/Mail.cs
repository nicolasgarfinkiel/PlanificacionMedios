using System.Collections.Generic;
using System.Net.Mail;
using System.Web;

namespace Irsa.PDM.Infrastructure
{
    public static class Mail
    {
        public static bool SendEmail(string to, string subject, string body, bool isBodyHtml = false, string from = null, IList<HttpPostedFileBase> files = null, IList<CustomFileBase> actualFiles = null, string name = null)
        {
            var message = new MailMessage();

            var toList = to.Split(new[] {';'});

            foreach (var t in toList)
            {
                message.To.Add(t);                
            }
            
            message.Subject = subject;
            message.Body = body;
            message.IsBodyHtml = isBodyHtml;

            if (!string.IsNullOrEmpty(from))
            {
                message.From = new MailAddress(from, name);
            }

            if (files != null)
            {
                foreach (var file in files)
                {
                    message.Attachments.Add(new Attachment(file.InputStream, file.FileName, file.ContentType));  
                }
            }

            if (actualFiles != null)
            {
                foreach (var file in actualFiles)
                {
                    message.Attachments.Add(new Attachment(file.Stream, file.FileName));
                }
            }

            var client = new SmtpClient();
            client.Send(message);


            return true;
        }
    }
}
