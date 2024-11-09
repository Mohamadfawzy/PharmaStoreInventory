using DataAccess.DomainModel;
using DataAccess.Helper;
using System.Net;
using System.Net.Mail;
namespace DataAccess.Services;

public class MailingService
{
    //private readonly string sender = "devfawzy@outlook.com";
    //private readonly string senderPassword = "Gimro7-Honwf7-pbwoz7";

    private readonly string sender = "modern.soft.2020@gmail.com";
    private readonly string senderPassword = "mbzi wkby cqil vgrv ";

    /// <summary>
    /// Sends a verification code to the specified email Address.
    /// </summary>
    /// <param name="mailTo">The email Address to send the verification code to.</param>
    /// <param name="verificationCode">The verification code to send. If null, a new code will be generated.</param>
    /// <param name="userFullName">The full name of the user. If null, an empty string will be used.</param>
    /// <param name="lang">The language for the email content (optional).</param>
    /// <returns>The verification code sent.</returns>
    public async Task<Result<string>> SendVerificationCodeAsync(string mailTo, string? verificationCode, string userFullName = "", string? lang = null)
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
                                 
                                      <p>Thank you for registering with our service! To complete your account registration and ensure the security of your account, we require you to verify your email Address.</p>
                                 
                                      <p><strong>Verification Code: <span class='verification-code'>{verificationCode}</span></strong></p>
                                 
                                      <p>To verify your email Address, please follow these steps{lang}:</p>
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

        var res = await SendEmailAsync(mailTo, subject, htmlBody);
        if (res)
            return Result<string>.Success(verificationCode);
        return Result<string>.Failure();
    }

    /// <summary>
    /// Sends an email with the specified subject and body to the specified recipient.
    /// </summary>
    /// <param name="mailTo">The recipient's email Address.</param>
    /// <param name="subject">The subject of the email.</param>
    /// <param name="body">The body of the email.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task<bool> SendEmailAsync(string mailTo, string subject, string body)
    {
        //string smtp = "smtp-mail.outlook.com";
        string smtp = "smtp.gmail.com";
        try
        {
            await Task.Run(() =>
            {
                var message = new MailMessage
                {
                    From = new MailAddress(sender),
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true
                };
                message.To.Add(new MailAddress(mailTo));

                var smtpClient = new SmtpClient(smtp)
                {
                    Port = 587,
                    Credentials = new NetworkCredential(sender, senderPassword),
                    EnableSsl = true,
                };
                smtpClient.Send(message);
            });
            return true;
        }
        catch
        {
            return false;
        }
    }
}




/* SmtpClient smt = new SmtpClient();
smt.Host = "smtp.gmail.com";
System.Net.NetworkCredential ntcd = new NetworkCredential();
ntcd.UserName = "modern.soft.2020@gmail.com";
ntcd.Password = "mbzi wkby cqil vgrv ";
smt.Credentials = ntcd;
smt.EnableSsl = true;
smt.Port = 587;
smt.Send(msg); */
