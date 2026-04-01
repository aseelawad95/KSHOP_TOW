using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace KSHOP_TWO.BLL.Service
{
    public class EmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string message)
        {
            var client = new SmtpClient("smtp.gmail.com", 587)
            {
                EnableSsl = true,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential("aseelawad252@gmail.com", "hdhw rqmk qbfm kcae")
            };

            return client.SendMailAsync(
                new MailMessage(from: "aseelawad252@gmail.com",
                                to: email,
                                subject,
                                message
                                )
                { IsBodyHtml=true}
                );
        }

        public Task SendEmailAsync(string? email, string v, object value)
        {
            throw new NotImplementedException();
        }
    }
}
//hdhw rqmk qbfm kcae\r\n