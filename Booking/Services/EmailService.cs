namespace Booking.Services
{
    using SendGrid;
    using SendGrid.Helpers.Mail;
    using System;
    using System.Threading.Tasks;

    public class EmailService
    {
        public async Task SendEmailAsync(string toEmail, string poruka)
        {
            var apiKey = ""; // zamijeni ako koristiš appsettings
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress("bookingooad@gmail.com", "Booking.com");
            var subject = "Booking Rezervacija";
            var to = new EmailAddress(toEmail, "Gost");
            var plainTextContent = "Zasto ovaj tekst ne prolazii, ili mozda prolazi???";
            var htmlContent = poruka;
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            var response = await client.SendEmailAsync(msg);
        }
    }
}
