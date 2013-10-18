using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;

namespace iTOLEDO.Classes
{
   public static class mEmail
    {
       public static Boolean Send(String Message)
       {
           Boolean _mailSend = false;
           try
           {
               MailMessage mail = new MailMessage();
               SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");

               mail.From = new MailAddress("apatil@kraususa.com");
               mail.To.Add("kbedi@kraususa.com");
               mail.To.Add("apatil@kraususa.com");
               mail.Subject =  DateTime.Now.ToString("MMM dd, yyyy hh:mm tt")+ " -iTOLEDO Application Error alert "; 
               mail.Body = "iTOLEDO application got error : " +Environment.NewLine+ Message + Environment.NewLine + "Please find the attachment For more detail description Of Error.";
               System.Net.Mail.Attachment attachment;
               attachment = new System.Net.Mail.Attachment(Environment.CurrentDirectory + "\\Resources\\ErrorLog.txt");
               mail.Attachments.Add(attachment);

               SmtpServer.Port = 587;
               SmtpServer.Credentials = new System.Net.NetworkCredential("apatil@kraususa.com", "apatil@shiva");
               SmtpServer.EnableSsl = true;

               SmtpServer.Send(mail);
               
               //On success mail send 
               _mailSend = true;
           }
           catch (Exception ex)
           {
           }
           return _mailSend;
       }
    }
}
