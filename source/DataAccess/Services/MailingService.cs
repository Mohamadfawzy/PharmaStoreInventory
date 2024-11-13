using DataAccess.DomainModel;
using DataAccess.Helper;
using System.Net;
using System.Net.Mail;
namespace DataAccess.Services;

public class MailingService
{
    //private readonly string sender = "modern.soft.2020@gmail.com";
    //private readonly string senderPassword = "mbzi wkby cqil vgrv ";

    string sender = "devfawze@gmail.com";
    string senderPassword = "tlsyvdaurcubqwbi";

    public async Task<Result<string>> SendVerificationCodeAsync(string mailTo, string? verificationCode, string userFullName = "", string? lang = null)
    {
        verificationCode ??= Common.GenerateVerificationCode();

        string subject = "Email Verification Code - Please Confirm Your Account";
        var res = await SendEmailAsync(mailTo, subject, Common.GenerateEmailBody(verificationCode, userFullName));
        if (res)
            return Result<string>.Success(verificationCode);
        return Result<string>.Failure();
    }
    
    public async Task<Result<string>> SendVerificationCodeAsync(EmailRequestModel emailRequestModel)
    {
        emailRequestModel.VerificationCode ??= Common.GenerateVerificationCode();

        string subject = "Email Verification Code - Please Confirm Your Account";
        string body = Common.GenerateEmailBody(emailRequestModel.VerificationCode, emailRequestModel.UserFullName);

        var res = await SendEmailAsync(emailRequestModel.Recipient, subject, body);
        if (res)
            return Result<string>.Success(emailRequestModel.VerificationCode);
        return Result<string>.Failure();
    }

    private async Task<bool> SendEmailAsync(string mailTo, string subject, string body)
    {
        string smtpHost = "smtp.gmail.com";
        try
        {
            using var message = new MailMessage
            {
                From = new MailAddress(sender),
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            };
            message.To.Add(new MailAddress(mailTo));

            using var smtpClient = new SmtpClient(smtpHost)
            {
                Port = 587,
                Credentials = new NetworkCredential(sender, senderPassword),
                EnableSsl = true,
            };

            await smtpClient.SendMailAsync(message);
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

/*
 
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
 */
