using DataAccess.Helper;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;

namespace DataAccess.Services;

public class MailingService
{
    private readonly string sender = "devfawzy@outlook.com";
    private readonly string senderPassword = "Gimro7-Honwf7-pbwoz7";

    /// <summary>
    /// Sends a verification code to the specified email address.
    /// </summary>
    /// <param name="mailTo">The email address to send the verification code to.</param>
    /// <param name="verificationCode">The verification code to send. If null, a new code will be generated.</param>
    /// <param name="userFullName">The full name of the user. If null, an empty string will be used.</param>
    /// <param name="lang">The language for the email content (optional).</param>
    /// <returns>The verification code sent.</returns>
    public async Task<string> SendVerificationCodeAsync(string mailTo, string? verificationCode, string userFullName = "", string? lang = null)
    {
        //if (string.IsNullOrEmpty(mailTo))
        //{
        //    return null;
        //}

        // Generate a new verification code if none was provided
        verificationCode ??= Common.GenerateVerificationCode();


        // Define the email subject and body
        string subject = "Email Verification Code - Please Confirm Your Account";
        string htmlBody = $@"<html>
                                  <head>
                                      <style>
                                          p {{
                                              font-size: 16px; 
                                          }}
                                          .verification-code {{
                                              font-size: 25px;
                                              font-weight: bold;
                                          }}
                                      </style>
                                  </head>
                                  <body>
                                      <p>Dear {userFullName},</p>
                                 
                                      <p>Thank you for registering with our service! To complete your account registration and ensure the security of your account, we require you to verify your email address.</p>
                                 
                                      <p><strong>Verification Code: <span class='verification-code'>{verificationCode}</span></strong></p>
                                 
                                      <p>To verify your email address, please follow these steps:</p>
                                      <ol>
                                          <li>Open our website/app and log in to your account.</li>
                                          <li>Go to the verification page or click on the verification link provided.</li>
                                          <li>Enter the above verification code when prompted.</li>
                                      </ol>
                                 
                                      <p>If you did not initiate this registration, please disregard this email.</p>
                                 
                                      <p>Thank you for being a part of our community!</p>
                                 
                                      <p>Best regards,</p>
                                      <p>Modern soft Team</p>
                                  </body>
                                </html>";

        await SendEmailAsync(mailTo, subject, htmlBody);
        return verificationCode;
    }

    /// <summary>
    /// Sends an email with the specified subject and body to the specified recipient.
    /// </summary>
    /// <param name="mailTo">The recipient's email address.</param>
    /// <param name="subject">The subject of the email.</param>
    /// <param name="body">The body of the email.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task SendEmailAsync(string mailTo, string subject, string body)
    {
        try
        {
            var email = new MimeMessage
            {
                Sender = MailboxAddress.Parse(sender),
                Subject = subject
            };

            email.To.Add(MailboxAddress.Parse(mailTo));
            email.From.Add(new MailboxAddress("ModernSoft", sender));

            var builder = new BodyBuilder
            {
                HtmlBody = body
            };

            email.Body = builder.ToMessageBody();

            using var smtp = new SmtpClient();

            // Connect to the SMTP server with TLS
            await smtp.ConnectAsync("smtp-mail.outlook.com", 587, SecureSocketOptions.StartTls);

            // Authenticate
            await smtp.AuthenticateAsync(sender, senderPassword);

            // Send email
            await smtp.SendAsync(email);
            Console.WriteLine("Email sent successfully.");

            // Disconnect and quit
            await smtp.DisconnectAsync(true);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
    }

    //public async Task SendEmailAsync(string mailTo, string subject, string body)
    //{
    //    var email = new MimeMessage
    //    {
    //        Sender = MailboxAddress.Parse(sender),
    //        Subject = subject
    //    };

    //    email.To.Add(MailboxAddress.Parse(mailTo));

    //    var builder = new BodyBuilder
    //    {
    //        HtmlBody = body
    //    };

    //    email.Body = builder.ToMessageBody();
    //    email.From.Add(new MailboxAddress("ModernSoft", sender));

    //    using var smtp = new SmtpClient();
    //    smtp.Connect("smtp-mail.outlook.com", 587, SecureSocketOptions.StartTlsWhenAvailable);
    //    smtp.Authenticate(sender, senderPassword);

    //    await smtp.SendAsync(email);
    //    Console.WriteLine("Email sent successfully.");

    //    smtp.Disconnect(true);
    //}


    string body = $@"Dear #userFullName#,

                        Thank you for registering with our service! To complete your account registration and ensure the security of your account, we require you to verify your email address.

                        *Verification Code*: #verificationCode#

                        To verify your email address, please follow these steps:
                        1. Open our website/app and log in to your account.
                        2. Go to the verification page or click on the verification link provided.
                        3. Enter the above verification code when prompted.

                        If you did not initiate this registration, please disregard this email.

                        Thank you for being a part of our community!

                        Best regards,
                        Modern soft Team";

}