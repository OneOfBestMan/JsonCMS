//using FluentEmail.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JsonCMS.Models.Core
{
    public class Mail
    {
        public static async Task<bool> SendMail(string fromEmail, string toName, string toEmail, string subject, string body) {

            //var email = Email
            //    .From(fromEmail)
            //    .To(toEmail, toName)
            //    .Subject(subject)
            //    .Body(body);

            //var result = await email.SendAsync();

            //return result.Successful;
            return false;
        }

    }
}
