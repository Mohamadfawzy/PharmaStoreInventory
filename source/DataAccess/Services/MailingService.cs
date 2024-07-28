using DataAccess.Helper;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;

namespace DataAccess.Services;

public class MailingService 
{
    public async Task SendEmailAsync(string mailTo, string subject, string body)
    {
        SendEmail(mailTo, subject, body);
    }

    public async Task<string?> SendVerificationCodeAsync(string mailTo, string? verificationCode, string? userFullName = "", string? lang = null)
    {
        if (string.IsNullOrEmpty(mailTo))
        {
            return null;
        }
        if (verificationCode is null)
        {
            verificationCode = Common.GenerateVerificationCode();
        }
        if (userFullName == null)
            userFullName = "";

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

        string body = $@"Dear {userFullName},

                        Thank you for registering with our service! To complete your account registration and ensure the security of your account, we require you to verify your email address.

                        *Verification Code*: {verificationCode}

                        To verify your email address, please follow these steps:
                        1. Open our website/app and log in to your account.
                        2. Go to the verification page or click on the verification link provided.
                        3. Enter the above verification code when prompted.

                        If you did not initiate this registration, please disregard this email.

                        Thank you for being a part of our community!

                        Best regards,
                        Modern soft Team";

        SendEmail(mailTo, subject, htmlBody);
        return verificationCode;
    }

    private async void SendEmail(string mailTo, string subject, string body)
    {
        string sender = "devfawzy@outlook.com";
        string senderPassword = "Micro9-Tomsu7-quton8";


        var email = new MimeMessage
        {
            Sender = MailboxAddress.Parse(sender),
            Subject = subject
        };

        email.To.Add(MailboxAddress.Parse("mohamedfawzy733@yahoo.com"));

        var builder = new BodyBuilder();

        builder.HtmlBody = body;
        email.Body = builder.ToMessageBody();
        email.From.Add(new MailboxAddress("ModernSoft", sender));

        using var smtp = new SmtpClient();
        smtp.Connect("smtp-mail.outlook.com", 587, SecureSocketOptions.StartTls);
        smtp.Authenticate(sender, senderPassword);

        await smtp.SendAsync(email);
        Console.WriteLine("Email sent successfully.");

        smtp.Disconnect(true);
    }
}