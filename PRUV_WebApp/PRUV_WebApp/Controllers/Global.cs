using PRUV_WebApp.Models;
using System.Net.Mail;

namespace PRUV_WebApp.Controllers
{
    public static class Global
    {
        // store user info across views
        public static int empNum = 0;
        public static int empLoc = 0;
        public static string vintageBuys = "vintagebuys@guitarcenter.com";

        public static byte[] ConvertImageFile(IFormFile imageFile)
        {
            byte[]? image = null;

            
            if (imageFile.Length > 0)
            {
                using (var ms = new MemoryStream())
                {
                    imageFile.CopyTo(ms);
                    image = ms.ToArray();

                }


            }

            return image;

        }



        public static void SendNotification(JoinedRequest request)
        {
            MailMessage mail = new MailMessage();
            mail.To.Add(vintageBuys);
            mail.To.Add(request.Email);
            mail.From = new MailAddress("timeggenbergergc@gmail.com");
            mail.Subject = ComposeEmailSubject(request);
            string Body = ComposeEmailBody(request);
            mail.Body = Body;
            mail.IsBodyHtml = true;
            SmtpClient smtp = new SmtpClient();
            smtp.Host = "smtp.gmail.com";
            smtp.Port = 587;
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = new System.Net.NetworkCredential("Timeggenbergergc", "wyxullvsplgdoqeu"); // Enter senders User name and password       
            smtp.EnableSsl = true;
            smtp.Send(mail);
        }

        private static string ComposeEmailBody(JoinedRequest request)
        {
            string body = "Greetings Used and Vintage,";
            body += $"<br/><p>&emsp;{request.UserName} has requested assistance with pricing on the following item:";
            body += $"<br/><br>{request.Year} {request.Brand} {request.Model}" ;
            return body;
        }

        private static string ComposeEmailSubject(JoinedRequest request)
        {
            return  $"New Pricing Request {request.RequestID} {request.Created}";
        }
    }
}
